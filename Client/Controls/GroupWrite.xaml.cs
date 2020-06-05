using Client.ChatService;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client.Controls
{
    /// <summary>
    /// Interaction logic for GroupWrite.xaml
    /// </summary>
    public partial class GroupWrite : UserControl
    {
        GroupItem itmn;
        //List<RUser> outsiders = new List<RUser>(); 
        public EventHandler OnLoading;
        int lastID;
        HiddenItem AddUsers;
        StackPanel outsiderUsers;

        HiddenItem EditUsers;
        StackPanel Users;

        public List<RMUserInGroup> users = new List<RMUserInGroup>();


        public bool IsLoading
        {
            get => plashkaLoading.Height != 0;
            set {
                Application.Current.Dispatcher.Invoke((Action)delegate {
                    if (value) plashkaLoading.Height = Double.NaN;
                    else plashkaLoading.Height = 0;
                });
            }
        }

        public GroupWrite(GroupItem itmn)
        {
            InitializeComponent();
            this.itmn = itmn;

            outsiderUsers = new StackPanel();
            AddUsers = new HiddenItem("Add users", outsiderUsers);
            AddUsers.OnOpen += OnShow_PanelAddUsers;
            AddUsers.OnClose += OnHide_PanelAddUsers;
            AddUsers.OnConfirm += OnConfirm_PanelAddUsers;
            mainItems.Children.Add(AddUsers);

            Users = new StackPanel();
            EditUsers = new HiddenItem("Edit users", Users);
            EditUsers.OnOpen += OnShow_PanelAddUsers;
            EditUsers.OnClose += OnHide_PanelAddUsers;
            EditUsers.IsNavigatingVisible = false;
            mainItems.Children.Add(EditUsers);

            GetUsers();
            GetMessages();

            MsgScrolls.ScrollToBottom();
            GetOutsiders();
            


        }

        public void ObChangeUsers(object obj, EventArgs e) {

        }

        private void GetUsers()
        {
            new Task(() => {
                foreach (var item in itmn.client.Client.GetUsersInGroup(itmn.baseUserInGroup.Group.ID, -1, -1))
                    if (users.FirstOrDefault((x) => x.User.ID == item.User.ID) == null) users.Add(item);

                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    Users.Children.Clear();
                    foreach (var item in users)
                        Users.Children.Add(new UserEdit(item, itmn.baseUserInGroup, RemoveFromGroup));
                });
            }).Start();
        }

        public void RemoveFromGroup(object obj, EventArgs e) {
            RMUserInGroup usr = (RMUserInGroup)obj;
            itmn.client.Client.RemoveUserFromGroup(usr.Group.ID, usr.User.ID);
        }

        public void OnLeaveUserFromGroup(RGroup grp, RUser usr) {
            RMUserInGroup tmp = users.FirstOrDefault((x) => x.User.ID == usr.ID);
            if (tmp != null) {
                users.Remove(tmp);
                UserEdit tos = (UserEdit)Common.UIElementCollectionToList(Users.Children).FirstOrDefault((x) => ((UserEdit)x).User.User.ID == usr.ID);
                if (tos != null) Users.Children.Remove(tos);
                OnNewOutsider(usr);
            }
        }

        public void OnChangeOnline(RUser user)
        {
            foreach (var item in users)
                if (item.User.ID == user.ID)
                {
                    item.User.Online = user.Online;
                    break;
                }

            foreach (UserEdit item in Users.Children)
                if (item.User.User.ID == user.ID)
                {
                    item.Online = user.Online;
                    break;
                }

            foreach (CCheckUserItem item in Common.UIElementCollectionToList(outsiderUsers.Children).Where((x) => x is CCheckUserItem))
                if (item.User.ID == user.ID)
                {
                    item.Online = user.Online;
                    break;
                }

        }

        public void OnNewUsers(RMUserInGroup[] userss)
        {
            CCheckUserItem coms;
            foreach (var user in userss)
            {
                if (users.FirstOrDefault((x) => x.User.ID == user.User.ID) == null)
                {
                    users.Add(user);
                    Users.Children.Add(new UserEdit(user, itmn.baseUserInGroup, RemoveFromGroup));
                    coms = (CCheckUserItem)Common.UIElementCollectionToList(outsiderUsers.Children).FirstOrDefault((x) => x is CCheckUserItem && ((CCheckUserItem)x).User.ID == user.User.ID);
                    if (coms != null) outsiderUsers.Children.Remove(coms);
                }
            }
           
            
        }

        public void OnNewOutsider(RUser usr) {
            if (Common.UIElementCollectionToList(outsiderUsers.Children).FirstOrDefault((x) => x is CCheckUserItem && ((CCheckUserItem)x).User.ID == usr.ID) == null)
                outsiderUsers.Children.Add(new CCheckUserItem(usr));
        }

        public void GetMessages() {
            int count = msges.Children.Count-1;
            new Task(() => {
                IsLoading = true;
                int countMessages = itmn.client.Client.GetCountMessages(itmn.baseUserInGroup.Group.ID);
                if (countMessages - count > 0)
                {
                    Dictionary<RUser, RGroupMessage> messages = itmn.client.Client.GetMessages(itmn.baseUserInGroup.Group.ID, true, countMessages - count > 30 ? 30 : countMessages - count, count);
                    if (messages != null && messages.Count > 0)
                    {
                        Application.Current.Dispatcher.Invoke((Action)delegate {
                            foreach (var item in messages)
                                msges.Children.Insert(0, new GroupMessage(item.Key, item.Value));
                            itmn.SetExample(new GroupMessageMini(((GroupMessage)msges.Children[msges.Children.Count - 2]).baseUsr, ((GroupMessage)msges.Children[msges.Children.Count - 2]).baseMsg));
                            
                        });
                        
                    }
                }
                IsLoading = false;
            }).Start();
            
        }

        private void GetOutsiders()
        {
            new Task(() => {
                RUser[] users = itmn.client.Client.GetUsersOutGroup(itmn.baseUserInGroup.Group.ID, -1, -1);
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    foreach (var item in users)
                        outsiderUsers.Children.Add(new CCheckUserItem(item));
                });
            }).Start();

        }

        private void SendMsg(object sender, RoutedEventArgs e)
        {
            itmn.client.Client.SendMessage(itmn.BaseUserInGroup.Group.ID, msg.Text);
            msg.Text = String.Empty;
        }

        public void ReciveMessage(RUser usr, RGroupMessage grpMsg) {
            Application.Current.Dispatcher.Invoke((Action)delegate {
                bool isTo = MsgScrolls.VerticalOffset + 20 >= MsgScrolls.ViewportHeight;
                itmn.SetExample(new GroupMessageMini(usr, grpMsg));
                msges.Children.Insert(msges.Children.Count - 1, new GroupMessage(usr, grpMsg));
                if (isTo) MsgScrolls.ScrollToBottom();
            });
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void Msg_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) SendMsg(null, null);
        }

        private void Storyboard_Completed(object sender, EventArgs e)
        {
            anim.Begin();
        }

        private void MsgScrolls_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (MsgScrolls.VerticalOffset == 0 && IsLoading == false) GetMessages();
        }

        private void OnShow_PanelAddUsers(object sender, EventArgs e)
        {
            lastID = mainItems.Children.IndexOf((UIElement)sender);
            mainItems.Children.Remove((UIElement)sender);
            mainItems.Visibility = Visibility.Collapsed;
            mainMain.Children.Add((UIElement)sender);
        }

        private void OnHide_PanelAddUsers(object sender, EventArgs e)
        {
            mainMain.Children.Remove((UIElement)sender);
            mainItems.Visibility = Visibility.Visible;
            mainItems.Children.Insert(lastID, (UIElement)sender);
        }

        private void OnConfirm_PanelAddUsers(object sender, EventArgs e)
        {
            if (Common.UIElementCollectionToList(outsiderUsers.Children).FirstOrDefault((x) => ((CCheckUserItem)x).Selected) != null) {
                List<int> IDs = new List<int>();
                foreach (CCheckUserItem item in Common.UIElementCollectionToList(outsiderUsers.Children).Where((x) => ((CCheckUserItem)x).Selected))
                    IDs.Add(item.User.ID);
                itmn.client.Client.AddUsersToGroup(itmn.baseUserInGroup.Group.ID, IDs.ToArray());
                ((HiddenItem)sender).IsOpened = false;
            }
        }

        private void ExitFromAddUsers(object sender, RoutedEventArgs e)
        {
            //if (hidenBox.Visibility != Visibility.Collapsed)
            //{
            //    hidenBox.Visibility = Visibility.Collapsed;
            //    mainMain.Children.Remove(AddUsersItem);
            //    mainItems.Visibility = Visibility.Visible;
            //    mainItems.Children.Add(AddUsersItem);
            //    linia.Background = new SolidColorBrush(Color.FromArgb(255, 60, 138, 218));
            //}
        }

        private void RemoveUsers_Click(object sender, RoutedEventArgs e)
        {
           
        }
    }
}
