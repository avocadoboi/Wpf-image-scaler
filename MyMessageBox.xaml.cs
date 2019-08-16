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
using System.Windows.Shapes;

namespace Mass_Image_Scaler
{
    /// <summary>
    /// Interaction logic for MyMessageBox.xaml
    /// </summary>
    public partial class MyMessageBox : Window
    {
        //-------------------------------------------------------------------------------------

        private void HandleButtonClicked_0(object sender, RoutedEventArgs e)
        {
            if ((string)button_0.Content == "OK")
            {
                result = MessageBoxResult.OK;
            }
            else if ((string)button_0.Content == "Yes")
            {
                result = MessageBoxResult.Yes;
            }
            Close();
        }

        private void HandleButtonClicked_1(object sender, RoutedEventArgs e)
        {
            if ((string)button_1.Content == "Cancel")
            {
                result = MessageBoxResult.Cancel;
            }
            else if ((string)button_0.Content == "No")
            {
                result = MessageBoxResult.No;
            }
            Close();
        }

        //-------------------------------------------------------------------------------------

        public MessageBoxResult result;

        public MyMessageBox(string message, string title, MessageBoxButton buttons)
        {
            InitializeComponent();

            window.Title = title;
            text.Text = message;

            if (buttons == MessageBoxButton.OK)
            {
                result = MessageBoxResult.OK;
                button_0.Content = "OK";

                Thickness margin = button_0.Margin;
                margin.Right = 0;
                button_0.Margin = margin;

                button_1.Visibility = Visibility.Hidden;
            }
            else if (buttons == MessageBoxButton.YesNo)
            {
                result = MessageBoxResult.No;
                button_0.Content = "Yes";
                button_1.Content = "No";
            }
        }

        //-------------------------------------------------------------------------------------

        static public MessageBoxResult Show(string message, string title, MessageBoxButton buttons)
        {
            MyMessageBox myMessageBox = new MyMessageBox(message, title, buttons);
            myMessageBox.ShowDialog();
            return myMessageBox.result;
        }
    }
}
