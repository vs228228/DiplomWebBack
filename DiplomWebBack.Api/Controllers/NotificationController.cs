using DiplomWebBack.Api.Extensions;
using DiplomWebBack.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DiplomWebBack.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        /// <summary>
        /// Test Notification
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> GetTestNotification([FromQuery] string message, CancellationToken ct)
        {
            var userId = HttpContext.GetCurrentUserId();

            await _notificationService.SendToUser(userId, new Domain.Entities.Responses.Notification("Test message", message, "Code_000"));

            return Ok();
        }
    }
}
