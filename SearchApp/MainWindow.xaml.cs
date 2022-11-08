using SearchApp.Lib.Models;
using SearchApp.Lib.Utilities;
using System;
using System.Windows;

namespace SearchApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ISearch _search;

        public MainWindow(ISearch search)
        {
            InitializeComponent();
            _search = search;
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {        
            try
            {
                SearchRequest request = new SearchRequest
                {
                    Keywords = KeywordsTextBox.Text,
                    Url = UrlTextBox.Text,
                    MaxNumber = Int32.Parse(MaxNumberTextBox.Text),
                };

                var response = await _search.SearchAsync(request);

                HighestAppearanceTextBox.Text = response.HighestAppearance.ToString();
                NumberOfAppearanceTextBox.Text = response.NumberOfAppearance.ToString();
                ResultListView.ItemsSource = response.Results;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
