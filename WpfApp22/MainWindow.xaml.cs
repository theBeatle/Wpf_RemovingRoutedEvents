using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp22
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        List<RoutedEventHandler> delegates = new List<RoutedEventHandler>
        {
            (object sender, RoutedEventArgs e) => { MessageBox.Show("Hello 1"); },
            (object sender, RoutedEventArgs e) => { MessageBox.Show("Hello 2"); },
            (object sender, RoutedEventArgs e) => { MessageBox.Show("Hello 3"); },
            (object sender, RoutedEventArgs e) => { MessageBox.Show("Hello 4"); },
            (object sender, RoutedEventArgs e) => { MessageBox.Show("Hello 5"); }
        };

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var rnd = new Random();
            for (int i = 0; i < rnd.Next(5); i++)
            {
                Tester.Click += delegates[rnd.Next(delegates.Count)];
            }
        }

        public static RoutedEventHandlerInfo[] GetRoutedEventHandlers(UIElement element, RoutedEvent routedEvent)
        {
            // Get the EventHandlersStore instance which holds event handlers for the specified element.
            // The EventHandlersStore class is declared as internal.
            var eventHandlersStoreProperty = typeof(UIElement).GetProperty(
                "EventHandlersStore", BindingFlags.Instance | BindingFlags.NonPublic);
            object eventHandlersStore = eventHandlersStoreProperty.GetValue(element, null);

            // Invoke the GetRoutedEventHandlers method on the EventHandlersStore instance 
            // for getting an array of the subscribed event handlers.
            var getRoutedEventHandlers = eventHandlersStore.GetType().GetMethod(
                "GetRoutedEventHandlers", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var routedEventHandlers = (RoutedEventHandlerInfo[])getRoutedEventHandlers.Invoke(
                eventHandlersStore, new object[] { routedEvent });
            return routedEventHandlers;
        }

        private void RemoveClickEvent(Button b)
        {
            var routedEventHandlers = GetRoutedEventHandlers(b, ButtonBase.ClickEvent);
            foreach (var routedEventHandler in routedEventHandlers)
                b.Click -= (RoutedEventHandler)routedEventHandler.Handler;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            RemoveClickEvent(Tester);
            delegates.ForEach((item) => { if(delegates.IndexOf(item) %2 == 0) Tester.Click += item; });
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            RemoveClickEvent(Tester);
            delegates.ForEach((item) => { if (delegates.IndexOf(item) % 2 != 0) Tester.Click += item; });
        }
    }
}
