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
        private bool searchNotClicked = true;
        public MainWindow()
        {
            InitializeComponent();

            try
            {
                createSaveFile();
            } catch (Exception)
            {
                MessageBox.Show("Issues creating save file ", "File Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void updateGalleryGrid(List<GalleryItemData>? currentItems)
        {
            if (currentItems.Count == 0) return;

            galleryItems.Clear();

            for(int i = 0; i < currentItems.Count; i++)
            {
                GalleryItemData data = currentItems[i];
                GalleryItem newItem = new GalleryItem(i, data.ImgName, data.Title, data.Desc);
                newItem.AddHandler(MouseLeftButtonDownEvent, new MouseButtonEventHandler(editGalleryItem));
                newItem.AddHandler(MouseRightButtonDownEvent, new MouseButtonEventHandler(selectGalleryItemForRemoval));
                galleryItems.Add(newItem);
            }

        }

        public void Window_Activated(object sender, EventArgs e)
        {
            try
            {
                updateGalleryGrid(GalleryItemData.getCurrentGallery());
                SearchBar_TextChanged(null, null);
            } catch (Exception)
            {
                MessageBox.Show("Issues reading save file ", "File Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
                return;
            }
        }

        private void editGalleryItem(object sender, RoutedEventArgs e)
        {
            GalleryItem item = (GalleryItem)sender;

            EditGalleryItem popup = new EditGalleryItem();
            popup.populateControlsForEditing(item.ListIndex, item.ImgName, item.Title, item.Desc);
            popup.ShowDialog();
        }

        private void selectGalleryItemForRemoval(object sender, RoutedEventArgs e)
        {
            GalleryItem item = (GalleryItem)sender;

            item.SetRemoveToggle(!item.GetRemoveToggle());
        }

        private void SearchBar_MouseDown(object sender, RoutedEventArgs e)
        {
            if (searchNotClicked)
            {
                SearchBar.Clear();
                searchNotClicked = false;
            }
        }

        private void SearchBar_TextChanged(object sender, RoutedEventArgs e)
        {
            if (searchNotClicked) return;
            
            string search = SearchBar.Text;

            try
            {
                if (search == "")
                {
                    updateGalleryGrid(GalleryItemData.getCurrentGallery());
                    return;
                }

                List<GalleryItemData>? newItems = GalleryItemData.searchGallery(search);

                if (newItems.Count == 0)
                {
                    galleryItems.Clear();
                } else
                {
                    updateGalleryGrid(newItems);
                }

            } catch (Exception)
            {
                return;
            }
        }

        

    }
}
