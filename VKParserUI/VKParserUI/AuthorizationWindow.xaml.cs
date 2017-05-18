using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Threading;
using System.Reflection;

namespace VKParserUI
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {

        private String ACCESS_TOKEN = null;
        private String CLIENT_ID = null;

        private static WebBrowser sWebBrowser;
        private static TextBlock sTextAuthorisation;
        private static TextBlock sTextId;
        private static TextBlock sTextName;
        private static TextBlock sTextSurname;
        private static TextBlock sTextError;
        private static Button sButtonCommunitiesSearch;
        private static Button sButtonExit;

        public AuthorizationWindow()
        {
            InitializeComponent();
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

        private void button_Like_Repost_Click(object sender, RoutedEventArgs e)
        {
            LikeRepostWindow a = new LikeRepostWindow(ACCESS_TOKEN);
            a.Show();
            this.Close();
        }

        private void Form_Load(object sender, RoutedEventArgs e)
        {
            sWebBrowser = web_browser;
            sWebBrowser.Navigate("https://oauth.vk.com/authorize?client_id=6033617&scope=groups&redirect_uri=http://oauth.vk.com/blank.html&display=page&response_type=token");
        }

        private void web_browser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            string URL = sWebBrowser.Source.ToString();
            string patternAccessTokenMatch = @"^(?=.*\baccess_token=\b)(?=.*\buser_id=\b).*$";

            foreach (Match m in Regex.Matches(URL, patternAccessTokenMatch))
            {
                text_authorisation.Text = "Вы успешно авторизованы!";
                button_exit.Visibility = Visibility.Visible;

                sTextAuthorisation = text_authorisation;
                sTextId = text_id;
                sTextName = text_name;
                sTextSurname = text_surname;
                sButtonCommunitiesSearch = button_Communities_Search;
                sButtonExit = button_exit;
                sTextError = text_error;

                char[] Symbols = { '=', '&' };
                string[] URLs = URL.Split(Symbols);
                ACCESS_TOKEN = URLs[1];
                CLIENT_ID = URLs[5];

                VkApi VkApi = new VkApi(ACCESS_TOKEN, CLIENT_ID);
                ModelUser user = VkApi.getUserInfo();

                if (user.error != null)
                {
                    string errorText = String.Format("Error №{0}: {1}.", user.error.errorCode, user.error.errorMsg);
                    text_error.Visibility = Visibility.Visible;
                    text_error.Text = errorText;
                    return;
                }
                else
                {
                    text_error.Visibility = Visibility.Collapsed;
                }

                text_id.Text = user.users[0].uId.ToString();
                text_name.Text = user.users[0].name;
                text_surname.Text = user.users[0].surname;

                button_Communities_Search.IsEnabled = true;
            }

        }

        private void button_exit_Click(object sender, RoutedEventArgs e)
        {
            WebBrowser oldBrowser = sWebBrowser;

            if (sWebBrowser == web_browser)
            {
                web_browser2.Visibility = Visibility.Visible;
                sWebBrowser = web_browser2;
                web_browser.Visibility = Visibility.Collapsed;

            }
            else
            {
                web_browser.Visibility = Visibility.Visible;
                sWebBrowser = web_browser;
                web_browser2.Visibility = Visibility.Collapsed;
            }


            oldBrowser.ObjectForScripting = new ScriptInterface();
            oldBrowser.Navigate("javascript:void((function(){var a,b,c,e,f;f=0;a=document.cookie.split('; ');for(e=0;e<a.length&&a[e];e++){f++;for(b='.'+location.host;b;b=b.replace(/^(?:%5C.|[^%5C.]+)/,'')){for(c=location.pathname;c;c=c.replace(/.$/,'')){document.cookie=(a[e]+'; domain='+b+'; path='+c+'; expires='+new Date((new Date()).getTime()-1e11).toGMTString());}}}window.external.callMe();})())");
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            web_browser.Navigate("https://oauth.vk.com/authorize?client_id=6033617&scope=groups&redirect_uri=http://oauth.vk.com/blank.html&display=page&response_type=token");
        }

        [System.Runtime.InteropServices.ComVisibleAttribute(true)]
        public class ScriptInterface
        {
            public void callMe()
            {

                try
                {
                    sWebBrowser.Navigate("https://oauth.vk.com/authorize?client_id=6033617&scope=groups&redirect_uri=http://oauth.vk.com/blank.html&display=page&response_type=token");

                    sButtonExit.Visibility = Visibility.Collapsed;
                    sTextAuthorisation.Text = "Вы не авторизованы.";
                    sTextId.Text = "NONE";
                    sTextName.Text = "NONE";
                    sTextSurname.Text = "NONE";
                    sTextError.Visibility = Visibility.Collapsed;
                    sButtonCommunitiesSearch.IsEnabled = false;
                }
                catch (Exception)
                {

                }
            }
        }

    }
}
