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

namespace Client
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Page
    {
        ChatClient clin = null;
        public Register(ChatClient clin)
        {
            InitializeComponent();
            this.clin = clin;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            clin.Client.Register(txtLogin.Text, txtEmail.Text, txtPass.Password, txtPassR.Password);
        }

        private void Button_Back(object sender, RoutedEventArgs e)
        {
            clin.SetPage(new Login(clin));
        }
    }
}
