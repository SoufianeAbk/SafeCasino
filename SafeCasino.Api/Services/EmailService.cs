using System.Web;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using SafeCasino.Data.Entities;

namespace SafeCasino.Api.Services;

public class EmailService : IEmailService
{
    private readonly SmtpSettings _smtpSettings;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IOptions<SmtpSettings> smtpSettings, ILogger<EmailService> logger)
    {
        _smtpSettings = smtpSettings.Value;
        _logger = logger;
    }

    public async Task SendEmailVerificationAsync(ApplicationUser user, string token)
    {
        try
        {
            var encodedToken = HttpUtility.UrlEncode(token);
            var verificationLink = $"https://localhost:7243/verify-email?userId={user.Id}&token={encodedToken}";

            var subject = "Bevestig je e-mailadres bij SafeCasino";
            var body = $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background-color: #0a0e27;
            color: #e0e0e0;
            margin: 0;
            padding: 0;
        }}
        .container {{
            max-width: 600px;
            margin: 40px auto;
            background: linear-gradient(135deg, #1a1f3a 0%, #0f1228 100%);
            border-radius: 15px;
            box-shadow: 0 8px 32px rgba(255, 215, 0, 0.1);
            overflow: hidden;
        }}
        .header {{
            background: linear-gradient(135deg, #d4af37 0%, #aa8c2d 100%);
            padding: 30px;
            text-align: center;
        }}
        .header h1 {{
            color: #0a0e27;
            margin: 0;
            font-size: 32px;
            font-weight: bold;
        }}
        .content {{
            padding: 40px 30px;
        }}
        .content h2 {{
            color: #d4af37;
            margin-top: 0;
        }}
        .content p {{
            line-height: 1.6;
            color: #b0b0b0;
        }}
        .button {{
            display: inline-block;
            padding: 15px 40px;
            background: linear-gradient(135deg, #d4af37 0%, #aa8c2d 100%);
            color: #0a0e27;
            text-decoration: none;
            border-radius: 25px;
            font-weight: bold;
            margin: 20px 0;
            transition: transform 0.2s;
        }}
        .button:hover {{
            transform: translateY(-2px);
        }}
        .footer {{
            background-color: #0a0e27;
            padding: 20px;
            text-align: center;
            color: #666;
            font-size: 12px;
        }}
        .warning {{
            background-color: rgba(212, 175, 55, 0.1);
            border-left: 4px solid #d4af37;
            padding: 15px;
            margin: 20px 0;
            border-radius: 5px;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>🎰 SafeCasino</h1>
        </div>
        <div class='content'>
            <h2>Welkom bij SafeCasino, {user.FirstName}!</h2>
            <p>Bedankt voor je registratie. Klik op de onderstaande knop om je e-mailadres te bevestigen en je account te activeren.</p>
            
            <div style='text-align: center;'>
                <a href='{verificationLink}' class='button'>Bevestig E-mailadres</a>
            </div>
            
            <div class='warning'>
                <p><strong>⚠️ Belangrijke informatie:</strong></p>
                <p>Deze link is 24 uur geldig. Als je deze e-mail niet hebt aangevraagd, kun je deze negeren.</p>
            </div>
            
            <p>Als de knop niet werkt, kopieer dan deze link naar je browser:</p>
            <p style='word-break: break-all; color: #d4af37;'>{verificationLink}</p>
        </div>
        <div class='footer'>
            <p>&copy; 2026 SafeCasino. Alle rechten voorbehouden.</p>
            <p>Dit is een automatisch gegenereerde e-mail. Reageer hier niet op.</p>
        </div>
    </div>
</body>
</html>";

            await SendEmailAsync(user.Email!, subject, body);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fout bij verzenden verificatie-email naar {Email}", user.Email);
            throw;
        }
    }

    public async Task SendPasswordResetEmailAsync(ApplicationUser user, string token)
    {
        try
        {
            var encodedToken = HttpUtility.UrlEncode(token);
            var resetLink = $"https://localhost:7243/reset-password?userId={user.Id}&token={encodedToken}";

            var subject = "Wachtwoord Herstellen - SafeCasino";
            var body = $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background-color: #0a0e27;
            color: #e0e0e0;
            margin: 0;
            padding: 0;
        }}
        .container {{
            max-width: 600px;
            margin: 40px auto;
            background: linear-gradient(135deg, #1a1f3a 0%, #0f1228 100%);
            border-radius: 15px;
            box-shadow: 0 8px 32px rgba(255, 215, 0, 0.1);
            overflow: hidden;
        }}
        .header {{
            background: linear-gradient(135deg, #d4af37 0%, #aa8c2d 100%);
            padding: 30px;
            text-align: center;
        }}
        .header h1 {{
            color: #0a0e27;
            margin: 0;
            font-size: 32px;
            font-weight: bold;
        }}
        .content {{
            padding: 40px 30px;
        }}
        .content h2 {{
            color: #d4af37;
            margin-top: 0;
        }}
        .content p {{
            line-height: 1.6;
            color: #b0b0b0;
        }}
        .button {{
            display: inline-block;
            padding: 15px 40px;
            background: linear-gradient(135deg, #d4af37 0%, #aa8c2d 100%);
            color: #0a0e27;
            text-decoration: none;
            border-radius: 25px;
            font-weight: bold;
            margin: 20px 0;
            transition: transform 0.2s;
        }}
        .button:hover {{
            transform: translateY(-2px);
        }}
        .footer {{
            background-color: #0a0e27;
            padding: 20px;
            text-align: center;
            color: #666;
            font-size: 12px;
        }}
        .warning {{
            background-color: rgba(220, 53, 69, 0.1);
            border-left: 4px solid #dc3545;
            padding: 15px;
            margin: 20px 0;
            border-radius: 5px;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>🎰 SafeCasino</h1>
        </div>
        <div class='content'>
            <h2>Wachtwoord Herstellen</h2>
            <p>Hallo {user.FirstName},</p>
            <p>We hebben een verzoek ontvangen om je wachtwoord te resetten. Klik op de onderstaande knop om een nieuw wachtwoord in te stellen.</p>
            
            <div style='text-align: center;'>
                <a href='{resetLink}' class='button'>Reset Wachtwoord</a>
            </div>
            
            <div class='warning'>
                <p><strong>⚠️ Beveiligingswaarschuwing:</strong></p>
                <p>Deze link is slechts 1 uur geldig om veiligheidsredenen.</p>
                <p>Als je dit niet hebt aangevraagd, negeer deze e-mail dan en je wachtwoord blijft ongewijzigd.</p>
            </div>
            
            <p>Als de knop niet werkt, kopieer dan deze link naar je browser:</p>
            <p style='word-break: break-all; color: #d4af37;'>{resetLink}</p>
        </div>
        <div class='footer'>
            <p>&copy; 2026 SafeCasino. Alle rechten voorbehouden.</p>
            <p>Dit is een automatisch gegenereerde e-mail. Reageer hier niet op.</p>
        </div>
    </div>
</body>
</html>";

            await SendEmailAsync(user.Email!, subject, body);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fout bij verzenden wachtwoord reset email naar {Email}", user.Email);
            throw;
        }
    }

    public async Task SendWelcomeEmailAsync(ApplicationUser user)
    {
        try
        {
            var subject = "Welkom bij SafeCasino! 🎉";
            var body = $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background-color: #0a0e27;
            color: #e0e0e0;
            margin: 0;
            padding: 0;
        }}
        .container {{
            max-width: 600px;
            margin: 40px auto;
            background: linear-gradient(135deg, #1a1f3a 0%, #0f1228 100%);
            border-radius: 15px;
            box-shadow: 0 8px 32px rgba(255, 215, 0, 0.1);
            overflow: hidden;
        }}
        .header {{
            background: linear-gradient(135deg, #d4af37 0%, #aa8c2d 100%);
            padding: 30px;
            text-align: center;
        }}
        .header h1 {{
            color: #0a0e27;
            margin: 0;
            font-size: 32px;
            font-weight: bold;
        }}
        .content {{
            padding: 40px 30px;
        }}
        .content h2 {{
            color: #d4af37;
            margin-top: 0;
        }}
        .content p {{
            line-height: 1.6;
            color: #b0b0b0;
        }}
        .bonus-box {{
            background: linear-gradient(135deg, #d4af37 0%, #aa8c2d 100%);
            color: #0a0e27;
            padding: 25px;
            border-radius: 10px;
            text-align: center;
            margin: 25px 0;
        }}
        .bonus-box h3 {{
            margin: 0 0 10px 0;
            font-size: 24px;
        }}
        .bonus-amount {{
            font-size: 48px;
            font-weight: bold;
            margin: 10px 0;
        }}
        .button {{
            display: inline-block;
            padding: 15px 40px;
            background: linear-gradient(135deg, #d4af37 0%, #aa8c2d 100%);
            color: #0a0e27;
            text-decoration: none;
            border-radius: 25px;
            font-weight: bold;
            margin: 20px 0;
            transition: transform 0.2s;
        }}
        .button:hover {{
            transform: translateY(-2px);
        }}
        .footer {{
            background-color: #0a0e27;
            padding: 20px;
            text-align: center;
            color: #666;
            font-size: 12px;
        }}
        .features {{
            background-color: rgba(212, 175, 55, 0.1);
            padding: 20px;
            border-radius: 10px;
            margin: 20px 0;
        }}
        .features ul {{
            list-style: none;
            padding: 0;
        }}
        .features li {{
            padding: 10px 0;
            border-bottom: 1px solid rgba(212, 175, 55, 0.2);
        }}
        .features li:last-child {{
            border-bottom: none;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>🎰 SafeCasino</h1>
        </div>
        <div class='content'>
            <h2>Welkom {user.FirstName}! 🎊</h2>
            <p>Je account is succesvol geactiveerd en je bent nu klaar om te beginnen met spelen op SafeCasino!</p>
            
            <div class='bonus-box'>
                <h3>🎁 Welkomstbonus</h3>
                <div class='bonus-amount'>€100</div>
                <p style='margin: 0;'>Is toegevoegd aan je account!</p>
            </div>
            
            <div class='features'>
                <h3 style='color: #d4af37; margin-top: 0;'>Wat kun je verwachten?</h3>
                <ul>
                    <li>🎰 Honderden spannende casinospellen</li>
                    <li>🎲 Live dealer games voor de echte casino ervaring</li>
                    <li>💰 Snelle en veilige betalingen</li>
                    <li>🎁 Regelmatige bonussen en promoties</li>
                    <li>🏆 VIP-programma met exclusieve voordelen</li>
                    <li>🔒 100% veilig en betrouwbaar</li>
                </ul>
            </div>
            
            <div style='text-align: center;'>
                <a href='https://localhost:7243/games' class='button'>Begin met Spelen!</a>
            </div>
            
            <p style='margin-top: 30px; padding-top: 20px; border-top: 1px solid rgba(212, 175, 55, 0.2);'>
                <strong>Heb je vragen?</strong><br>
                Ons supportteam staat 24/7 voor je klaar via de chat op onze website.
            </p>
        </div>
        <div class='footer'>
            <p><strong>Speel Verantwoord</strong></p>
            <p>Gokken kan verslavend zijn. SafeCasino promoot verantwoord gokken.</p>
            <p>&copy; 2026 SafeCasino. Alle rechten voorbehouden.</p>
        </div>
    </div>
</body>
</html>";

            await SendEmailAsync(user.Email!, subject, body);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fout bij verzenden welkomst-email naar {Email}", user.Email);
            // Don't throw here - welcome email is not critical
        }
    }

    private async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_smtpSettings.FromName, _smtpSettings.FromEmail));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = body };
            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            _logger.LogInformation("Email verzonden naar {Email}", toEmail);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fout bij verzenden email naar {Email}", toEmail);
            throw;
        }
    }
}

public class SmtpSettings
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    public bool EnableSsl { get; set; }
    public string FromEmail { get; set; } = string.Empty;
    public string FromName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}