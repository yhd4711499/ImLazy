using System.Windows;
using System.Windows.Media;

namespace ImLazy.ControlPanel.Util
{
    public static class MyVisualTreeHelper
    {
        public static FrameworkElement GetTopParent(FrameworkElement element)
        {
            FrameworkElement topParent = null;
            while (element != null)
            {
                topParent = element;
                element = (FrameworkElement)element.Parent;
            }
            return topParent;
        }

        public static FrameworkElement GetParent(FrameworkElement element, string name)
        {
            FrameworkElement topParent = null;
            while (element != null)
            {
                topParent = element;
                if (element.Name == name)
                    return element;
                element = (FrameworkElement)element.Parent;
            }
            return topParent;
        }

        public static FrameworkElement GetTemplateParent(FrameworkElement element, string name)
        {
            FrameworkElement topParent = null;
            while (element != null)
            {
                topParent = element;
                if (element.Name == name)
                    return element;
                element = (FrameworkElement)element.TemplatedParent;
            }
            return topParent;
        }

        public static FrameworkElement GetTemplateParent<T>(FrameworkElement element)
        {
            FrameworkElement topParent = null;
            while (element != null)
            {
                topParent = element;
                if (element is T)
                    return element;
                element = (FrameworkElement)element.TemplatedParent;
            }
            return topParent;
        }

        public static FrameworkElement GetParent<T>(FrameworkElement element)
        {
            FrameworkElement topParent = null;
            while (element != null)
            {
                topParent = element;
                if (element is T)
                    return element;
                element = (FrameworkElement) VisualTreeHelper.GetParent(element);
                    //(FrameworkElement)element.Parent;
            }
            return topParent;
        }
    }
}
