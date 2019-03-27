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
        public async Task<ActionResult<SendMailResult>> Process([FromBody]SendMailRequest request)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest();
            }

            BackgroundJob.Enqueue(() => _sender.PrepareAndSendAsync(request.SenderName, request.SenderEmail, request.MailSubject, request.MailMessage));

            return new SendMailResult()
            {
                Success = true
            };
        }
    }
}