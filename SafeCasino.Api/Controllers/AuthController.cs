using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SafeCasino.Api.Services;
using SafeCasino.Data.Entities;
using SafeCasino.Shared.DTOs;
using SafeCasino.Shared.Requests;
using SafeCasino.Shared.Responses;

namespace SafeCasino.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailService emailService,
            ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _logger = logger;
        }

        /// <summary>
        /// Registreer een nieuwe gebruiker
        /// </summary>
        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<UserDto>>> Register([FromBody] RegisterRequest request)
        {
            try
            {
                // Valideer leeftijd (minimaal 18 jaar)
                var age = DateTime.Today.Year - request.DateOfBirth.Year;
                if (request.DateOfBirth.Date > DateTime.Today.AddYears(-age)) age--;

                if (age < 18)
                {
                    return BadRequest(ApiResponse<UserDto>.ErrorResponse(
                        "Je moet minimaal 18 jaar oud zijn om te registreren", 400));
                }

                // Check of email al bestaat
                var existingUser = await _userManager.FindByEmailAsync(request.Email);
                if (existingUser != null)
                {
                    return BadRequest(ApiResponse<UserDto>.ErrorResponse(
                        "Dit e-mailadres is al geregistreerd", 400));
                }

                // Maak nieuwe gebruiker aan
                var user = new ApplicationUser
                {
                    UserName = request.Email,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    DateOfBirth = request.DateOfBirth,
                    RegistrationDate = DateTime.Now,
                    Balance = 100m, // Welkomstbonus
                    IsVerified = false,
                    EmailConfirmed = false
                };

                var result = await _userManager.CreateAsync(user, request.Password);

                if (!result.Succeeded)
                {
                    var errors = result.Errors.ToDictionary(
                        e => e.Code,
                        e => new List<string> { e.Description }
                    );
                    return BadRequest(ApiResponse<UserDto>.ValidationErrorResponse(
                        errors, "Registratie mislukt"));
                }

                // Voeg Player rol toe
                await _userManager.AddToRoleAsync(user, "Player");

                // Genereer email verificatie token
                var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                // Verstuur verificatie email (EmailService verwacht ApplicationUser en token)
                await _emailService.SendEmailVerificationAsync(user, emailToken);

                _logger.LogInformation($"Nieuwe gebruiker geregistreerd: {user.Email}");

                var userDto = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    EmailConfirmed = user.EmailConfirmed
                };

                return Ok(ApiResponse<UserDto>.SuccessResponse(
                    userDto,
                    "Registratie succesvol! Check je email om je account te verifiëren.",
                    201));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij registratie");
                return StatusCode(500, ApiResponse<UserDto>.ErrorResponse(
                    "Er is een fout opgetreden bij de registratie", 500));
            }
        }

        /// <summary>
        /// Verifieer email adres
        /// </summary>
        [HttpGet("verify-email")]
        public async Task<IActionResult> VerifyEmail(string userId, string token)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return BadRequest(ApiResponse.ErrorResponse("Gebruiker niet gevonden", 404));
                }

                if (user.EmailConfirmed)
                {
                    return Ok(ApiResponse.SuccessResponse("Je email is al geverifieerd"));
                }

                var result = await _userManager.ConfirmEmailAsync(user, token);

                if (!result.Succeeded)
                {
                    return BadRequest(ApiResponse.ErrorResponse(
                        "Email verificatie mislukt. De link is mogelijk verlopen.", 400));
                }

                // Markeer gebruiker als geverifieerd
                user.IsVerified = true;
                await _userManager.UpdateAsync(user);

                // Verstuur welkomst email
                await _emailService.SendWelcomeEmailAsync(user);

                _logger.LogInformation($"Email geverifieerd voor gebruiker: {user.Email}");

                return Ok(ApiResponse.SuccessResponse(
                    "Je email is succesvol geverifieerd! Je kunt nu inloggen."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij email verificatie");
                return StatusCode(500, ApiResponse.ErrorResponse(
                    "Er is een fout opgetreden bij de verificatie", 500));
            }
        }

        /// <summary>
        /// Verstuur email verificatie opnieuw
        /// </summary>
        [HttpPost("resend-verification")]
        public async Task<IActionResult> ResendVerificationEmail([FromBody] ResendVerificationRequest request)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                {
                    // Uit veiligheidsoverwegingen geven we niet aan of het account bestaat
                    return Ok(ApiResponse.SuccessResponse(
                        "Als het account bestaat, is er een verificatie email verstuurd"));
                }

                if (user.EmailConfirmed)
                {
                    return BadRequest(ApiResponse.ErrorResponse(
                        "Je email is al geverifieerd", 400));
                }

                // Genereer nieuwe token
                var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                await _emailService.SendEmailVerificationAsync(user, emailToken);

                return Ok(ApiResponse.SuccessResponse(
                    "Verificatie email is opnieuw verstuurd"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij opnieuw versturen verificatie email");
                return StatusCode(500, ApiResponse.ErrorResponse(
                    "Er is een fout opgetreden", 500));
            }
        }

        /// <summary>
        /// Vraag wachtwoord reset aan
        /// </summary>
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null || !user.EmailConfirmed)
                {
                    // Uit veiligheidsoverwegingen geven we niet aan of het account bestaat
                    return Ok(ApiResponse.SuccessResponse(
                        "Als het account bestaat, is er een reset email verstuurd"));
                }

                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

                await _emailService.SendPasswordResetEmailAsync(user, resetToken);

                _logger.LogInformation($"Password reset aangevraagd voor: {user.Email}");

                return Ok(ApiResponse.SuccessResponse(
                    "Als het account bestaat, is er een reset email verstuurd"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij wachtwoord reset aanvraag");
                return StatusCode(500, ApiResponse.ErrorResponse(
                    "Er is een fout opgetreden", 500));
            }
        }

        /// <summary>
        /// Reset wachtwoord
        /// </summary>
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(request.UserId);
                if (user == null)
                {
                    return BadRequest(ApiResponse.ErrorResponse("Gebruiker niet gevonden", 404));
                }

                var result = await _userManager.ResetPasswordAsync(
                    user, request.Token, request.NewPassword);

                if (!result.Succeeded)
                {
                    var errors = result.Errors.ToDictionary(
                        e => e.Code,
                        e => new List<string> { e.Description }
                    );
                    return BadRequest(ApiResponse.ValidationErrorResponse(
                        errors, "Wachtwoord reset mislukt"));
                }

                _logger.LogInformation($"Wachtwoord gereset voor: {user.Email}");

                return Ok(ApiResponse.SuccessResponse(
                    "Je wachtwoord is succesvol gewijzigd. Je kunt nu inloggen."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij wachtwoord reset");
                return StatusCode(500, ApiResponse.ErrorResponse(
                    "Er is een fout opgetreden", 500));
            }
        }

        /// <summary>
        /// Login
        /// </summary>
        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<UserDto>>> Login([FromBody] LoginRequest request)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                {
                    return Unauthorized(ApiResponse<UserDto>.UnauthorizedResponse(
                        "Ongeldige inloggegevens"));
                }

                if (!user.EmailConfirmed)
                {
                    return BadRequest(ApiResponse<UserDto>.ErrorResponse(
                        "Je email moet eerst geverifieerd worden. Check je inbox.", 403));
                }

                var result = await _signInManager.PasswordSignInAsync(
                    user.UserName!,
                    request.Password,
                    request.RememberMe,
                    lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    var roles = await _userManager.GetRolesAsync(user);

                    var userDto = new UserDto
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Balance = user.Balance,
                        EmailConfirmed = user.EmailConfirmed,
                        IsVerified = user.IsVerified,
                        Roles = roles.ToList()
                    };

                    _logger.LogInformation($"Gebruiker ingelogd: {user.Email}");

                    return Ok(ApiResponse<UserDto>.SuccessResponse(
                        userDto, "Login succesvol"));
                }

                if (result.IsLockedOut)
                {
                    return BadRequest(ApiResponse<UserDto>.ErrorResponse(
                        "Je account is tijdelijk vergrendeld. Probeer het later opnieuw.", 403));
                }

                return Unauthorized(ApiResponse<UserDto>.UnauthorizedResponse(
                    "Ongeldige inloggegevens"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij login");
                return StatusCode(500, ApiResponse<UserDto>.ErrorResponse(
                    "Er is een fout opgetreden bij het inloggen", 500));
            }
        }

        /// <summary>
        /// Logout
        /// </summary>
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(ApiResponse.SuccessResponse("Succesvol uitgelogd"));
        }
    }
}