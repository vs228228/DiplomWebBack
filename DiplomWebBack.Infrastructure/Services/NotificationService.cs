using DiplomWebBack.Domain.Entities.Responses;
using DiplomWebBack.Domain.Interfaces;
using DiplomWebBack.Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace DiplomWebBack.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendToUser(Guid userId, Notification notification)
        {
            await _hubContext.Clients
                .User(userId.ToString())
                .SendAsync("ReceiveNotification", notification);
        }
    }
}