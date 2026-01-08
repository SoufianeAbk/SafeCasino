using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SafeCasino.Data.Data;
using SafeCasino.Data.Identity;
using SafeCasino.Shared.DTOs;
using SafeCasino.Shared.Requests;
using SafeCasino.Shared.Responses;

namespace SafeCasino.Api.Controllers
{
    /// <summary>
    /// Admin controller voor user en role management
    /// Alleen toegankelijk voor gebruikers met Admin rol
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AdminController> _logger;

        public AdminController(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            ApplicationDbContext context,
            ILogger<AdminController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _logger = logger;
        }

        // ============ USER MANAGEMENT ============

        /// <summary>
        /// Haal alle gebruikers op met paginering en zoeken
        /// </summary>
        [HttpGet("users")]
        public async Task<ActionResult<ApiResponse<List<AdminUserDto>>>> GetAllUsers(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? searchTerm = null)
        {
            try
            {
                var query = _context.Users.AsQueryable();

                // Zoeken op email, voornaam of achternaam
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    query = query.Where(u =>
                        u.Email.Contains(searchTerm) ||
                        u.FirstName.Contains(searchTerm) ||
                        u.LastName.Contains(searchTerm));
                }

                var totalUsers = await query.CountAsync();
                var users = await query
                    .OrderByDescending(u => u.RegistrationDate)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var userDtos = new List<AdminUserDto>();

                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    var isLockedOut = await _userManager.IsLockedOutAsync(user);
                    var reviewCount = await _context.Reviews.CountAsync(r => r.UserId == user.Id);

                    userDtos.Add(new AdminUserDto
                    {
                        Id = user.Id,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        DateOfBirth = user.DateOfBirth,
                        Balance = user.Balance,
                        RegistrationDate = user.RegistrationDate,
                        EmailConfirmed = user.EmailConfirmed,
                        IsVerified = user.IsVerified,
                        IsActive = user.IsActive,
                        IsLockedOut = isLockedOut,
                        Roles = roles.ToList(),
                        ReviewCount = reviewCount
                    });
                }

                return Ok(ApiResponse<List<AdminUserDto>>.SuccessResponse(
                    userDtos, $"{totalUsers} gebruikers gevonden"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij ophalen van gebruikers");
                return StatusCode(500, ApiResponse<List<AdminUserDto>>.ErrorResponse(
                    "Er is een fout opgetreden", 500));
            }
        }

        /// <summary>
        /// Haal specifieke gebruiker op via ID
        /// </summary>
        [HttpGet("users/{id}")]
        public async Task<ActionResult<ApiResponse<AdminUserDto>>> GetUser(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound(ApiResponse<AdminUserDto>.NotFoundResponse(
                        "Gebruiker niet gevonden"));
                }

                var roles = await _userManager.GetRolesAsync(user);
                var isLockedOut = await _userManager.IsLockedOutAsync(user);
                var reviewCount = await _context.Reviews.CountAsync(r => r.UserId == user.Id);

