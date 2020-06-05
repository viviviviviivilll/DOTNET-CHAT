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
    /// Interaction logic for HiddenItem.xaml
    /// </summary>
    public partial class HiddenItem : UserControl
    {
        public event EventHandler OnOpen, OnClose, OnConfirm;

        public HiddenItem(string name, UIElement el)
        {
            InitializeComponent();
            Caption.Content = name;
            cnt.Content = el;
        }

        public bool IsOpened {
            get => hidenBox.Visibility == Visibility.Visible;
            set {
                if (value == IsOpened) return;
                if (value)
                {
                    hidenBox.Visibility = Visibility.Visible;
                    linia.Background = new SolidColorBrush(Color.FromArgb(255, 90, 74, 241));
                    OnOpen?.Invoke(this, null);
                }
                else
                {
                    hidenBox.Visibility = Visibility.Collapsed;
                    linia.Background = new SolidColorBrush(Color.FromArgb(255, 60, 138, 218));
                    OnClose?.Invoke(this, null);
                }
            }
        }

        public bool IsNavigatingVisible {
            get => navigationBlock.Visibility == Visibility.Visible;
            set {
                if (value != IsNavigatingVisible)
                    navigationBlock.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void Go_Click(object sender, RoutedEventArgs e)
        {
            OnConfirm?.Invoke(this, null);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IsOpened = !IsNavigatingVisible ? !IsOpened : true;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            IsOpened = false;
        }
    }
}
