namespace PersonalMailer.Sender
{
    public class SendGridConfiguration
    {
        public string ApiKey { get; set; }
        public string TemplateId { get; set; }

        public string RecipientEmail { get; set; }
        public string RecipientName { get; set; }
    }
}