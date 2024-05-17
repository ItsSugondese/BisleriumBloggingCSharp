using Application.Blogging.NotificationApp;
using Application.Blogging.RepoInterface.UserRepoInterface;
using Domain.Blogging;
using Domain.Blogging.Entities;
using Domain.Blogging.view.NotficationView;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Blogging.ServicesImpl.NotificationsImpl
{
    public class NotificationServiceImpl : INotificationService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IUserRepo _userRepo;

        public NotificationServiceImpl(ApplicationDbContext dbContext, 
            IUserRepo userRepo)
        {
            _dbContext = dbContext;
            _userRepo = userRepo;
        }

        public async Task SaveNotification(NotificationViewModel model)
        {
            Notifications notifications = new Notifications()
            {
                CreatedAt = DateTime.UtcNow,
                User = await _userRepo.FindById(model.UserId),
                Message = model.Message
            };

            _dbContext.Add(notifications);
            await _dbContext.SaveChangesAsync();
        }
    }
}
