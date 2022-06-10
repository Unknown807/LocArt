using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
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
                updateGalleryGrid();
            } catch (Exception ex)
            {
                //MessageBox.Show("Issues creating/reading save file ", "File Error", MessageBoxButton.OK, MessageBoxImage.Error);
                MessageBox.Show(ex.Message, "File Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
                return;
            }
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

        private void updateGalleryGrid()
        {
            List<GalleryItemData>? currentItems = GalleryItemData.getCurrentGallery();
            if (currentItems.Count == 0) return;

            for(int i = 0; i < currentItems.Count; i++)
            {
                GalleryItemData data = currentItems[i];
                GalleryItem newItem = new GalleryItem(i, data.ImgName, data.Title, data.Desc);
                galleryItems.Add(newItem);
            }

        }


    }
}
