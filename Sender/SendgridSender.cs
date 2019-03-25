using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace PersonalMailer.Sender
{
    public class SendGridSender : ISendGridSender
    {
        private readonly ISendGridClient _client;
        private readonly ILogger<SendGridSender> _logger;
        private readonly SendGridConfiguration _configuration;

        public SendGridSender(ISendGridClient client, ILogger<SendGridSender> logger, IOptions<SendGridConfiguration> options)
        {
            _client = client;
            _logger = logger;
            _configuration = options.Value;
        }

        public async Task<bool> PrepareAndSendAsync(string senderName, string senderEmail, string mailSubject, string mailMessage)
        {
            SendGridMessage message = PrepareMessage(senderName, senderEmail, mailSubject, mailMessage);

            var response = await _client.SendEmailAsync(message);
            var isSuccess = response.StatusCode == HttpStatusCode.Accepted;

            if (!isSuccess)
            {
                _logger.LogWarning($"SendGrid API response: \t {JsonConvert.SerializeObject(response)}");
            }

            return isSuccess;
        }

        private SendGridMessage PrepareMessage(string senderName, string senderEmail, string mailSubject, string mailMessage)
        {
            EmailAddress sender = new EmailAddress(senderEmail, senderName);
            object templateData = PrepareTemplateData(senderName, senderEmail, mailSubject, mailMessage);
            Personalization personalization = BuildPersonalization(templateData);

            var message = new SendGridMessage();
            message.From = sender;

            // set personalizations
            message.Personalizations = new List<Personalization>();
            message.Personalizations.Add(personalization);

            message.TemplateId = _configuration.TemplateId;

            // handle sandbox mode
            ConfigureOptionalSandbox(message);

            return message;
        }

        private void ConfigureOptionalSandbox(SendGridMessage message)
        {
            if (_configuration.Sandbox)
            {
                message.MailSettings = new MailSettings()
                {
                    SandboxMode = new SandboxMode()
                    {
                        Enable = true
                    }
                };
            }
        }

        private Personalization BuildPersonalization(object templateData)
        {
            EmailAddress recipient = new EmailAddress(_configuration.RecipientEmail, _configuration.RecipientName);

            var personalization = new Personalization();

            // set recipient
            personalization.Tos = new List<EmailAddress>();
            personalization.Tos.Add(recipient);

            // set template id
            personalization.TemplateData = templateData;

            return personalization;
        }

        private object PrepareTemplateData(string senderName, string senderEmail, string mailSubject, string mailMessage)
        => new
        {
            SenderName = senderName,
            SenderEmail = senderEmail,
            MailSubject = mailSubject,
            MailMessage = mailMessage
        };
    }
}