
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Windows;
using Client.ChatService;

namespace Client
{
    [CallbackBehavior(UseSynchronizationContext = false)]
    public class MyCallback : ChatService.IChatServiceCallback
    {
        public event EventHandler OnLogin;
        public event EventHandler OnLeave;

        public event EventHandler OnChangeOnline;
        public event EventHandler OnNewUser;
        
        public event EventHandler OnGroups;
        public event EventHandler OnLeaveGroup;
        public event EventHandler OnNewGroup;
        public event EventHandler OnNewUsersInGroups;

        public event EventHandler OnNewMessage;
        public event EventHandler OnLeaveUserInGroup;

        public void RLogin(RUser usr) => OnLogin?.Invoke(usr, null);
        public void RLeave() => OnLeave?.Invoke(null, null);

        public void RChangeOnline(RUser usr) => OnChangeOnline?.Invoke(usr, null);
        public void RNewUser(RUser usr) => OnNewUser?.Invoke(usr, null);

        public void RGroups(RMUserInGroup[] group) => OnGroups?.Invoke(group, null);
        public void RLeaveGroup(RGroup group) => OnLeaveGroup?.Invoke(group, null);
        public void RNewGroup(RMUserInGroup usrInGrp) => OnNewGroup?.Invoke(usrInGrp, null);
        public void RNewUsersInGroup(RMUserInGroup[] users) => OnNewUsersInGroups?.Invoke(users, null);
        public void RLeaveUserInGroup(RGroup group, RUser usr) => OnLeaveUserInGroup?.Invoke(new KeyValuePair<RGroup, RUser>(group, usr), null);

        public void RNewMessage(RUser usr, RGroupMessage msg) => OnNewMessage?.Invoke(new KeyValuePair<RUser, RGroupMessage>(usr, msg), null);

        public void Error(string message)
        {
            MessageBox.Show(message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void Message([MessageParameter(Name = "message")] string message1)
        {
            throw new System.NotImplementedException();
        }
    }
}
