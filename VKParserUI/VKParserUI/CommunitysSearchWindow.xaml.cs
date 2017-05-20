using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace VKParserUI
{
    public partial class CommunitysSearchWindow : Window
    {

        private string ACCESS_TOKEN;
        private VkApi VkApi;
        private ModelCountriesAndCities modelCountries;
        private ModelCountriesAndCities.CountryOrCity currentCountry;
        private ModelCountriesAndCities modelCities;
        private ModelCountriesAndCities.CountryOrCity currentCity;
        private int sortTypeIndex;
        private bool isFutureEvent;
        private bool isWithMarket;
        private string searchFor;
        private string stopWords;
        private int searchType;
        private ModelGroup modelGroupWithStopWords;
        private ModelGroup modelGroup;

        private readonly List<string> sortParams = new List<string> { "По умолчанию", "По скорости роста", "По отношению дневной посещяемости к количеству пользователей", "По отношению количества лайков к количеству пользователей", "По отношению количества комментариев к количеству пользователей", "По отношению количества записей в обсуждениях к количеству пользователей" };

        public CommunitysSearchWindow()
        {
            InitializeComponent();
        }

        public CommunitysSearchWindow(string accessToken)
        {
            InitializeComponent();

            ACCESS_TOKEN = accessToken;
            VkApi = new VkApi(ACCESS_TOKEN);
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            modelCountries = VkApi.getCountries();
            foreach (ModelCountriesAndCities.CountryOrCity item in modelCountries.listOfCountryOrCity)
                comboBox_country.Items.Add(item.title);

            comboBox_sort.ItemsSource = sortParams;

            textBlock_loading.Visibility = Visibility.Collapsed;
            textBlock_error.Visibility = Visibility.Collapsed;
            button_loadMore.Visibility = Visibility.Collapsed;
            button_saveToFile.IsEnabled = false;
        }

        private void button_Authorization_Click(object sender, RoutedEventArgs e)
        {
            AuthorizationWindow a = new AuthorizationWindow();
            a.Show();
            this.Close();
        }

        private void button_Like_Repost_Click(object sender, RoutedEventArgs e)
        {
            LikeRepostWindow a = new LikeRepostWindow(ACCESS_TOKEN);
            a.Show();
            this.Close();
        }

        private void textBox_searchWords_TextChanged(object sender, TextChangedEventArgs e)
        {
            searchFor = textBox_searchWords.Text;
        }

        private void textBox_stopWords_TextChanged(object sender, TextChangedEventArgs e)
        {
            stopWords = textBox_stopWords.Text;
        }

        private void checkBox_parametr_Checked(object sender, RoutedEventArgs e)
        {
            isWithMarket = (bool)checkBox_parametr.IsChecked;
        }

        private void checkBox_parametr_event_Checked(object sender, RoutedEventArgs e)
        {
            isFutureEvent = (bool)checkBox_parametr_event.IsChecked;
        }

        private void checkBox_parametr_Unchecked(object sender, RoutedEventArgs e)
        {
            isWithMarket = (bool)checkBox_parametr.IsChecked;
        }

        private void checkBox_parametr_event_Unchecked(object sender, RoutedEventArgs e)
        {
            isFutureEvent = (bool)checkBox_parametr_event.IsChecked;
        }

        private void radioButton_all_Checked(object sender, RoutedEventArgs e)
        {
            searchType = 0;
            checkBox_parametr_event.Visibility = Visibility.Collapsed;
        }

        private void radioButton_group_Checked(object sender, RoutedEventArgs e)
        {
            searchType = 1;
            checkBox_parametr_event.Visibility = Visibility.Collapsed;
        }

        private void radioButton_event_Checked(object sender, RoutedEventArgs e)
        {
            searchType = 2;
            checkBox_parametr_event.Visibility = Visibility.Visible;
        }

        private void radioButton_page_Checked(object sender, RoutedEventArgs e)
        {
            searchType = 3;
            checkBox_parametr_event.Visibility = Visibility.Collapsed;
        }

        private void comboBox_sort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sortTypeIndex = comboBox_sort.SelectedIndex;
        }

        private void comboBox_country_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectIndex = comboBox_country.SelectedIndex;
            if (selectIndex == -1)
                return;

            textBlock_loading.Visibility = Visibility.Visible;
            currentCountry = modelCountries.listOfCountryOrCity[selectIndex];
            modelCities = VkApi.getCitiesByCountry(currentCountry);
            comboBox_city.Items.Clear();
            foreach (ModelCountriesAndCities.CountryOrCity item in modelCities.listOfCountryOrCity)
                comboBox_city.Items.Add(item.title);
            textBlock_loading.Visibility = Visibility.Collapsed;
        }

        private void comboBox_city_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectIndex = comboBox_city.SelectedIndex;
            if (selectIndex == -1)
                return;

            textBlock_loading.Visibility = Visibility.Visible;
            currentCity = modelCities.listOfCountryOrCity[selectIndex];
            textBlock_loading.Visibility = Visibility.Collapsed;
        }

        private void button_search_Click(object sender, RoutedEventArgs e)
        {
            modelGroupWithStopWords = null;

            button_loadMore.Visibility = Visibility.Collapsed;
            textBlock_error.Visibility = Visibility.Collapsed;
            textBlock_loading.Visibility = Visibility.Visible;
            button_saveToFile.IsEnabled = false;

            load(0);
        }

        private void load(int offset)
        {
            ModelGroup newModelGroup = VkApi.searchGroups(searchFor, searchType, currentCountry, currentCity, isFutureEvent, isWithMarket, sortTypeIndex, offset);

            if(newModelGroup == null || (newModelGroup.error == null && newModelGroup.groups == null))
            {
                textBlock_loading.Visibility = Visibility.Collapsed;
                textBlock_error.Text = "По вашему запросу ничего нет";
                textBlock_error.Visibility = Visibility.Visible;

                return;
            }

            if (newModelGroup.error != null)
            {
                textBlock_error.Text = string.Format("Error №{0}: {1}.", newModelGroup.error.errorCode, newModelGroup.error.errorMsg);
                textBlock_error.Visibility = Visibility.Visible;
            }
            else
            {
                ModelGroup groupAfterStopWords = makeStopWords(newModelGroup);

                modelGroupWithStopWords = concat(modelGroupWithStopWords, groupAfterStopWords);
                modelGroup = concat(modelGroup, newModelGroup);

                showGroups(groupAfterStopWords, searchType);

                button_saveToFile.IsEnabled = true;

                button_loadMore.Visibility = modelGroup.count > modelGroup.groups.Count ? Visibility.Visible : Visibility.Collapsed;
            }

        }

        private ModelGroup concat(ModelGroup _in, ModelGroup _from)
        {
            if (_in == null)
                _in = _from;
            else
                _in.groups.AddRange(_from.groups);

            return _in;
        }

        private void button_loadMore_Click(object sender, RoutedEventArgs e)
        {
            load(modelGroup.groups.Count);
        }

        private void button_saveToFile_Click(object sender, RoutedEventArgs e)
        {
            //TODO: реализовать сохранение в файл, весь список хранится в modelGroupWithStopWords.groups
            // сохранять в формате как и выводишь, т.е.
            // заголовк - запрос searchFor
            // если группа/событие/паблик шапка (группы/события/паблики) и выводить в формате addModelGroup.groups[0].name (https://vk.com/addModelGroup.groups[0].screenName)
            // если тип все, то выводить в формате addModelGroup.groups[0].type: addModelGroup.groups[0].name (https://vk.com/addModelGroup.groups[0].screenName)
        }

        private ModelGroup makeStopWords(ModelGroup modelGroup)
        {
            //TODO: сделать выборку из списка addModelGroup.groups по полю addModelGroup.groups[0].name
            // и удалить все те элементы списка, в котором встречаются слова или слово из stopWords
            return modelGroup;
        }

        private void showGroups(ModelGroup addModelGroup, int type)
        {
            //TODO: вывести список всех (type = 0)|групп(1)|событий(2)|пабликов(3)
            // в зависимости от типа менять заголовок стобца (группа/событие/паблик)
            // выводить в формате addModelGroup.groups[0].name (https://vk.com/addModelGroup.groups[0].screenName)
            // если тип все, то выводить в формате: addModelGroup.groups[0].type: addModelGroup.groups[0].name (https://vk.com/addModelGroup.groups[0].screenName)
            // addModelGroup необходимо добовлять в список, а не заменять

            // Аля Log
            System.IO.StreamWriter file = new System.IO.StreamWriter("C:\\Users\\Oleksii\\Desktop\\test.txt", true);
            foreach (ModelGroup.Group item in addModelGroup.groups)
            {
                file.WriteLine(String.Format("{0} - {1} (vk.com/{2})", item.type, item.name, item.screenName));

            }
            file.Close();

            // в самом конце
            textBlock_loading.Visibility = Visibility.Collapsed;
        }

        private void button_CountryCityEmpty_Click(object sender, RoutedEventArgs e)
        {
            comboBox_city.SelectedIndex = -1;
            comboBox_city.Items.Clear();
            comboBox_country.SelectedIndex = -1;
        }
    }
}