                var userDto = new AdminUserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    DateOfBirth = user.DateOfBirth,
                    Balance = user.Balance,
                    RegistrationDate = user.RegistrationDate,
                    EmailConfirmed = user.EmailConfirmed,
                    IsVerified = user.IsVerified,
                    IsActive = user.IsActive,
                    IsLockedOut = isLockedOut,
                    Roles = roles.ToList(),
                    ReviewCount = reviewCount
                };

                return Ok(ApiResponse<AdminUserDto>.SuccessResponse(userDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij ophalen van gebruiker {UserId}", id);
                return StatusCode(500, ApiResponse<AdminUserDto>.ErrorResponse(
                    "Er is een fout opgetreden", 500));
            }
        }

        /// <summary>
        /// Update gebruiker gegevens (admin kan alles wijzigen)
        /// </summary>
        [HttpPut("users/{id}")]
        public async Task<ActionResult<ApiResponse<AdminUserDto>>> UpdateUser(
            string id, [FromBody] UpdateUserRequest request)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound(ApiResponse<AdminUserDto>.NotFoundResponse(
                        "Gebruiker niet gevonden"));
                }

                // Update user properties
                user.FirstName = request.FirstName;
                user.LastName = request.LastName;
                user.Balance = request.Balance;
                user.IsActive = request.IsActive;
                user.IsVerified = request.IsVerified;
                user.EmailConfirmed = request.EmailConfirmed;

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    var errors = result.Errors.ToDictionary(
                        e => e.Code,
                        e => new List<string> { e.Description });
                    return BadRequest(ApiResponse<AdminUserDto>.ValidationErrorResponse(
                        errors, "Update mislukt"));
                }

                var roles = await _userManager.GetRolesAsync(user);
                var isLockedOut = await _userManager.IsLockedOutAsync(user);
                var reviewCount = await _context.Reviews.CountAsync(r => r.UserId == user.Id);

                var userDto = new AdminUserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    DateOfBirth = user.DateOfBirth,
                    Balance = user.Balance,
                    RegistrationDate = user.RegistrationDate,
                    EmailConfirmed = user.EmailConfirmed,
                    IsVerified = user.IsVerified,
                    IsActive = user.IsActive,
                    IsLockedOut = isLockedOut,
                    Roles = roles.ToList(),
                    ReviewCount = reviewCount
                };

                _logger.LogInformation("Gebruiker {UserId} bijgewerkt door admin", id);

                return Ok(ApiResponse<AdminUserDto>.SuccessResponse(
                    userDto, "Gebruiker succesvol bijgewerkt"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij bijwerken van gebruiker {UserId}", id);
                return StatusCode(500, ApiResponse<AdminUserDto>.ErrorResponse(
                    "Er is een fout opgetreden", 500));
            }
        }

        /// <summary>
        /// Verwijder gebruiker permanent (CASCADE delete via EF)
        /// </summary>
        [HttpDelete("users/{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteUser(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound(ApiResponse.NotFoundResponse("Gebruiker niet gevonden"));
                }

                // Voorkom dat admin zichzelf verwijdert
                var currentUserId = _userManager.GetUserId(User);
                if (id == currentUserId)
                {
                    return BadRequest(ApiResponse.ErrorResponse(
                        "Je kunt je eigen account niet verwijderen"));
                }

                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    return BadRequest(ApiResponse.ErrorResponse("Verwijderen mislukt"));
                }

                _logger.LogWarning("Gebruiker {UserId} ({Email}) verwijderd door admin",
                    id, user.Email);

                return Ok(ApiResponse.SuccessResponse("Gebruiker succesvol verwijderd"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij verwijderen van gebruiker {UserId}", id);
                return StatusCode(500, ApiResponse.ErrorResponse(
                    "Er is een fout opgetreden", 500));
            }
        }

        /// <summary>
        /// Vergrendel gebruiker (lockout)
        /// </summary>
        [HttpPost("users/{id}/lock")]
        public async Task<ActionResult<ApiResponse>> LockUser(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound(ApiResponse.NotFoundResponse("Gebruiker niet gevonden"));
                }

                // Voorkom dat admin zichzelf vergrendelt
                var currentUserId = _userManager.GetUserId(User);
                if (id == currentUserId)
                {
                    return BadRequest(ApiResponse.ErrorResponse(
                        "Je kunt je eigen account niet vergrendelen"));
                }

                // Vergrendel voor 100 jaar (permanent lockout)
                var result = await _userManager.SetLockoutEndDateAsync(
                    user, DateTimeOffset.Now.AddYears(100));

                if (!result.Succeeded)
                {
                    return BadRequest(ApiResponse.ErrorResponse("Vergrendelen mislukt"));
                }

                _logger.LogWarning("Gebruiker {UserId} ({Email}) vergrendeld door admin",
                    id, user.Email);

                return Ok(ApiResponse.SuccessResponse("Gebruiker succesvol vergrendeld"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij vergrendelen van gebruiker {UserId}", id);
                return StatusCode(500, ApiResponse.ErrorResponse(
                    "Er is een fout opgetreden", 500));
            }
        }

        /// <summary>
        /// Ontgrendel gebruiker (unlock)
        /// </summary>
        [HttpPost("users/{id}/unlock")]
        public async Task<ActionResult<ApiResponse>> UnlockUser(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound(ApiResponse.NotFoundResponse("Gebruiker niet gevonden"));
                }

                // Remove lockout
                var result = await _userManager.SetLockoutEndDateAsync(user, null);

                if (!result.Succeeded)
                {
                    return BadRequest(ApiResponse.ErrorResponse("Ontgrendelen mislukt"));
                }

                // Reset failed access count
                await _userManager.ResetAccessFailedCountAsync(user);

                _logger.LogInformation("Gebruiker {UserId} ({Email}) ontgrendeld door admin",
                    id, user.Email);

                return Ok(ApiResponse.SuccessResponse("Gebruiker succesvol ontgrendeld"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij ontgrendelen van gebruiker {UserId}", id);
                return StatusCode(500, ApiResponse.ErrorResponse(
                    "Er is een fout opgetreden", 500));
            }
        }

        // ============ ROLE MANAGEMENT ============

        /// <summary>
        /// Haal alle rollen op met user count
        /// </summary>
        [HttpGet("roles")]
        public async Task<ActionResult<ApiResponse<List<RoleDto>>>> GetAllRoles()
        {
            try
            {
                var roles = await _roleManager.Roles.ToListAsync();
                var roleDtos = new List<RoleDto>();

                foreach (var role in roles)
                {
                    var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name!);

                    roleDtos.Add(new RoleDto
                    {
                        Id = role.Id,
                        Name = role.Name!,
                        Description = role.Description,
                        IsActive = role.IsActive,
                        UserCount = usersInRole.Count
                    });
                }

                return Ok(ApiResponse<List<RoleDto>>.SuccessResponse(
                    roleDtos, $"{roleDtos.Count} rollen gevonden"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij ophalen van rollen");
                return StatusCode(500, ApiResponse<List<RoleDto>>.ErrorResponse(
                    "Er is een fout opgetreden", 500));
            }
        }

        /// <summary>
        /// Haal specifieke rol op
        /// </summary>
        [HttpGet("roles/{id}")]
        public async Task<ActionResult<ApiResponse<RoleDto>>> GetRole(string id)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role == null)
                {
                    return NotFound(ApiResponse<RoleDto>.NotFoundResponse("Rol niet gevonden"));
                }

                var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name!);

                var roleDto = new RoleDto
                {
                    Id = role.Id,
                    Name = role.Name!,
                    Description = role.Description,
                    IsActive = role.IsActive,
                    UserCount = usersInRole.Count
                };

                return Ok(ApiResponse<RoleDto>.SuccessResponse(roleDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij ophalen van rol {RoleId}", id);
                return StatusCode(500, ApiResponse<RoleDto>.ErrorResponse(
                    "Er is een fout opgetreden", 500));
            }
        }

        /// <summary>
        /// Haal alle rollen van een gebruiker op
        /// </summary>
        [HttpGet("users/{userId}/roles")]
        public async Task<ActionResult<ApiResponse<List<string>>>> GetUserRoles(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound(ApiResponse<List<string>>.NotFoundResponse(
                        "Gebruiker niet gevonden"));
                }

                var roles = await _userManager.GetRolesAsync(user);

                return Ok(ApiResponse<List<string>>.SuccessResponse(
                    roles.ToList(), $"{roles.Count} rollen gevonden"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij ophalen van gebruiker rollen");
                return StatusCode(500, ApiResponse<List<string>>.ErrorResponse(
                    "Er is een fout opgetreden", 500));
            }
        }

        /// <summary>
        /// Wijs rol toe aan gebruiker
        /// </summary>
        [HttpPost("users/{userId}/roles/{roleName}")]
        public async Task<ActionResult<ApiResponse>> AssignRole(string userId, string roleName)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound(ApiResponse.NotFoundResponse("Gebruiker niet gevonden"));
                }

                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    return BadRequest(ApiResponse.ErrorResponse("Rol bestaat niet"));
                }

                if (await _userManager.IsInRoleAsync(user, roleName))
                {
                    return BadRequest(ApiResponse.ErrorResponse(
                        "Gebruiker heeft deze rol al"));
                }

                var result = await _userManager.AddToRoleAsync(user, roleName);

                if (!result.Succeeded)
                {
                    return BadRequest(ApiResponse.ErrorResponse("Rol toewijzen mislukt"));
                }

                _logger.LogInformation("Rol {RoleName} toegewezen aan gebruiker {UserId} ({Email})",
                    roleName, userId, user.Email);

                return Ok(ApiResponse.SuccessResponse(
                    $"Rol '{roleName}' succesvol toegewezen"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij toewijzen van rol");
                return StatusCode(500, ApiResponse.ErrorResponse(
                    "Er is een fout opgetreden", 500));
            }
        }

        /// <summary>
        /// Verwijder rol van gebruiker
        /// </summary>
        [HttpDelete("users/{userId}/roles/{roleName}")]
        public async Task<ActionResult<ApiResponse>> RemoveRole(string userId, string roleName)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound(ApiResponse.NotFoundResponse("Gebruiker niet gevonden"));
                }

                if (!await _userManager.IsInRoleAsync(user, roleName))
                {
                    return BadRequest(ApiResponse.ErrorResponse(
                        "Gebruiker heeft deze rol niet"));
                }

                // Voorkom dat laatste Admin zijn Admin rol verliest
                if (roleName == "Admin")
                {
                    var admins = await _userManager.GetUsersInRoleAsync("Admin");
                    if (admins.Count <= 1)
                    {
                        return BadRequest(ApiResponse.ErrorResponse(
                            "Kan Admin rol niet verwijderen van de laatste administrator"));
                    }
                }

                var result = await _userManager.RemoveFromRoleAsync(user, roleName);

                if (!result.Succeeded)
                {
                    return BadRequest(ApiResponse.ErrorResponse("Rol verwijderen mislukt"));
                }

                _logger.LogInformation("Rol {RoleName} verwijderd van gebruiker {UserId} ({Email})",
                    roleName, userId, user.Email);

                return Ok(ApiResponse.SuccessResponse(
                    $"Rol '{roleName}' succesvol verwijderd"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij verwijderen van rol");
                return StatusCode(500, ApiResponse.ErrorResponse(
                    "Er is een fout opgetreden", 500));
            }
        }

        // ============ STATISTICS ============

        /// <summary>
        /// Haal gebruiker statistieken op
        /// </summary>
        [HttpGet("stats/users")]
        public async Task<ActionResult<ApiResponse<object>>> GetUserStatistics()
        {
            try
            {
                var totalUsers = await _context.Users.CountAsync();
                var activeUsers = await _context.Users.CountAsync(u => u.IsActive);
                var verifiedUsers = await _context.Users.CountAsync(u => u.IsVerified);
                var lockedUsers = await _context.Users.CountAsync(u => u.LockoutEnd != null && u.LockoutEnd > DateTimeOffset.Now);

                // Nieuwe gebruikers laatste 7 dagen
                var sevenDaysAgo = DateTime.Now.AddDays(-7);
                var newUsersLastWeek = await _context.Users
                    .CountAsync(u => u.RegistrationDate >= sevenDaysAgo);

                // Nieuwe gebruikers laatste 30 dagen
                var thirtyDaysAgo = DateTime.Now.AddDays(-30);
                var newUsersLastMonth = await _context.Users
                    .CountAsync(u => u.RegistrationDate >= thirtyDaysAgo);

                var stats = new
                {
                    TotalUsers = totalUsers,
                    ActiveUsers = activeUsers,
                    VerifiedUsers = verifiedUsers,
                    LockedUsers = lockedUsers,
                    NewUsersLastWeek = newUsersLastWeek,
                    NewUsersLastMonth = newUsersLastMonth
                };

                return Ok(ApiResponse<object>.SuccessResponse(stats));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij ophalen van gebruiker statistieken");
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "Er is een fout opgetreden", 500));
            }
        }

        /// <summary>
        /// Haal rol statistieken op
        /// </summary>
        [HttpGet("stats/roles")]
        public async Task<ActionResult<ApiResponse<object>>> GetRoleStatistics()
        {
            try
            {
                var roles = await _roleManager.Roles.ToListAsync();
                var roleStats = new List<object>();

                foreach (var role in roles)
                {
                    var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name!);

                    roleStats.Add(new
                    {
                        RoleName = role.Name,
                        Description = role.Description,
                        UserCount = usersInRole.Count,
                        IsActive = role.IsActive
                    });
                }

                var stats = new
                {
                    TotalRoles = roles.Count,
                    ActiveRoles = roles.Count(r => r.IsActive),
                    RoleBreakdown = roleStats
                };

                return Ok(ApiResponse<object>.SuccessResponse(stats));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij ophalen van rol statistieken");
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "Er is een fout opgetreden", 500));
            }
        }
    }
}