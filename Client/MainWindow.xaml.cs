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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ChatClient clnt = new ChatClient();
        BlurEffect myBlur = null;

        public MainWindow()
        {
            InitializeComponent();

            myBlur = new BlurEffect();
            myBlur.Radius = 0;
           

            clnt.OnSetPage += SetPage;
            clnt.SetPage(new Login(clnt));
        }

        static void BlurApply(UIElement element,
        double blurRadius, TimeSpan duration, TimeSpan beginTime, EventHandler onBlured)
        {
            BlurEffect blur = new BlurEffect() { Radius = 0 };
            DoubleAnimation blurEnable = new DoubleAnimation(0, blurRadius, duration)
            { BeginTime = beginTime };
            element.Effect = blur;
            blurEnable.Completed += onBlured;
            blur.BeginAnimation(BlurEffect.RadiusProperty, blurEnable);
        }

        static void BlurDisable(UIElement element, TimeSpan duration, TimeSpan beginTime, EventHandler onBlured)
        {
            BlurEffect blur = element.Effect as BlurEffect;
            if (blur == null || blur.Radius == 0)
            {
                return;
            }
            DoubleAnimation blurDisable = new DoubleAnimation(blur.Radius, 0, duration) { BeginTime = beginTime };
            blurDisable.Completed += onBlured;
            blur.BeginAnimation(BlurEffect.RadiusProperty, blurDisable);
            //while (blur.Radius > 0) { }

        }

        private void OnBlured(object obj, EventArgs e)
        {

        }

        //private void AnimationUnBlur()
        //{
        //    DoubleAnimation da = new DoubleAnimation();
        //    da.To = 0;
        //    da.Duration = TimeSpan.FromSeconds(8);

        //    Storyboard sb = new Storyboard();
        //    Storyboard.SetTarget(da, mainFrm);
        //    Storyboard.SetTargetProperty(da, new PropertyPath("Effect.Radius"));
        //    sb.Children.Add(da);
        //    sb.Begin();
        //    while (sb.GetCurrentState() == ClockState.Active) { }
        //}

        public void SetPage(Page pg) {
            //BlurApply(mainFrm, 10, TimeSpan.FromSeconds(1), TimeSpan.Zero);
            //BlurApply(pg, 10000, TimeSpan.FromSeconds(10), TimeSpan.Zero);
            mainFrm.Navigate(pg);
            //BlurDisable(mainFrm, TimeSpan.FromSeconds(1), TimeSpan.Zero);
            //BlurDisable(pg, TimeSpan.FromSeconds(10), TimeSpan.Zero);
        }

        private void Frame_Navigating(object sender, NavigatingCancelEventArgs e)
        {

        }

        private void OnClising(object sender, System.ComponentModel.CancelEventArgs e)
        {
           
        }
    }
}
