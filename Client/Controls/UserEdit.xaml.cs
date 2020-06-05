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
    /// Interaction logic for UserEdit.xaml
    /// </summary>
    public partial class UserEdit : UserControl
    {
        private Color WpfBrushToDrawingColor(SolidColorBrush br)
        {
            return Color.FromArgb(
                br.Color.A,
                br.Color.R,
                br.Color.G,
                br.Color.B);
        }

        public string Caption
        {
            get => usrName.Content.ToString();
            set => usrName.Content = value;
        }

        public bool Online
        {
            get => WpfBrushToDrawingColor((SolidColorBrush)onl.Fill) == Color.FromArgb(255, 81, 214, 81);
            set
            {
                if (value) onl.Fill = new SolidColorBrush(Color.FromArgb(255, 81, 214, 81));
                else onl.Fill = new SolidColorBrush(Color.FromArgb(255, 240, 137, 137));
            }
        }

        public RMUserInGroup User { get; private set; }
        public RMUserInGroup MainUser { get; private set; }

        public EventHandler OnRemoveUser;

        public RRole UserRole {
            get => User.Role;
            set {
                User.Role = value;
                Revuse();
            }
        }

        public RRole MainUserRole
        {
            get => MainUser.Role;
            set
            {
                MainUser.Role = value;
                Revuse();
            }
        }

        private void Revuse() {
            if (MainUser.Role.ID >= User.Role.ID) contrlRemove.Visibility = Visibility.Collapsed;
            else contrlRemove.Visibility = Visibility.Visible;
        }

        public UserEdit(RMUserInGroup user, RMUserInGroup main, EventHandler OnRemove)
        {
            InitializeComponent();
            Caption = user.User.Login;
            Online = user.User.Online;
            User = user;
            MainUser = main;
            OnRemoveUser += OnRemove;
            Revuse();
        }

        private void RemoveUserFromGroup(object sender, RoutedEventArgs e)
        {
            OnRemoveUser?.Invoke(User, null);
        }
    }
}
