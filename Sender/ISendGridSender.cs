using System.Threading.Tasks;

namespace PersonalMailer.Sender
{
    public interface ISendGridSender
    {
        Task<bool> PrepareAndSendAsync(string senderName, string senderEmail, string mailSubject, string mailMessage);
    }
}