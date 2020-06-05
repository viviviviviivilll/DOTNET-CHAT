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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client.Controls
{
    /// <summary>
    /// Interaction logic for GroupCreateItem.xaml
    /// </summary>

    public delegate void Do();

    public partial class GroupCreateItem : UserControl
    {

        public event Do OnOk;
        public event Do OnCancel;

        ChatClient clin = null;
        int countGetUsers;

        public GroupCreateItem(ChatClient clin, int countGetUsers) {
            InitializeComponent();
            this.clin = clin;
            this.countGetUsers = countGetUsers;
            new Task(() =>
               {
                   int countUsers = clin.Client.GetCountUsers();
                   int offcet = 0;
                   List<RUser> userss = new List<RUser>();

                   while (countUsers > offcet)
                   {
                       if (countUsers <= countGetUsers)
                       {
                           userss.AddRange(clin.Client.GetUsers(offcet, -1));
                           countUsers = -1;
                       }
                       else
                       {
                           userss.AddRange(clin.Client.GetUsers(offcet, countGetUsers));
                           offcet += countGetUsers;
                           countUsers -= countGetUsers;
                       }
                   }
                   Application.Current.Dispatcher.Invoke((Action)delegate
                   {
                       foreach (var item in userss)
                           users.Children.Add(new CCheckUserItem(item));
                   });
               }).Start();
        }

        public void OnChangeOnline(RUser user)
        {
            int i = 0; for (; i < users.Children.Count; i++)
                if (((CCheckUserItem)users.Children[i]).User.ID == user.ID)
                {
                    ((CCheckUserItem)users.Children[i]).Online = user.Online;
                    break;
                }

            if (i == users.Children.Count) OnNewUser(user);
        }

        public void OnNewUser(RUser user)
        {
            if (Common.UIElementCollectionToList(users.Children).FirstOrDefault((x) => ((CCheckUserItem)x).User.ID == user.ID) == null)
                users.Children.Add(new CCheckUserItem(user));
        }

        private void CancelCreate(object sender, RoutedEventArgs e)
        {
            OnCancel?.Invoke();
        }

        private void OkCreate(object sender, RoutedEventArgs e)
        {
            List<int> IDs = new List<int>();
            Common.UIElementCollectionToList(users.Children).Where((x) => ((CCheckUserItem)x).Selected).ToList().ForEach((x) => IDs.Add(((CCheckUserItem)x).User.ID));
            clin.Client.CreateGroup(groupName.Text, IDs.ToArray());
            OnOk?.Invoke();
        }

    }
}
