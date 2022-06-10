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
            InitializeComponent();

            try
            {
                createSaveFile();
            } catch (Exception)
            {
                MessageBox.Show("Save file was unable to be created", "File Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
                return;
            }

/*            for (int i = 0; i < 22; i++)
            {
                GalleryItem item = new GalleryItem();
                item.GalleryItemTitle.Content = "My Title Here";
                item.GalleryItemImage.Source = new BitmapImage(new Uri("pack://application:,,,/images/placeholder.jpg"));
                galleryItems.Add(item);
            }*/
        }

        private ObservableCollection<GalleryItem> galleryItems = new ObservableCollection<GalleryItem>();
        public ObservableCollection<GalleryItem> Gallery
        {
            get { return galleryItems; }
            set { galleryItems = value; }
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

        private void SearchBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SearchBar.Text = "";   
        }

        


    }
}
