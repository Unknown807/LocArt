using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Local_Gallery
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // For Testing
            for (int i = 0; i < 10; i++)
            {
                GalleryItem item = new GalleryItem();
                item.GalleryItemTitle.Content = "My Title Here";
                item.GalleryItemImage.Source = new BitmapImage(new Uri("pack://application:,,,/images/testimg.jpg"));
                TestGallery.Add(item);
            }
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {

        }

        void SearchBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SearchBar.Text = "";   
        }

        // For Testing
        private ObservableCollection<GalleryItem> testGalleryItems = new ObservableCollection<GalleryItem>();
        public ObservableCollection<GalleryItem> TestGallery
        {
            get { return testGalleryItems; }
            set { testGalleryItems = value; }
        }
    }
}
