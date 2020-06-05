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
    public partial class CCheckBox : UserControl
    {

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

        public CCheckBox()
        {
            InitializeComponent();
        }

        public CCheckBox(string caption)
        {
            InitializeComponent();
            Caption = caption;
        }
    }
}
