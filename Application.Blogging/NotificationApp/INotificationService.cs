using Domain.Blogging.view.NotficationView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Blogging.NotificationApp
{
    public interface INotificationService
    {
        Task SaveNotification(NotificationViewModel model);
    }
}
