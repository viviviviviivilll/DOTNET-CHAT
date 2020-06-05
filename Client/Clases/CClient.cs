using Client.ChatService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Client
{
    public delegate void SetPage(Page pg);

    public class ChatClient
    {
        public MyCallback Events { get; private set; } 
        public ChatService.ChatServiceClient Client { get; private set; }

        public ChatService.RUser usr;

        List<Page> pages = new List<Page>();

        public event SetPage OnSetPage;

        public ChatClient() {
            Events = new MyCallback();
            Events.OnLogin += Login;
            Events.OnLeave += Leave;
            Client = new ChatService.ChatServiceClient(new InstanceContext(Events));
        }

        private void Login(object obj, EventArgs e) {
            Application.Current.Dispatcher.Invoke((Action)delegate {
                RUser usr = (RUser)obj;
                SetPage(new Pages.LoginedPage(this));
            });
            
        }

        private void Leave(object obj, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke((Action)delegate {
                SetPage(new Login(this));
            });
        }

        public void SetPage(Page pg) {
            if (pages.FirstOrDefault((x) => x.GetType().FullName == pg.GetType().FullName) != null)
                OnSetPage?.Invoke(pages.FirstOrDefault((x) => x.GetType().FullName == pg.GetType().FullName));
            else {
                pages.Add(pg);
                OnSetPage?.Invoke(pg);
            }
        }
    }
}
