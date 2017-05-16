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

namespace VKParserUI
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        public AuthorizationWindow()
        {
            InitializeComponent();
            try
            {
                Rect bounds = Properties.Settings.Default.WindowPosition;
                this.Top = bounds.Top;
                this.Left = bounds.Left;
                // Восстановить размеры, только если они
                // устанавливались для окна вручную
                if (this.SizeToContent == SizeToContent.Manual)
                {
                    this.Width = bounds.Width;
                    this.Height = bounds.Height;
                }
            }
            catch
            {
                MessageBox.Show("Нет сохраненных параметров");
            }
        }

        private void button_Communities_Search_Click(object sender, RoutedEventArgs e)
        {
            CommunitysSearchWindow a = new CommunitysSearchWindow();
            a.Show();
            this.Close();
        }

        private void button_Authorization_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button_Enter_Click(object sender, RoutedEventArgs e)
        {
            SMSWindow a = new SMSWindow();
            a.ShowDialog();           
        }

        private void button_Like_Repost_Click(object sender, RoutedEventArgs e)
        {
            LikeRepostWindow a = new LikeRepostWindow();
            a.Show();
            this.Close();
        }

    }
}
