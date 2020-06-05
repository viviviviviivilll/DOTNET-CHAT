using Client.ChatService;
using Client.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client.Pages
{
    /// <summary>
    /// Interaction logic for LoginedPage.xaml
    /// </summary>
    public partial class LoginedPage : Page
    {
        ChatClient clin;

        const int countGetUsers = 50;

        GroupCreateItem createGroupitem = null;

        public LoginedPage(ChatClient clin)
        {
            InitializeComponent();

            this.clin = clin;
            clin.Events.OnGroups += OnReciveGroups;
            clin.Events.OnLeaveGroup += OnLeaveGroup;
            clin.Events.OnNewGroup += OnNewGroup;
            clin.Events.OnNewMessage += OnReciveMessage;
            clin.Events.OnNewUsersInGroups += OnNewUsersInGroups;
            clin.Events.OnChangeOnline += OnChangeOnline;
            clin.Events.OnLeaveUserInGroup += OnLeaveUserFromGroup;

            createGroupitem = new GroupCreateItem(clin, countGetUsers);
            createGroupitem.OnOk += OnCreateGroup;
            createGroupitem.OnCancel += OnCancelCreateGroup;

            CreateGroup.Child = createGroupitem;

            //clin.Events.OnChangeOnline += OnChangeOnline;
            //clin.Events.OnNewUser += OnNewUser;
        }

        private void OnLeaveGroup(object obj, EventArgs e) {
            Application.Current.Dispatcher.Invoke((Action)delegate {

                RGroup rgp = (RGroup)obj;

                foreach (GroupItem item in ss.Children)
                    if (item.BaseUserInGroup.Group.ID == rgp.ID) {

                        if (item.Selected) writeMsg.Child = null;
                        ss.Children.Remove(item);
                        break;
                    }
            });
        }

        private void OnNewUsersInGroups(object obj, EventArgs e) {
            Application.Current.Dispatcher.Invoke((Action)delegate {
                RMUserInGroup[] rsm = (RMUserInGroup[])obj; 
                foreach (GroupItem item in ss.Children)
                    if (item.baseUserInGroup.Group.ID == rsm[0].Group.ID) item.WriteMessages.OnNewUsers(rsm);
            });
        }

        private void OnLeaveUserFromGroup(object obj, EventArgs e) {
            Application.Current.Dispatcher.Invoke((Action)delegate {
                KeyValuePair<RGroup, RUser> user = (KeyValuePair<RGroup, RUser>)obj;
                foreach (GroupItem item in ss.Children)
                    if (item.baseUserInGroup.Group.ID == user.Key.ID) item.WriteMessages.OnLeaveUserFromGroup(user.Key, user.Value);
            });
        }

        public void OnNewGroup(object obj, EventArgs e) {
            Application.Current.Dispatcher.Invoke((Action)delegate {
                RMUserInGroup usrInGrp = (RMUserInGroup)obj;
                ss.Children.Add(new GroupItem(usrInGrp, clin, OnChangeSelectedGroup));
                OnCancelCreateGroup();
            });
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                writeMsg.Child = null;
                clin.Client.GetMyGroups();
            }
            catch (Exception)
            {
            }
        }

        

        private void OnReciveGroups(object obj, EventArgs e) {
            Application.Current.Dispatcher.Invoke((Action)delegate {
                RMUserInGroup[] gprs = (RMUserInGroup[])obj;
                ss.Children.Clear();
                foreach (var item in gprs)
                    ss.Children.Add(new GroupItem(item, clin, OnChangeSelectedGroup));
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clin.Client.Leave();
            }
            catch (Exception)
            {
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void OnCreateGroup() {
            GroupsItems.Visibility = Visibility.Visible;
            CreateGroup.Visibility = Visibility.Hidden;
        }

        private void OnCancelCreateGroup() {
            GroupsItems.Visibility = Visibility.Visible;
            CreateGroup.Visibility = Visibility.Hidden;
        }

        private void Create_Group(object sender, RoutedEventArgs e)
        {
            if (GroupsItems.Visibility != Visibility.Hidden) {
                GroupsItems.Visibility = Visibility.Hidden;
                CreateGroup.Visibility = Visibility.Visible;
            }
            else {
                GroupsItems.Visibility = Visibility.Visible;
                CreateGroup.Visibility = Visibility.Hidden;
            }
        }

        public void OnChangeSelectedGroup(object obj, EventArgs e) {
            if (((GroupItem)obj).Selected) {
                writeMsg.Child = ((GroupItem)obj).WriteMessages;
                foreach (GroupItem item in ss.Children)
                    if (item.BaseUserInGroup.Group.ID != ((GroupItem)obj).BaseUserInGroup.Group.ID)
                        item.Selected = false;
            }
        }

        public void OnReciveMessage(object obj, EventArgs e) {
            Application.Current.Dispatcher.Invoke((Action)delegate {
                KeyValuePair<RUser, RGroupMessage> msg = (KeyValuePair<RUser, RGroupMessage>)obj;
                foreach (GroupItem item in ss.Children)
                    if (item.baseUserInGroup.Group.ID == msg.Value.GroupID) item.ReciveMessage(msg.Key, msg.Value);
            });
        }

        public void OnChangeOnline(object obj, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                RUser user = (RUser)obj;
                createGroupitem.OnChangeOnline(user);
                foreach (GroupItem item in ss.Children)
                    item.WriteMessages.OnChangeOnline(user);
            });
        }

        public void OnNewUser(RUser user)
        {
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                createGroupitem.OnNewUser(user);
                foreach (GroupItem item in ss.Children)
                    item.WriteMessages.OnNewOutsider(user);
            });
        }

    }
}
