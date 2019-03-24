using System.ComponentModel.DataAnnotations;

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
        public string Recaptcha { get; set; }
    }
}