using System;
using System.Collections.Generic;
using System.Linq;
using AzureChat.Models;
using Enums;
using Microsoft.AspNet.SignalR;
using Validators;
using ViewModels.Home;

namespace AzureChat.Hubs
{
    public class ChatHub : Hub
    {
        static List<UserModel> connectedUsers = new List<UserModel>();
        static List<MessageModel> latestMessages = new List<MessageModel>();
        
        public ErrorCodes Connect(string userName)
        {
            UserViewModel userViewModel = new UserViewModel
            {
                UserName = userName
            };

            if (!new UserValidator().Validate(userViewModel).IsValid)
                return ErrorCodes.ServerValidationFailed; //in case if client validation has been broken

            if (this.IsDuplicate(userName))
                return ErrorCodes.DuplicateName;

            var id = Context.ConnectionId;
            if (!connectedUsers.Any(x => x.ConnectionId == id))
            {
                connectedUsers.Add(new UserModel { ConnectionId = id, UserName = userName });
            }

            UserModel currentUser = connectedUsers.Where(u => u.ConnectionId == id).FirstOrDefault();
            Clients.Caller.onConnected(currentUser.UserName, connectedUsers, latestMessages);
            Clients.AllExcept(currentUser.ConnectionId).onNewUserConnected(currentUser.ConnectionId, currentUser.UserName, connectedUsers.Count);

            return ErrorCodes.NoErrors;
        }
        
        public void Send(string message)
        {
            var user = connectedUsers.Where(x => x.ConnectionId == Context.ConnectionId).FirstOrDefault();
            if (user == null)
                throw new Exception("Some error happened");

            MessageModel messageModel = new MessageModel
            {
                Message = message,
                Time = DateTime.UtcNow,
                UserName = user.UserName
            };
            Clients.All.broadcastMessage(messageModel.UserName, messageModel.Message, messageModel.Time);
            AddMessageInCache(messageModel);
        }

        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            var connectionId = Context.ConnectionId;
            var item = connectedUsers.FirstOrDefault(x => x.ConnectionId == connectionId);
            if (item != null)
            {
                connectedUsers.Remove(item);
                if (!connectedUsers.Any(u => u.ConnectionId == item.ConnectionId))
                {
                    Clients.All.onUserDisconnected(item.ConnectionId, connectedUsers.Count);
                }
            }
            return base.OnDisconnected(stopCalled);
        }

        private bool IsDuplicate(string userName)
        {
            var id = Context.ConnectionId;
            if (connectedUsers.Any(x => x.ConnectionId != id && x.UserName == userName))
                return true;

            return false;
        }

        private void AddMessageInCache(MessageModel message)
        {
            latestMessages.Add(message);
            if (latestMessages.Count > Enums.AppSettings.CacheMessagesCount)
                latestMessages.RemoveAt(0);
        }
    }
}