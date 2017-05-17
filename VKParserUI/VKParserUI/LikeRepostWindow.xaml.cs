using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Navigation;
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
    /// Логика взаимодействия для LikeRepostWindow.xaml
    /// </summary>
    public partial class LikeRepostWindow : Window
    {
        private ModelLikeRepost response;
        private VkApi VkApi = new VkApi();
        private String link;
        private bool isLikes = false;


        private readonly List<string> requestTypeParams = new List<string> { "пост", "комментарий", "фото", "видео", "заметка", "товар", "комментарий к фото", "комментарий к видео", "комментарий в обсуждении", "комментарий к товару" };


        void getMoreOrNothing(string link, bool isLikes)
        {
            int count = 0;
            const int max_count = 5;

            while (VkApi.isNotAllLikeRepostResult(response))
            {
                if (count < max_count)
                {
                    ModelLikeRepost newResult = VkApi.getLikesOrRepost(link, isLikes, response.response.arrayItem.Count);

                    if (newResult.response != null)
                    {
                        response.response.arrayItem.AddRange(newResult.response.arrayItem);
                        showLikesOrReposts(isLikes, newResult.response.arrayItem);
                    }
                    else
                    {
                        string errorText = String.Format("Error №{0}: {1}.", newResult.error.errorCode, newResult.error.errorMsg);
                        label_error.Content = errorText;
                        return;
                    }

                    count++;
                }
                else
                {
                    button_Load_More.Visibility = Visibility.Visible;
                    return;
                }
            }

            if (!VkApi.isNotAllLikeRepostResult(response))
            {
                button_Load_More.Visibility = Visibility.Collapsed;
            }
        }

        void showLikesOrReposts(bool isLikes, List<ModelLikeRepost.Response.Item> arrayForAdd)
        {
            // TODO: вывод в списке Likes|Reposts в формате:
            //  String.Format("vk.com/id{0} ({2} {3})", arrayForAdd[0].uid, arrayForAdd[0].FirstName, arrayForAdd[0].LastName);
            // + реализовать vk.com/id1 как кликабельную ссылку
            // или, если получится - "arrayForAdd.FirstName arrayForAdd.LastName", а по клику открывается ссылка vk.com/id + arrayForAdd.uid
            // так же сделать сохранения списка в новый файл (имя файла - текущая дата_likes|reposts .txt) в формате:
            //  String.Format("vk.com/id{0} ({2} {3})", arrayForAdd[0].uid, arrayForAdd[0].FirstName, arrayForAdd[0].LastName);

            // Аля Log
            System.IO.StreamWriter file = new System.IO.StreamWriter("C:\\Users\\Oleksii\\Desktop\\test.txt", true);
            foreach (ModelLikeRepost.Response.Item item in arrayForAdd)
            {
                file.WriteLine(String.Format("vk.com/id{0} ({1} {2})", item.uid, item.FirstName, item.LastName));

            }
            file.Close();

        }

        public LikeRepostWindow()
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

            // just for mocking data
            ModelUser user = new ModelUser();
            user.users.Add(new ModelUser.User(271636103, "NAMA", "SUR"));
            members_listview.ItemsSource = user.users;

            comboBox.ItemsSource = requestTypeParams;
            
        }

        private void Hyper_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("http://vk.com/id" + ((ModelUser.User)((Hyperlink)e.OriginalSource).DataContext).uId));
            e.Handled = true;
        }

        private void button_Authorization_Click(object sender, RoutedEventArgs e)
        {
            AuthorizationWindow a = new AuthorizationWindow();
            a.Show();
            this.Close();
        }


        private void button_Communities_Search_Click(object sender, RoutedEventArgs e)
        {
            CommunitysSearchWindow a = new CommunitysSearchWindow();
            a.Show();
            this.Close();
        }

        private void button_Like_Repost_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button_Like_Click(object sender, RoutedEventArgs e)
        {
            isLikes = true;
            response = VkApi.getLikesOrRepost(link, isLikes, 0);
            if (response.response == null)
            {
                string errorText = String.Format("Error №{0}: {1}.", response.error.errorCode, response.error.errorMsg);
                label_error.Content = errorText;
            }
            else
            {
                label_error.Content = "";
                showLikesOrReposts(isLikes, response.response.arrayItem);
                getMoreOrNothing(link, isLikes);
            }

        }

        private void button_Repost_Click(object sender, RoutedEventArgs e)
        {
            isLikes = false;
            response = VkApi.getLikesOrRepost(link, isLikes, 0);
            if (response.response == null)
            {
                label_error.Content = "";
                string errorText = String.Format("Error №{0}: {1}.", response.error.errorCode, response.error.errorMsg);
                label_error.Content = errorText;
            }
            else
            {
                showLikesOrReposts(isLikes, response.response.arrayItem);
                getMoreOrNothing(link, isLikes);
            }
        }

        private void button_Load_More_Click(object sender, RoutedEventArgs e)
        {
            getMoreOrNothing(link, isLikes);
        }

        private void textBox_URL_TextChanged(object sender, TextChangedEventArgs e)
        {
            link = textBox_URL.Text;
        }


    }
}
