﻿using System;
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
using System.Windows.Shapes;
using System.IO;

namespace VKParserUI
{
    /// <summary>
    /// Логика взаимодействия для LikeRepostWindow.xaml
    /// </summary>
    public partial class LikeRepostWindow : Window
    {
        private string ACCESS_TOKEN;
        private ModelLikeRepost response;
        private VkApi VkApi;
        private string link;
        private bool isLikes = false;
        private int selectedType;

        private readonly List<string> requestTypeParams = new List<string> { "Пост", "Комментарий к посту", "Фото", "Видео", "Заметка", "Товар", "Комментарий к фото", "Комментарий к видео", "Комментарий в обсуждении", "Комментарий к товару" };

        public LikeRepostWindow()
        {
            InitializeComponent();
        }

        public LikeRepostWindow(string accessToken)
        {
            InitializeComponent();

            ACCESS_TOKEN = accessToken;
            VkApi = new VkApi(ACCESS_TOKEN);

            if (ACCESS_TOKEN == null)
            {
                button_Communities_Search.IsEnabled = false;
            }
            else
            {
                button_Communities_Search.IsEnabled = true;
            }

            textBlock_loading.Visibility = Visibility.Hidden;
            comboBox.ItemsSource = requestTypeParams;
        }

        void getMoreOrNothing(string link, bool isLikes)
        {
            int count = 0;
            const int max_count = 5;

            while (VkApi.isNotAllLikeRepostResult(response))
            {
                if (count < max_count)
                {
                    ModelLikeRepost newResult = VkApi.getLikesOrRepost(link, isLikes, response.response.arrayItem.Count, selectedType);

                    if (newResult.response != null)
                    {
                        response.response.arrayItem.AddRange(newResult.response.arrayItem);
                        showLikesOrReposts(isLikes, newResult.response.arrayItem);
                    }
                    else
                    {
                        string errorText = String.Format("Error №{0}: {1}.", newResult.error.errorCode, newResult.error.errorMsg);
                        label_error.Text = errorText;
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
            //TODO: сделать это после вывода списка
           

            // TODO: вывод в списки Likes и Reposts в формате Имя Фамилия и по клику открывется ссылка vk.com/id..
            this.isLikes = isLikes;
            members_listview.ItemsSource = arrayForAdd;
            textBlock_loading.Visibility = Visibility.Collapsed;

            // Аля Log
            //System.IO.StreamWriter file = new System.IO.StreamWriter("C:\\Users\\Oleksii\\Desktop\\test.txt", true);
            //foreach (ModelLikeRepost.Response.Item item in arrayForAdd)
            //{
            //    file.WriteLine(String.Format("vk.com/id{0} ({1} {2})", item.uid, item.FirstName, item.LastName));

            //}
            //file.Close();

        }

        private void Hyper_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("http://vk.com/id" + ((ModelLikeRepost.Response.Item)((Hyperlink)e.OriginalSource).DataContext).uid));
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

        private void button_Like_Click(object sender, RoutedEventArgs e)
        {
            textBlock_loading.Visibility = Visibility.Visible;

            isLikes = true;
            selectedType = comboBox.SelectedIndex;
            response = VkApi.getLikesOrRepost(link, isLikes, 0, comboBox.SelectedIndex);
            if (response.response == null)
            {
                string errorText = String.Format("Error №{0}: {1}.", response.error.errorCode, response.error.errorMsg);
                label_error.Text = errorText;
            }
            else
            {
                label_error.Text = "";
                showLikesOrReposts(isLikes, response.response.arrayItem);
                getMoreOrNothing(link, isLikes);
            }

        }

        private void button_Repost_Click(object sender, RoutedEventArgs e)
        {
            textBlock_loading.Visibility = Visibility.Visible;

            isLikes = false;
            selectedType = comboBox.SelectedIndex;
            response = VkApi.getLikesOrRepost(link, isLikes, 0, comboBox.SelectedIndex);
            if (response.response == null)
            {
                label_error.Text = "";
                string errorText = String.Format("Error №{0}: {1}.", response.error.errorCode, response.error.errorMsg);
                label_error.Text = errorText;
            }
            else
            {
                showLikesOrReposts(isLikes, response.response.arrayItem);
                getMoreOrNothing(link, isLikes);
            }
        }

        private void button_Load_More_Click(object sender, RoutedEventArgs e)
        {
            textBlock_loading.Visibility = Visibility.Visible;

            getMoreOrNothing(link, isLikes);
        }

        private void textBox_URL_TextChanged(object sender, TextChangedEventArgs e)
        {
            link = textBox_URL.Text;
        }

        private void saveInFile_button_Click(object sender, RoutedEventArgs e)
        {
            string type = isLikes == true ? "likes" : "reposts";
            //TODO: реализовать сохранение в файле
            //Поставить две кнопки для сохранений отдельно Лайков отдельно Репостов 
            //с названием по формату текущая "дата_likes|reposts .txt"


            //Проверь пжл у себя на пк. 
            string header = $"{textBox_URL.Text}/n{type}";
            string fileName = $"{DateTime.Now}_{type}.txt".Replace('/','.').Replace(' ', '_');
            string writePath = @"D:\" + fileName;

            try
            {
                File.Create(writePath);
                using (StreamWriter sw = new StreamWriter(writePath))
                {
                    sw.WriteLine(header);
                    foreach (ModelLikeRepost.Response.Item item in members_listview.Items)
                    {
                        sw.WriteLine($"http://vk.com/id{item.uid}");
                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

    }
}
