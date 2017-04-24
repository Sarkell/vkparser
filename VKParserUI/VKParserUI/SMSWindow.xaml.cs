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

namespace VKParserUI
{
    /// <summary>
    /// Логика взаимодействия для SMSWindow.xaml
    /// </summary>
    public partial class SMSWindow : Window
    {
        public SMSWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (textBox.Text != "")
            {
                this.Close();
            }
            else
            {
                MessageBox.Show("Пожалуйста введите пароль из смс");
            }
        }
    }
}
