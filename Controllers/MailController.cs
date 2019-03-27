using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using PersonalMailer.Requests;
using PersonalMailer.Results;
using PersonalMailer.Sender;

namespace PersonalMailer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : Controller
    {
        public ISendGridSender _sender;
        public MailController(ISendGridSender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        public ActionResult<SendMailResult> Process([FromBody]SendMailRequest request)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest();
            }
#pragma warning disable 1998
            BackgroundJob.Enqueue(() => _sender.PrepareAndSendAsync(request.SenderName, request.SenderEmail, request.MailSubject, request.MailMessage));
#pragma warning restore 1998

            return new SendMailResult()
            {
                Success = true
            };
        }
    }
}