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
        private ObservableCollection<GalleryItem> galleryItems = new ObservableCollection<GalleryItem>();

        /// <summary>
        /// Constructor
        /// </summary>
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

        /// <summary>
        /// Dynamically changes the list of gallery items on-screen
        /// </summary>
        public ObservableCollection<GalleryItem> Gallery
        {
            get { return galleryItems; }
            set { galleryItems = value; }
        }

        /// <summary>
        /// If there is no save file, then create a new empty one
        /// </summary>
        private void createSaveFile()
        {
            if (File.Exists("../../../GallerySave.json")) return;
            GalleryItemData.setCurrentGallery(new Dictionary<int, GalleryItemData>());
            
        }

        /// <summary>
        /// Opens a popup window to allow the user to create a new gallery item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            EditGalleryItem popup = new EditGalleryItem();
            popup.ShowDialog();

        }

        /// <summary>
        /// Updates the current view of the gallery
        /// </summary>
        /// <param name="currentItems">The 'list' of items to update the view of the gallery with</param>
        private void updateGalleryGrid(Dictionary<int, GalleryItemData>? currentItems)
        {
            if (currentItems.Count == 0) return;

            galleryItems.Clear();

            foreach(int i in currentItems.Keys)
            {
                GalleryItemData data = currentItems[i];
                GalleryItem newItem = new GalleryItem(i, data.ImgName, data.Title, data.Desc);
                newItem.AddHandler(MouseLeftButtonDownEvent, new MouseButtonEventHandler(editGalleryItem));
                newItem.AddHandler(MouseRightButtonDownEvent, new MouseButtonEventHandler(selectGalleryItemForRemoval));
                galleryItems.Add(newItem);
            }

        }

        /// <summary>
        /// Everytime the window gains focus, it checks if any gallery items have been added or edited
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// When a gallery item is left-clicked, it opens a popup to let the user modify it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editGalleryItem(object sender, RoutedEventArgs e)
        {
            GalleryItem item = (GalleryItem)sender;

            EditGalleryItem popup = new EditGalleryItem();
            popup.populateControlsForEditing(item.DictIndex, item.ImgName, item.Title, item.Desc);
            popup.ShowDialog();
        }

        /// <summary>
        /// When a gallery item is right-clicked, it changes its background color to indicate its
        /// been marked for removal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectGalleryItemForRemoval(object sender, RoutedEventArgs e)
        {
            GalleryItem item = (GalleryItem)sender;

            item.SetRemoveToggle(!item.GetRemoveToggle());
        }

        /// <summary>
        /// The first time the search bar is clicked it should remove the text, like a placeholder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchBar_MouseDown(object sender, RoutedEventArgs e)
        {
            if (searchNotClicked)
            {
                SearchBar.Clear();
                searchNotClicked = false;
            }
        }

        /// <summary>
        /// Every time the user changes the text in the search bar, the search results should display automatically on-screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

                Dictionary<int, GalleryItemData>? newItems = GalleryItemData.searchGallery(search);

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

        /// <summary>
        /// When the remove button is clicked, the program will pass the current list of gallery items
        /// and those that are marked will be removed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Checks if there is at least one item to remove
                bool remove = false;
                foreach(GalleryItem item in galleryItems) remove |= item.GetRemoveToggle();

                if (!remove) return;

                Dictionary<int, GalleryItemData>? newItems = GalleryItemData.removeGalleryItems(galleryItems);
                if (newItems.Count == 0)
                {
                    galleryItems.Clear();
                }
                {
                    updateGalleryGrid(newItems);
                }
                
            } catch (Exception)
            {
                MessageBox.Show("An error occurred while trying to remove the selected items", "Program Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
