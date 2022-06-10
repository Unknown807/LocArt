using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Utilities;

namespace Local_Gallery
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            try
            {
                createSaveFile();
            } catch (Exception)
            {
                MessageBox.Show("Save file was unable to be created", "File Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            InitializeComponent();
            // For Testing
            for (int i = 0; i < 10; i++)
            {
                GalleryItem item = new GalleryItem();
                item.GalleryItemTitle.Content = "My Title Here";
                item.GalleryItemImage.Source = new BitmapImage(new Uri("pack://application:,,,/images/placeholder.jpg"));
                TestGallery.Add(item);
            }
        }

        private void createSaveFile()
        {
            if (File.Exists("../../../GallerySave.json")) return;
            GalleryItemData.setCurrentGallery(new List<GalleryItemData>());
            
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            EditGalleryItem popup = new EditGalleryItem();
            popup.ShowDialog();

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
