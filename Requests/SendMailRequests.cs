using System.ComponentModel.DataAnnotations;
using PersonalMailer.Attributes;

namespace PersonalMailer.Requests
{
    public class SendMailRequest
    {
        [Required]
        public string SenderName { get; set; }

        [Required]
        public string SenderEmail { get; set; }

        [Required]
        public string MailSubject { get; set; }

        [Required]
        public string MailMessage { get; set; }

        [Required]
        [RecaptchaValidation]
        public string Recaptcha { get; set; }
    }
}