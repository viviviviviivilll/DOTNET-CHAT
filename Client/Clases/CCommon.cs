using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Client
{
    public static class Common
    {
        public static List<UIElement> UIElementCollectionToList(UIElementCollection collection)
        {
            List<UIElement> elmnts = new List<UIElement>();
            foreach (UIElement item in collection)
                elmnts.Add(item);
            return elmnts;
        }
    }
}
