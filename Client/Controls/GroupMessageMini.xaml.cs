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
    /// Interaction logic for GroupMessage.xaml
    /// </summary>
    public partial class GroupMessageMini : UserControl
    {
        public ChatService.RGroupMessage baseMsg;
        public ChatService.RUser baseUsr;

        public GroupMessageMini(RUser usr, RGroupMessage grpMsg)
        {
            InitializeComponent();

            UserName.Content = usr.Login;
            Teext.Content = grpMsg.MessageSource;

            baseMsg = grpMsg;
            baseUsr = usr;
        }
    }
}
