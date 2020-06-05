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
    /// Interaction logic for CCheckBox.xaml
    /// </summary>
    public partial class CCheckUserItem : UserControl
    {

        private Color WpfBrushToDrawingColor(SolidColorBrush br)
        {
            return Color.FromArgb(
                br.Color.A,
                br.Color.R,
                br.Color.G,
                br.Color.B);
        }

        public bool Selected {
            get => chBox.Visibility == Visibility.Visible;
            set {
                if (value) chBox.Visibility = Visibility.Visible;
                else chBox.Visibility = Visibility.Hidden;
            }
        }

        public string Caption {
            get => usrName.Content.ToString();
            set => usrName.Content = value;
        }
        
        public bool Online {
            get => WpfBrushToDrawingColor((SolidColorBrush)onl.Fill) == Color.FromArgb(255, 81, 214, 81);
            set {
                if (value) onl.Fill = new SolidColorBrush(Color.FromArgb(255, 81, 214, 81));
                else onl.Fill = new SolidColorBrush(Color.FromArgb(255, 240, 137, 137));
            }
        } 

        public RUser User { get; set; }

        public CCheckUserItem(RUser user)
        {
            InitializeComponent();
            Caption = user.Login;
            Online = user.Online;
            // #FFF08989
            // #FF51D651



            User = user;
        }

        

        private void OnChange(object sender, RoutedEventArgs e)
        {
            Selected = !Selected;
        }
    }
}
