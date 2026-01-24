using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SafeCasino.Data.Data;
using SafeCasino.Data.Identity;
using SafeCasino.Web.ViewModels.Admin;

namespace SafeCasino.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ILogger<AdminController> _logger;

        public AdminController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            ILogger<AdminController> logger)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        // Dashboard
        public async Task<IActionResult> Index()
        {
            var viewModel = new AdminDashboardViewModel
            {
                TotalUsers = await _userManager.Users.CountAsync(),
                TotalGames = await _context.CasinoGames.CountAsync(),
                PendingReviews = await _context.Reviews.CountAsync(r => !r.IsApproved),
                ActiveUsers = await _userManager.Users.CountAsync(u => !u.LockoutEnd.HasValue || u.LockoutEnd <= DateTimeOffset.Now)
            };

            return View(viewModel);
        }

        // ============ USER MANAGEMENT ============

        [HttpGet]
        public async Task<IActionResult> Users(string search, int page = 1, int pageSize = 20)
        {
            var query = _userManager.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(u =>
                    u.Email.Contains(search) ||
                    u.FirstName.Contains(search) ||
                    u.LastName.Contains(search));
            }

            var totalUsers = await query.CountAsync();
            var users = await query
                .OrderByDescending(u => u.RegistrationDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var userViewModels = new List<UserManagementViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userViewModels.Add(new UserManagementViewModel
                {
                    User = user,
                    Roles = roles.ToList(),
                    IsLocked = user.LockoutEnd.HasValue && user.LockoutEnd > DateTimeOffset.Now
                });
            }

            var viewModel = new UsersIndexViewModel
            {
                Users = userViewModels,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalUsers / (double)pageSize),
                SearchQuery = search
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> UserDetails(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var roles = await _userManager.GetRolesAsync(user);
            var allRoles = await _roleManager.Roles.ToListAsync();

            var viewModel = new UserDetailsViewModel
            {
                User = user,
                CurrentRoles = roles.ToList(),
                AvailableRoles = allRoles.Select(r => r.Name).ToList(),
                IsLocked = user.LockoutEnd.HasValue && user.LockoutEnd > DateTimeOffset.Now
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                TempData["Error"] = "Rol bestaat niet";
                return RedirectToAction(nameof(UserDetails), new { id = userId });
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);

            if (result.Succeeded)
            {
                _logger.LogInformation($"Admin assigned role {roleName} to user {user.Email}");
                TempData["Success"] = $"Rol '{roleName}' toegekend aan {user.Email}";
            }
            else
            {
                TempData["Error"] = "Fout bij toekennen rol: " + string.Join(", ", result.Errors.Select(e => e.Description));
            }

            return RedirectToAction(nameof(UserDetails), new { id = userId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);

            if (result.Succeeded)
            {
                _logger.LogInformation($"Admin removed role {roleName} from user {user.Email}");
                TempData["Success"] = $"Rol '{roleName}' verwijderd van {user.Email}";
            }
            else
            {
                TempData["Error"] = "Fout bij verwijderen rol: " + string.Join(", ", result.Errors.Select(e => e.Description));
            }

            return RedirectToAction(nameof(UserDetails), new { id = userId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LockUser(string userId, int days = 30)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            // Voorkom dat admin zichzelf blokkeert
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser.Id == userId)
            {
                TempData["Error"] = "Je kunt jezelf niet blokkeren";
                return RedirectToAction(nameof(UserDetails), new { id = userId });
            }

            var lockoutEnd = DateTimeOffset.Now.AddDays(days);
            var result = await _userManager.SetLockoutEndDateAsync(user, lockoutEnd);

            if (result.Succeeded)
            {
                _logger.LogWarning($"Admin locked user {user.Email} until {lockoutEnd}");
                TempData["Success"] = $"Gebruiker {user.Email} geblokkeerd tot {lockoutEnd:dd/MM/yyyy}";
            }
            else
            {
                TempData["Error"] = "Fout bij blokkeren gebruiker";
            }

            return RedirectToAction(nameof(UserDetails), new { id = userId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnlockUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            var result = await _userManager.SetLockoutEndDateAsync(user, null);

            if (result.Succeeded)
            {
                _logger.LogInformation($"Admin unlocked user {user.Email}");
                TempData["Success"] = $"Gebruiker {user.Email} gedeblokkeerd";
            }
            else
            {
                TempData["Error"] = "Fout bij deblokkeren gebruiker";
            }

            return RedirectToAction(nameof(UserDetails), new { id = userId });
        }

        // ============ REVIEW MODERATION ============

        [HttpGet]
        public async Task<IActionResult> Reviews(bool showApproved = false)
        {
            var query = _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Game)
                .AsQueryable();

            if (!showApproved)
            {
                query = query.Where(r => !r.IsApproved);
            }

            var reviews = await query
                .OrderByDescending(r => r.CreatedDate)
                .ToListAsync();

            var viewModel = new ReviewModerationViewModel
            {
                Reviews = reviews,
                ShowApproved = showApproved
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveReview(int reviewId)
        {
            var review = await _context.Reviews.FindAsync(reviewId);
            if (review == null)
                return NotFound();

            review.IsApproved = true;
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Admin approved review {reviewId}");
            TempData["Success"] = "Review goedgekeurd";

            return RedirectToAction(nameof(Reviews));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectReview(int reviewId)
        {
            var review = await _context.Reviews.FindAsync(reviewId);
            if (review == null)
                return NotFound();

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Admin rejected and deleted review {reviewId}");
            TempData["Success"] = "Review afgekeurd en verwijderd";

            return RedirectToAction(nameof(Reviews));
        }
    }
}