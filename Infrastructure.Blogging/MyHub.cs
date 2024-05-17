using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Blogging.view.NotficationView;
using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.Blogging
{
    public class MyHub: Hub 
    {
        
        Dictionary<string, string> userSession = new Dictionary<string, string>();
        public async Task askServer(string someTextFromClient)
        {
            string tempString;

            if (someTextFromClient == "hey")
            {
                tempString = "message was 'hey'";
            }
            else
            {
                tempString = "message was something else";
            }

            await Clients.Client(this.Context.ConnectionId).SendAsync("askServerResponse", tempString);
        }

        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "Come2Chat");
            await Clients.Caller.SendAsync("UserConected");
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Come2Chat");
            await base.OnDisconnectedAsync(exception);
        }

        public void AddUser(string userId, string conId)
        {
            userSession[userId] = conId;

        }
        
        public async Task AddUserConnectionId(string userId)
        {
            AddUser(userId, Context.ConnectionId);

        }
        
        public async Task SendNotification(NotificationSocketViewModel model)
        {
            await Clients.Group("Come2Chat").SendAsync("NewMessage", model);

        }



        //public async Task askServer(string someTextFromClient)
        //{
        //    string tempString;

        //    if (someTextFromClient == "hey")
        //    {
        //        tempString = "message was 'hey'";
        //    }
        //    else
        //    {
        //        tempString = "message was something else";
        //    }

        //    await Clients.Client(this.Context.ConnectionId).SendAsync("askServerResponse", tempString);
        //}
    }
}
