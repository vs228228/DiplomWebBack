using DiplomWebBack.Domain.Entities.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomWebBack.Domain.Interfaces
{
    public interface INotificationService
    {
        Task SendToUser(Guid userId, Notification notification);
    }
}
