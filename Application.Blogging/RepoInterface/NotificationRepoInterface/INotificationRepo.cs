using Domain.Blogging.view.NotficationView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Blogging.RepoInterface.NotificationRepoInterface
{
    public interface INotificationRepo
    {
        int? UserUnreadNotificationCount(string userId);

        Task<Dictionary<string, object>> GetNotificationPaginated(NotificationPaginationViewModel model);
    }
}
