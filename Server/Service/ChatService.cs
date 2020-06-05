using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Server.Base;
using Server.Base.Tables;

namespace Server.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)] 
    public class ChatService : IChatService
    {
        IChatServiceCallBack Callback {
            get => OperationContext.Current.GetCallbackChannel<IChatServiceCallBack>();
        }

        Context mainbBase = new Context();

        List<OnlineUser> onlineUsers = new List<OnlineUser>();

        void IChatService.Register(string Login1, string Email, string Password, string repeatPassword)
        {
            string errMsg;
            if (Login1.Length < 1) { Callback.Error("Input login!"); return; }
            if (mainbBase.Users.FirstOrDefault((x) => x.Login == Login1) != null) { Callback.Error("User with this login is already exists!"); return; }
            if (Email.Length < 1) { Callback.Error("Input email!"); return; }
            if (!Common.IsOkEmail(Email)) { Callback.Error("Incorrect email!"); return; }
            if (mainbBase.Users.FirstOrDefault((x) => x.Email == Email) != null) { Callback.Error("User with this email is already exists!"); return; }
            if (!Common.IsOkPassword(Password, out errMsg)) { Callback.Error(errMsg); return; }
            if (repeatPassword != Password) { Callback.Error("Input correct repeat password!"); return; }

            string hasedPassword = Common.GetMD5(Password);

            mainbBase.Users.Add(new Base.Tables.User { Login = Login1, PasswordHash = Common.GetMD5(Password), Email = Email, DCreate = DateTime.Now, LastActivity = DateTime.Now, Blocked = false });
            mainbBase.SaveChanges();
            User usr = mainbBase.Users.FirstOrDefault((x) => x.Login == Login1 && x.PasswordHash == hasedPassword);
            OnlineUser ussr = onlineUsers.FirstOrDefault((x) => x.BaseUser.ID == usr.ID);
            if (ussr == null)
            {
                ussr = new OnlineUser(usr, mainbBase, onlineUsers);
                ussr.OnUserLeave += LeaveOnlineUser;
                ussr.OnSessionLeave += LeaveUserSession;
                onlineUsers.Add(ussr);
                ussr.AddSession(new USession(ussr, Callback));
            }
            else ussr.AddSession(new USession(ussr, Callback));
        }

        void IChatService.Login(string Login, string Password)
        {
            if (Login.Length < 1) { Callback.Error("Input login!"); return; }
            if (Password.Length < 1) { Callback.Error("Input password!"); return; }
            string hasedPassword = Common.GetMD5(Password);

            User usr = mainbBase.Users.FirstOrDefault((x) => x.Login == Login && x.PasswordHash == hasedPassword);

            if (usr != null)
            {
                OnlineUser ussr = onlineUsers.FirstOrDefault((x) => x.BaseUser.ID == usr.ID);
                if (ussr == null)
                {
                    ussr = new OnlineUser(usr, mainbBase, onlineUsers);
                    ussr.OnUserLeave += LeaveOnlineUser;
                    ussr.OnSessionLeave += LeaveUserSession;
                    onlineUsers.Add(ussr);
                    ussr.AddSession(new USession(ussr, Callback));
                }
                else ussr.AddSession(new USession(ussr, Callback));

            }
            else Callback.Error("Incorrect password or login!");
        }

        void LeaveOnlineUser(OnlineUser usr) { // Event
            onlineUsers.Remove(usr);
            onlineUsers.ForEach((x) => x.Sessions.ForEach((s) => s.Callback.RChangeOnline(new RUser(usr.BaseUser, false))));
            Console.WriteLine("FLeave: " + usr.BaseUser.Login);
        }

        void LeaveUserSession(USession ussi) // Event
        {
            Console.WriteLine("SLeave: " + ussi.BaseOnlineUser.BaseUser.Login);
           
        }

        void IChatService.Leave()
        {
            USession usen = USession.GetSession(onlineUsers, Callback);
            if (usen != null) usen.Leave();
            else Callback.RLeave();
        }

        void IChatService.SendMessage(int groupID, string message)
        {
            USession usen = USession.GetSession(onlineUsers, Callback);
            if (usen != null) usen.SendMessage(groupID, message);
            else Callback.RLeave();
        }

        int IChatService.GetCountUsers()
        {
            USession usen = USession.GetSession(onlineUsers, Callback);
            if (usen != null) return mainbBase.Users.Count() - 1;
            else { Callback.RLeave(); return -1; }
        }

        int IChatService.GetCountUsersInGroup(int groupID)
        {
            USession usen = USession.GetSession(onlineUsers, Callback);
            if (usen != null)
            {
                Group grp = usen.BaseOnlineUser.BaseUser.UsersInGroups.FirstOrDefault((x) => x.GroupID == groupID)?.Group;
                if (grp != null) return grp.UsersInGroups.Count - 1;
                else
                {
                    Callback.Error("Incorrect group!");
                    Callback.RLeaveGroup(new RGroup(new Group { ID = groupID }));
                    return -1;
                }
            }
            else { Callback.RLeave(); return -1; }
        }

        int IChatService.GetCountMessages(int groupID)
        {
            USession usen = USession.GetSession(onlineUsers, Callback);
            if (usen != null)
            {
                Group grp = usen.BaseOnlineUser.BaseUser.UsersInGroups.FirstOrDefault((x) => x.GroupID == groupID)?.Group;
                if (grp != null) return grp.GroupsMessages != null ? grp.GroupsMessages.Count : 0;
                else
                {
                    Callback.Error("Incorrect group!");
                    Callback.RLeaveGroup(new RGroup(new Group { ID = groupID }));
                    return -1;
                }
            }
            else { Callback.RLeave(); return -1; }
        }

        void IChatService.GetMyGroups()
        {
            USession usen = USession.GetSession(onlineUsers, Callback);
            if (usen != null) usen.GetMyGroups();
            else Callback.RLeave();
        }

        void IChatService.CreateGroup(string Name, int[] IDs)
        {
            USession usen = USession.GetSession(onlineUsers, Callback);
            if (usen != null) usen.CreateGroup(Name, IDs);
            else Callback.RLeave();

        }

        void IChatService.LeaveGroup(int ID)
        {
            USession usen = USession.GetSession(onlineUsers, Callback);
            if (usen != null) usen.LeaveFromGroup(ID);
            else Callback.RLeave();
        }

        void IChatService.AddUsersToGroup(int ID, int[] IDs)
        {
            USession usen = USession.GetSession(onlineUsers, Callback);
            if (usen != null) usen.AddUsersToGroup(ID, IDs);
            else Callback.RLeave();
        }

        RUser[] IChatService.GetUsers(int offset, int count)
        {
            USession usen = USession.GetSession(onlineUsers, Callback);
            if (usen != null)
            {

                IQueryable<User> users = mainbBase.Users.Where((x) => x.ID != usen.BaseOnlineUser.BaseUser.ID);
                if (offset > users.Count()) { Callback.Error("Incorrect offcet!"); return null; }
                if (offset > 0) users = users.Skip(offset);
                if (count > users.Count()) { Callback.Error("Incorrect count!"); return null; }
                if (count > 0) users = users.Take(count);
                List<RUser> rusers = new List<RUser>();
                foreach (var item in users)
                    rusers.Add(new RUser(item, onlineUsers.FirstOrDefault((x) => x.BaseUser.ID == item.ID) != null));
                return rusers.ToArray();
            }
            else { Callback.RLeave(); return null; }
        }

        public RMUserInGroup[] GetUsersInGroup(int groupID, int offset, int count)
        {
            USession usen = USession.GetSession(onlineUsers, Callback);
            if (usen != null)
            {
                Group grp = usen.BaseOnlineUser.BaseUser.UsersInGroups.FirstOrDefault((x) => x.GroupID == groupID)?.Group;
                if (grp == null)
                {
                    Callback.Error("Incorrect group!");
                    Callback.RLeaveGroup(new RGroup(new Group { ID = groupID }));
                    return null;
                }

                List<UserInGroup> users = grp.UsersInGroups != null ? grp.UsersInGroups.Where((x) => x.UserID != usen.BaseOnlineUser.BaseUser.ID).ToList() : new List<UserInGroup>(); 
                if (offset > users.Count()) { Callback.Error("Incorrect offcet!"); return null; }
                if (offset > 0) users = users.Skip(offset).ToList();
                if (count > users.Count()) { Callback.Error("Incorrect count!"); return null; }
                if (count > 0) users = users.Take(count).ToList();
                List<RMUserInGroup> rusers = new List<RMUserInGroup>();
                foreach (var item in users)
                    rusers.Add(new RMUserInGroup { FriendID = item.FriendID, Group = new RGroup(item.Group), Muted = item.Muted, Role = new RRole(item.Role), User = new RUser(item.User, onlineUsers.FirstOrDefault((x) => x.BaseUser.ID == item.UserID) != null) });
                return rusers.ToArray();
            }
            else { Callback.RLeave(); return null; }
        }

        public RUser[] GetUsersOutGroup(int groupID, int offset, int count)
        {
            USession usen = USession.GetSession(onlineUsers, Callback);
            if (usen != null)
            {
                Group grp = usen.BaseOnlineUser.BaseUser.UsersInGroups.FirstOrDefault((x) => x.GroupID == groupID)?.Group;
                if (grp == null)
                {
                    Callback.Error("Incorrect group!");
                    Callback.RLeaveGroup(new RGroup(new Group { ID = groupID }));
                    return null;
                }

                List<User> users = mainbBase.Users.Where((x) => x.ID != usen.BaseOnlineUser.BaseUser.ID).ToList();
                if (grp.UsersInGroups != null) users = users.Where((x) => grp.UsersInGroups.FirstOrDefault((z) => z.UserID == x.ID) == null).ToList();
                if (offset > users.Count()) { Callback.Error("Incorrect offcet!"); return null; }
                if (offset > 0) users = users.Skip(offset).ToList();
                if (count > users.Count()) { Callback.Error("Incorrect count!"); return null; }
                if (count > 0) users = users.Take(count).ToList();
                List<RUser> rusers = new List<RUser>();
                foreach (var item in users)
                    rusers.Add(new RUser(item, onlineUsers.FirstOrDefault((x) => x.BaseUser.ID == item.ID) != null));
                return rusers.ToArray();
            }
            else { Callback.RLeave(); return null; }
        }

        Dictionary<RUser, RGroupMessage> IChatService.GetMessages(int groupID, bool reverced, int count, int offset)
        {
            USession usen = USession.GetSession(onlineUsers, Callback);
            if (usen != null) {
                Group grp = usen.BaseOnlineUser.BaseUser.UsersInGroups.FirstOrDefault((x) => x.GroupID == groupID)?.Group;
                if (grp == null) {
                    Callback.Error("Incorrect group!");
                    Callback.RLeaveGroup(new RGroup(new Group { ID = groupID }));
                    return null;
                }
                List<GroupMessage> messages = grp.GroupsMessages.ToList();
                if (reverced) messages.Reverse();
                if (offset > messages.Count()) { Callback.Error("Incorrect offcet!"); return null; }
                if (offset > 0) messages = messages.Skip(offset).ToList();
                if (count > messages.Count()) { Callback.Error("Incorrect count!"); return null; }
                if (count > 0) messages = messages.Take(count).ToList();
                Dictionary<RUser, RGroupMessage> rmessages = new Dictionary<RUser, RGroupMessage>();
                foreach (var item in messages)
                    rmessages.Add(new RUser(item.User, false), new RGroupMessage(item));
                return rmessages;
            }
            else { Callback.RLeave(); return null; }
        }

        public void RemoveUserFromGroup(int groupID, int userID)
        {
            USession usen = USession.GetSession(onlineUsers, Callback);
            if (usen != null) usen.RemoveUserFromGroup(groupID, userID);
            else Callback.RLeave();
        }

        public void EditRoleUserInGroup(int groupID, int userID, int roleID)
        {
            throw new NotImplementedException();
        }

        public void MuteUserInGroup(int groupID, int userID)
        {
            throw new NotImplementedException();
        }
    }
}
