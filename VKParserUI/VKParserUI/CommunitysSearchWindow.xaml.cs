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
    /// Логика взаимодействия для CommunitysSearchWindow.xaml
    /// </summary>
    public partial class CommunitysSearchWindow : Window
    {
        public CommunitysSearchWindow()
        {
            InitializeComponent();
        }

        private void button_Authorization_Click(object sender, RoutedEventArgs e)
        {
            AuthorizationWindow a = new AuthorizationWindow();
            a.Show();
            this.Close();
        }
          
        private void button_Like_Repost_Click(object sender, RoutedEventArgs e)
        {
            LikeRepostWindow a = new LikeRepostWindow();
            a.Show();
            this.Close();
        }

        private void textBox_searchWords_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void textBox_stopWords_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void checkBox_parametr_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void radioButton_event_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void radioButton_page_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void radioButton_group_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void comboBox_sort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void comboBox_country_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void comboBox_city_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void button_search_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button_loadMore_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button_saveToFile_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
