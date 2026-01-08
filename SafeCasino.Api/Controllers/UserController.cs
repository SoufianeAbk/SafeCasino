using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SafeCasino.Data.Data;
using SafeCasino.Data.Entities;
using SafeCasino.Data.Identity;
using SafeCasino.Shared.DTOs;
using SafeCasino.Shared.Responses;

namespace SafeCasino.Api.Controllers
{
    /// <summary>
    /// Controller voor gebruikersbeheer en profiel
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserController> _logger;

        public UserController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            ILogger<UserController> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Haal het profiel van de huidige gebruiker op
        /// </summary>
        [HttpGet("profile")]
        public async Task<ActionResult<ApiResponse<UserDto>>> GetProfile()
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ApiResponse<UserDto>.UnauthorizedResponse());
                }

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound(ApiResponse<UserDto>.NotFoundResponse(
                        "Gebruiker niet gevonden"));
                }

                var roles = await _userManager.GetRolesAsync(user);
                var reviewCount = await _context.Reviews
                    .CountAsync(r => r.UserId == userId);

                var userDto = new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    DateOfBirth = user.DateOfBirth,
                    Balance = user.Balance,
                    RegistrationDate = user.RegistrationDate,
                    EmailConfirmed = user.EmailConfirmed,
                    IsVerified = user.IsVerified,
                    Roles = roles.ToList(),
                    IsLockedOut = await _userManager.IsLockedOutAsync(user),
                    ReviewCount = reviewCount
                };

                return Ok(ApiResponse<UserDto>.SuccessResponse(userDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij ophalen van gebruikersprofiel");
                return StatusCode(500, ApiResponse<UserDto>.ErrorResponse(
                    "Er is een fout opgetreden", 500));
            }
        }

        /// <summary>
        /// Update profiel gegevens
        /// </summary>
        [HttpPut("profile")]
        public async Task<ActionResult<ApiResponse<UserDto>>> UpdateProfile(
            [FromBody] UpdateProfileRequest request)
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ApiResponse<UserDto>.UnauthorizedResponse());
                }

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound(ApiResponse<UserDto>.NotFoundResponse(
                        "Gebruiker niet gevonden"));
                }

                // Update alleen toegestane velden
                user.FirstName = request.FirstName;
                user.LastName = request.LastName;

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    var errors = result.Errors.ToDictionary(
                        e => e.Code,
                        e => new List<string> { e.Description }
                    );
                    return BadRequest(ApiResponse<UserDto>.ValidationErrorResponse(
                        errors, "Update mislukt"));
                }

                var roles = await _userManager.GetRolesAsync(user);
                var reviewCount = await _context.Reviews
                    .CountAsync(r => r.UserId == userId);

                var userDto = new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    DateOfBirth = user.DateOfBirth,
                    Balance = user.Balance,
                    RegistrationDate = user.RegistrationDate,
                    EmailConfirmed = user.EmailConfirmed,
                    IsVerified = user.IsVerified,
                    Roles = roles.ToList(),
                    IsLockedOut = await _userManager.IsLockedOutAsync(user),
                    ReviewCount = reviewCount
                };

                _logger.LogInformation("Profiel bijgewerkt voor gebruiker {UserId}", userId);

                return Ok(ApiResponse<UserDto>.SuccessResponse(
                    userDto, "Profiel succesvol bijgewerkt"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij bijwerken van profiel");
                return StatusCode(500, ApiResponse<UserDto>.ErrorResponse(
                    "Er is een fout opgetreden", 500));
            }
        }

        /// <summary>
        /// Haal het huidige saldo op
        /// </summary>
        [HttpGet("balance")]
        public async Task<ActionResult<ApiResponse<object>>> GetBalance()
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ApiResponse<object>.UnauthorizedResponse());
                }

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound(ApiResponse<object>.NotFoundResponse(
                        "Gebruiker niet gevonden"));
                }

                var result = new
                {
                    Balance = user.Balance,
                    LastUpdated = DateTime.Now
                };

                return Ok(ApiResponse<object>.SuccessResponse(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij ophalen van saldo");
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "Er is een fout opgetreden", 500));
            }
        }

        /// <summary>
        /// Voeg saldo toe (voor test doeleinden - normaal via payment provider)
        /// </summary>
        [HttpPost("balance/add")]
        public async Task<ActionResult<ApiResponse<object>>> AddBalance(
            [FromBody] UpdateBalanceRequest request)
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ApiResponse<object>.UnauthorizedResponse());
                }

                if (request.Amount <= 0)
                {
                    return BadRequest(ApiResponse<object>.ErrorResponse(
                        "Bedrag moet groter zijn dan 0"));
                }

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound(ApiResponse<object>.NotFoundResponse(
                        "Gebruiker niet gevonden"));
                }

                user.Balance += request.Amount;
                await _userManager.UpdateAsync(user);

                _logger.LogInformation("Saldo verhoogd met €{Amount} voor gebruiker {UserId}",
                    request.Amount, userId);

                var result = new
                {
                    OldBalance = user.Balance - request.Amount,
                    NewBalance = user.Balance,
                    AmountAdded = request.Amount
                };

                return Ok(ApiResponse<object>.SuccessResponse(
                    result, $"€{request.Amount:F2} toegevoegd aan je saldo"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij toevoegen van saldo");
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "Er is een fout opgetreden", 500));
            }
        }

        /// <summary>
        /// Haal reviews van de gebruiker op
        /// </summary>
        [HttpGet("reviews")]
        public async Task<ActionResult<ApiResponse<List<ReviewDto>>>> GetUserReviews()
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ApiResponse<List<ReviewDto>>.UnauthorizedResponse());
                }

                var reviews = await _context.Reviews
                    .Include(r => r.Game)
                    .Include(r => r.User)
                    .Where(r => r.UserId == userId)
                    .OrderByDescending(r => r.CreatedDate)
                    .Select(r => new ReviewDto
                    {
                        Id = r.Id,
                        Title = r.Title,
                        Content = r.Content,
                        Rating = r.Rating,
                        CreatedDate = r.CreatedDate,
                        IsApproved = r.IsApproved,
                        GameId = r.GameId,
                        GameName = r.Game.Name,
                        UserId = r.UserId,
                        UserName = $"{r.User.FirstName} {r.User.LastName}"
                    })
                    .ToListAsync();

                return Ok(ApiResponse<List<ReviewDto>>.SuccessResponse(
                    reviews, $"{reviews.Count} reviews gevonden"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij ophalen van gebruikersreviews");
                return StatusCode(500, ApiResponse<List<ReviewDto>>.ErrorResponse(
                    "Er is een fout opgetreden", 500));
            }
        }

        /// <summary>
        /// Plaas een nieuwe review voor een game
        /// </summary>
        [HttpPost("reviews")]
        public async Task<ActionResult<ApiResponse<ReviewDto>>> CreateReview(
            [FromBody] CreateReviewRequest request)
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ApiResponse<ReviewDto>.UnauthorizedResponse());
                }

                // Check of game bestaat
                var gameExists = await _context.CasinoGames
                    .AnyAsync(g => g.Id == request.GameId);
                if (!gameExists)
                {
                    return BadRequest(ApiResponse<ReviewDto>.ErrorResponse(
                        "Game niet gevonden"));
                }

                // Check of gebruiker al een review heeft voor deze game
                var existingReview = await _context.Reviews
                    .AnyAsync(r => r.UserId == userId && r.GameId == request.GameId);
                if (existingReview)
                {
                    return BadRequest(ApiResponse<ReviewDto>.ErrorResponse(
                        "Je hebt al een review voor deze game geschreven"));
                }

                var review = new Review
                {
                    Title = request.Title,
                    Content = request.Content,
                    Rating = request.Rating,
                    GameId = request.GameId,
                    UserId = userId,
                    CreatedDate = DateTime.Now,
                    IsApproved = false // Reviews moeten goedgekeurd worden
                };

                _context.Reviews.Add(review);
                await _context.SaveChangesAsync();

                // Laad relaties voor DTO
                await _context.Entry(review).Reference(r => r.Game).LoadAsync();
                await _context.Entry(review).Reference(r => r.User).LoadAsync();

                var reviewDto = new ReviewDto
                {
                    Id = review.Id,
                    Title = review.Title,
                    Content = review.Content,
                    Rating = review.Rating,
                    CreatedDate = review.CreatedDate,
                    IsApproved = review.IsApproved,
                    GameId = review.GameId,
                    GameName = review.Game.Name,
                    UserId = review.UserId,
                    UserName = $"{review.User.FirstName} {review.User.LastName}"
                };

                _logger.LogInformation("Review aangemaakt voor game {GameId} door gebruiker {UserId}",
                    request.GameId, userId);

                return Ok(ApiResponse<ReviewDto>.SuccessResponse(
                    reviewDto,
                    "Review succesvol aangemaakt. Je review wordt beoordeeld door moderators.",
                    201));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij aanmaken van review");
                return StatusCode(500, ApiResponse<ReviewDto>.ErrorResponse(
                    "Er is een fout opgetreden", 500));
            }
        }

        /// <summary>
        /// Update een bestaande review
        /// </summary>
        [HttpPut("reviews/{id}")]
        public async Task<ActionResult<ApiResponse<ReviewDto>>> UpdateReview(
            int id, [FromBody] CreateReviewRequest request)
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ApiResponse<ReviewDto>.UnauthorizedResponse());
                }

                var review = await _context.Reviews
                    .Include(r => r.Game)
                    .Include(r => r.User)
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (review == null)
                {
                    return NotFound(ApiResponse<ReviewDto>.NotFoundResponse(
                        "Review niet gevonden"));
                }

                // Check of gebruiker eigenaar is
                if (review.UserId != userId)
                {
                    return Forbid();
                }

                review.Title = request.Title;
                review.Content = request.Content;
                review.Rating = request.Rating;
                review.IsApproved = false; // Review moet opnieuw goedgekeurd worden

                await _context.SaveChangesAsync();

                var reviewDto = new ReviewDto
                {
                    Id = review.Id,
                    Title = review.Title,
                    Content = review.Content,
                    Rating = review.Rating,
                    CreatedDate = review.CreatedDate,
                    IsApproved = review.IsApproved,
                    GameId = review.GameId,
                    GameName = review.Game.Name,
                    UserId = review.UserId,
                    UserName = $"{review.User.FirstName} {review.User.LastName}"
                };

                _logger.LogInformation("Review {ReviewId} bijgewerkt door gebruiker {UserId}",
                    id, userId);

                return Ok(ApiResponse<ReviewDto>.SuccessResponse(
                    reviewDto, "Review succesvol bijgewerkt"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij bijwerken van review {ReviewId}", id);
                return StatusCode(500, ApiResponse<ReviewDto>.ErrorResponse(
                    "Er is een fout opgetreden", 500));
            }
        }

        /// <summary>
        /// Verwijder een review
        /// </summary>
        [HttpDelete("reviews/{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteReview(int id)
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ApiResponse.UnauthorizedResponse());
                }

                var review = await _context.Reviews.FindAsync(id);
                if (review == null)
                {
                    return NotFound(ApiResponse.NotFoundResponse("Review niet gevonden"));
                }

                // Check of gebruiker eigenaar is of admin
                var isAdmin = User.IsInRole("Admin");
                if (review.UserId != userId && !isAdmin)
                {
                    return Forbid();
                }

                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Review {ReviewId} verwijderd door gebruiker {UserId}",
                    id, userId);

                return Ok(ApiResponse.SuccessResponse("Review succesvol verwijderd"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij verwijderen van review {ReviewId}", id);
                return StatusCode(500, ApiResponse.ErrorResponse(
                    "Er is een fout opgetreden", 500));
            }
        }

        /// <summary>
        /// Keur een review goed (alleen Admin/Moderator)
        /// </summary>
        [Authorize(Roles = "Admin,Moderator")]
        [HttpPost("reviews/{id}/approve")]
        public async Task<ActionResult<ApiResponse>> ApproveReview(int id)
        {
            try
            {
                var review = await _context.Reviews.FindAsync(id);
                if (review == null)
                {
                    return NotFound(ApiResponse.NotFoundResponse("Review niet gevonden"));
                }

                review.IsApproved = true;
                await _context.SaveChangesAsync();

                _logger.LogInformation("Review {ReviewId} goedgekeurd door {UserId}",
                    id, _userManager.GetUserId(User));

                return Ok(ApiResponse.SuccessResponse("Review goedgekeurd"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij goedkeuren van review {ReviewId}", id);
                return StatusCode(500, ApiResponse.ErrorResponse(
                    "Er is een fout opgetreden", 500));
            }
        }

        /// <summary>
        /// Haal alle gebruikers op (alleen Admin)
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<ActionResult<ApiResponse<List<UserDto>>>> GetAllUsers(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20)
        {
            try
            {
                var totalUsers = await _context.Users.CountAsync();
                var users = await _context.Users
                    .OrderByDescending(u => u.RegistrationDate)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var userDtos = new List<UserDto>();

                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    var reviewCount = await _context.Reviews.CountAsync(r => r.UserId == user.Id);

                    userDtos.Add(new UserDto
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        DateOfBirth = user.DateOfBirth,
                        Balance = user.Balance,
                        RegistrationDate = user.RegistrationDate,
                        EmailConfirmed = user.EmailConfirmed,
                        IsVerified = user.IsVerified,
                        Roles = roles.ToList(),
                        IsLockedOut = await _userManager.IsLockedOutAsync(user),
                        ReviewCount = reviewCount
                    });
                }

                return Ok(ApiResponse<List<UserDto>>.SuccessResponse(
                    userDtos, $"{totalUsers} gebruikers gevonden"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij ophalen van alle gebruikers");
                return StatusCode(500, ApiResponse<List<UserDto>>.ErrorResponse(
                    "Er is een fout opgetreden", 500));
            }
        }
    }

    // Request models
    public class UpdateProfileRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }

    public class UpdateBalanceRequest
    {
        public decimal Amount { get; set; }
    }

    public class CreateReviewRequest
    {
        public int GameId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int Rating { get; set; } // 1-5
    }
}