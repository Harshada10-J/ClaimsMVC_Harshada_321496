using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;


namespace ClaimsMVC.Services
{


    public class EmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
      
        public async Task SendEmailAsync(string toEmail, string subject, string htmlContent)
        {
            Console.WriteLine("--- EMAIL SERVICE: SendEmailAsync method was successfully called. ---");

            try
            {
                var apiKey = _configuration["SendGridApiKey"];
                if (string.IsNullOrEmpty(apiKey))
                {
                    _logger.LogError("SendGrid API Key is not configured.");
                    return;
                }
                else // --- ADDED ELSE BLOCK ---
                {
                    _logger.LogInformation("SendGrid API Key found successfully.");
                }

                var client = new SendGridClient(apiKey);
                var from = new EmailAddress("jhvhliy@gmail.com", "Claims Processing System");
                var to = new EmailAddress(toEmail);
                var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlContent);

                var response = await client.SendEmailAsync(msg);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Failed to send email. Status code: {response.StatusCode}");
                }
                else // --- ADDED ELSE BLOCK ---
                {
                    _logger.LogInformation("Email sent successfully via SendGrid!");
                }
            }
            catch (Exception ex)
            {
                // This will catch the error and print it to your console
                _logger.LogError(ex, "An exception occurred while sending an email.");
            }
        }
    }
}