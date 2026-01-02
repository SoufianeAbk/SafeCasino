using System.Net;
using System.Net.Mail;

namespace SafeCasino.Api.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendEmailVerificationAsync(string email, string userName, string verificationLink)
        {
            var subject = "Verifieer je SafeCasino account";
            var body = GetEmailVerificationTemplate(userName, verificationLink);

            await SendEmailAsync(email, subject, body);
        }

        public async Task SendPasswordResetEmailAsync(string email, string userName, string resetLink)
        {
            var subject = "Reset je SafeCasino wachtwoord";
            var body = GetPasswordResetTemplate(userName, resetLink);

            await SendEmailAsync(email, subject, body);
        }

        public async Task SendWelcomeEmailAsync(string email, string userName)
        {
            var subject = "Welkom bij SafeCasino!";
            var body = GetWelcomeTemplate(userName);

            await SendEmailAsync(email, subject, body);
        }

        private async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var smtpSettings = _configuration.GetSection("SmtpSettings");
                var fromEmail = smtpSettings["FromEmail"];
                var fromName = smtpSettings["FromName"];
                var host = smtpSettings["Host"];
                var port = int.Parse(smtpSettings["Port"] ?? "587");
                var username = smtpSettings["Username"];
                var password = smtpSettings["Password"];
                var enableSsl = bool.Parse(smtpSettings["EnableSsl"] ?? "true");

                using var client = new SmtpClient(host, port)
                {
                    Credentials = new NetworkCredential(username, password),
                    EnableSsl = enableSsl
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail ?? "noreply@safecasino.be", fromName ?? "SafeCasino"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(toEmail);

                await client.SendMailAsync(mailMessage);

                _logger.LogInformation($"Email verzonden naar {toEmail} met onderwerp: {subject}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Fout bij verzenden email naar {toEmail}");
                throw;
            }
        }

        private string GetEmailVerificationTemplate(string userName, string verificationLink)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{
            font-family: Arial, sans-serif;
            background-color: #1a1a2e;
            color: #ffffff;
            margin: 0;
            padding: 0;
        }}
        .container {{
            max-width: 600px;
            margin: 0 auto;
            background-color: #16213e;
            padding: 40px;
            border-radius: 10px;
        }}
        .header {{
            text-align: center;
            margin-bottom: 30px;
        }}
        .logo {{
            font-size: 32px;
            font-weight: bold;
            color: #ffd700;
        }}
        .content {{
            line-height: 1.6;
        }}
        .button {{
            display: inline-block;
            padding: 15px 40px;
            background: linear-gradient(135deg, #e91e63 0%, #9c27b0 100%);
            color: white;
            text-decoration: none;
            border-radius: 5px;
            margin: 20px 0;
            font-weight: bold;
        }}
        .footer {{
            margin-top: 30px;
            padding-top: 20px;
            border-top: 1px solid rgba(255, 255, 255, 0.1);
            text-align: center;
            color: #888;
            font-size: 12px;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <div class='logo'>🎰 SAFECASINO</div>
        </div>
        <div class='content'>
            <h2>Hallo {userName},</h2>
            <p>Welkom bij SafeCasino! We zijn blij dat je je hebt aangemeld.</p>
            <p>Om je account te activeren, moet je je e-mailadres verifiëren. Klik op onderstaande knop om je account te verifiëren:</p>
            <div style='text-align: center;'>
                <a href='{verificationLink}' class='button'>Verifieer E-mailadres</a>
            </div>
            <p>Als de knop niet werkt, kopieer en plak deze link in je browser:</p>
            <p style='word-break: break-all; color: #ffd700;'>{verificationLink}</p>
            <p><strong>Let op:</strong> Deze link is 24 uur geldig.</p>
            <p>Als je dit account niet hebt aangemaakt, kun je deze email negeren.</p>
        </div>
        <div class='footer'>
            <p>© 2025 SafeCasino. Alle rechten voorbehouden.</p>
            <p>Gokken kan verslavend zijn. Speel verantwoord. 18+</p>
        </div>
    </div>
</body>
</html>";
        }

        private string GetPasswordResetTemplate(string userName, string resetLink)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{
            font-family: Arial, sans-serif;
            background-color: #1a1a2e;
            color: #ffffff;
            margin: 0;
            padding: 0;
        }}
        .container {{
            max-width: 600px;
            margin: 0 auto;
            background-color: #16213e;
            padding: 40px;
            border-radius: 10px;
        }}
        .header {{
            text-align: center;
            margin-bottom: 30px;
        }}
        .logo {{
            font-size: 32px;
            font-weight: bold;
            color: #ffd700;
        }}
        .content {{
            line-height: 1.6;
        }}
        .button {{
            display: inline-block;
            padding: 15px 40px;
            background: linear-gradient(135deg, #e91e63 0%, #9c27b0 100%);
            color: white;
            text-decoration: none;
            border-radius: 5px;
            margin: 20px 0;
            font-weight: bold;
        }}
        .footer {{
            margin-top: 30px;
            padding-top: 20px;
            border-top: 1px solid rgba(255, 255, 255, 0.1);
            text-align: center;
            color: #888;
            font-size: 12px;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <div class='logo'>🎰 SAFECASINO</div>
        </div>
        <div class='content'>
            <h2>Hallo {userName},</h2>
            <p>We hebben een verzoek ontvangen om je wachtwoord te resetten.</p>
            <p>Klik op onderstaande knop om een nieuw wachtwoord in te stellen:</p>
            <div style='text-align: center;'>
                <a href='{resetLink}' class='button'>Reset Wachtwoord</a>
            </div>
            <p>Als de knop niet werkt, kopieer en plak deze link in je browser:</p>
            <p style='word-break: break-all; color: #ffd700;'>{resetLink}</p>
            <p><strong>Let op:</strong> Deze link is 1 uur geldig.</p>
            <p>Als je geen wachtwoord reset hebt aangevraagd, kun je deze email negeren. Je wachtwoord blijft dan ongewijzigd.</p>
        </div>
        <div class='footer'>
            <p>© 2025 SafeCasino. Alle rechten voorbehouden.</p>
            <p>Gokken kan verslavend zijn. Speel verantwoord. 18+</p>
        </div>
    </div>
</body>
</html>";
        }

        private string GetWelcomeTemplate(string userName)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{
            font-family: Arial, sans-serif;
            background-color: #1a1a2e;
            color: #ffffff;
            margin: 0;
            padding: 0;
        }}
        .container {{
            max-width: 600px;
            margin: 0 auto;
            background-color: #16213e;
            padding: 40px;
            border-radius: 10px;
        }}
        .header {{
            text-align: center;
            margin-bottom: 30px;
        }}
        .logo {{
            font-size: 32px;
            font-weight: bold;
            color: #ffd700;
        }}
        .content {{
            line-height: 1.6;
        }}
        .footer {{
            margin-top: 30px;
            padding-top: 20px;
            border-top: 1px solid rgba(255, 255, 255, 0.1);
            text-align: center;
            color: #888;
            font-size: 12px;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <div class='logo'>🎰 SAFECASINO</div>
        </div>
        <div class='content'>
            <h2>Welkom {userName}! 🎉</h2>
            <p>Je account is succesvol geverifieerd en je bent nu klaar om te spelen!</p>
            <p>Bij SafeCasino vind je:</p>
            <ul>
                <li>✅ 9000+ Casino Games</li>
                <li>✅ Hoogste RTP Percentages</li>
                <li>✅ 24/7 Klantenservice</li>
                <li>✅ Snelle Stortingen & Opnames</li>
                <li>✅ Dagelijkse Bonussen & Promoties</li>
            </ul>
            <p>Begin vandaag nog met spelen en ontvang je welkomstbonus!</p>
            <p>Veel speelplezier,<br>Het SafeCasino Team</p>
        </div>
        <div class='footer'>
            <p>© 2025 SafeCasino. Alle rechten voorbehouden.</p>
            <p>Gokken kan verslavend zijn. Speel verantwoord. 18+</p>
        </div>
    </div>
</body>
</html>";
        }
    }
}