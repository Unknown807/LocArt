using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Utilities;

namespace Local_Gallery
{
    /// <summary>
    /// Interaction logic for EditGalleryItem.xaml,
    /// serves the dual purpose of creating and editing
    /// gallery items
    /// </summary>
    public partial class EditGalleryItem : Window
    {
        private int itemIndex = -1;
        private bool editing = false;
        private bool titleNotClicked = true, descNotClicked = true;

        private string tempGalleryImage = "";
        public EditGalleryItem()
        {
            InitializeComponent();
            GalleryItemDesc.Document.Blocks.Clear();
            GalleryItemDesc.AppendText("Enter description here (use # to indicate keywords)");
        }

        /// <summary>
        /// When the image is clicked, it opens a file dialog for choosing a new image. When an
        /// image is chosen, it places it in the 'Temp_images/' folder and only once the gallery item
        /// is finalised is the image moved permanently to 'Images/', otherwise the image will be removed
        /// from 'Temp_images/' or replaced by another image selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void GalleryItemImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = "c:\\";
            dlg.Filter = "Image files (*.jpg)|*.jpg|Image files (*.png)|*.png|All Files (*.*)|*.*";
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog() != true) return;
            
            string fileSource = dlg.FileName;
            string fileDest = "../../../Temp_images/" + Randomiser.generateRandAlphaNumStr() + "_" + Path.GetFileName(fileSource);

            if (!fileDest.Contains(".jpg") & !fileDest.Contains(".png"))
            {
                MessageBox.Show("Incorrect file extension, file must be jpg or png", "File Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try {
                removeTempImage();
                File.Copy(fileSource, fileDest, true);
                tempGalleryImage = fileDest;
            } catch (Exception) {
                MessageBox.Show("Unexpected issue occurred copying the file over", "File Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            BitmapImage bitmap = new BitmapImage(new Uri(fileSource));
            GalleryItemImage.Source = bitmap;
        }

        /// <summary>
        /// Checks if the title is blank or if the placeholder image is being used
        /// </summary>
        /// <returns>true if everything is valid data</returns>
        private bool validateGalleryDetails()
        {
            if (GalleryItemTitle.Text == "")
            {
                MessageBox.Show("The title cannot be left blank", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (tempGalleryImage == "" & !editing)
            {
                MessageBox.Show("The image cannot be left blank", "Image Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        /// <summary>
        /// When focus from the description box is lost, it will update the description by highlighting all the
        /// keywords
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GalleryDesc_LostFocus(object sender, RoutedEventArgs e)
        {
            TextStyler.applyKeyWordStyling(TextStyler.getAllKeyWords(GalleryItemDesc.Document), Brushes.DeepSkyBlue);
        }

        /// <summary>
        /// First time clicking will just get rid of some placeholder text.
        /// Every time you click, the text will un-highlight the keywords 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GalleryDesc_MouseDown(object sender, RoutedEventArgs e)
        {
            if (descNotClicked)
            {
                GalleryItemDesc.Document.Blocks.Clear();
                descNotClicked = false;
            }

            new TextRange(
                GalleryItemDesc.Document.ContentStart,
                GalleryItemDesc.Document.ContentEnd
            ).ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Black);
        }

        /// <summary>
        /// First time clicking will just get rid of some placeholder text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GalleryTitle_MouseDown(object sender, RoutedEventArgs e)
        {
            if (titleNotClicked)
            {
                GalleryItemTitle.Clear();
                titleNotClicked = false;
            }
        }

        /// <summary>
        /// When the gallery item is to be finalised, the title and image will be checked and then
        /// depending if the user is editing and existing item or creating a new one, it will return
        /// the new list of items to overwrite the save file with.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (!validateGalleryDetails()) return;
            
            try {
                Dictionary<int, GalleryItemData>? currentGallery = (editing) ? editExistingItem() : createNewItem();
                GalleryItemData.setCurrentGallery(currentGallery);
                this.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error occurred trying to modify this item, check if all details are correct", "File Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Creates a new GalleryItemData object
        /// </summary>
        /// <returns>The modified gallery items to be saved</returns>
        private Dictionary<int, GalleryItemData> createNewItem()
        {
            Dictionary<int, GalleryItemData>? currentGallery = GalleryItemData.getCurrentGallery();
            GalleryItemData newItem = new GalleryItemData()

            {
                ImgName = saveGalleryImage(),
                Title = GalleryItemTitle.Text,
                Desc = new TextRange(GalleryItemDesc.Document.ContentStart, GalleryItemDesc.Document.ContentEnd).Text
            };

            currentGallery[currentGallery.Count] = newItem;

            return currentGallery;
        }

        /// <summary>
        /// Modifies an existing gallery item
        /// </summary>
        /// <returns>The modified gallery items to be saved</returns>
        private Dictionary<int, GalleryItemData> editExistingItem()
        {
            Dictionary<int, GalleryItemData>? currentGallery = GalleryItemData.getCurrentGallery();
            GalleryItemData item = currentGallery[itemIndex];

            // Checks if the image is actually different and not the same
            if (item.ImgName != Path.GetFileName(tempGalleryImage) & tempGalleryImage != "")
            {
                File.Delete("../../../Images/"+item.ImgName);
                item.ImgName = saveGalleryImage();
            }

            item.Title = GalleryItemTitle.Text;
            item.Desc = new TextRange(GalleryItemDesc.Document.ContentStart, GalleryItemDesc.Document.ContentEnd).Text;

            return currentGallery;
        }

        /// <summary>
        /// Is called when a gallery item from MainWindow is selected for editing. Populates the 
        /// data in this window with the selected gallery item data.
        /// </summary>
        /// <param name="index">The ID of the gallery item in the save file</param>
        /// <param name="imgname"></param>
        /// <param name="title"></param>
        /// <param name="desc"></param>
        public void populateControlsForEditing(int index, string imgname, string title, string desc)
        {
            editing = true;
            itemIndex = index;
            GalleryItemImage.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/../../../Images/" + imgname));
            GalleryItemTitle.Text = title;
            GalleryItemDesc.Document.Blocks.Clear();
            GalleryItemDesc.AppendText(desc);
            TextStyler.applyKeyWordStyling(TextStyler.getAllKeyWords(GalleryItemDesc.Document), Brushes.DeepSkyBlue);

            titleNotClicked = false;
            descNotClicked = false;
        }

        /// <summary>
        /// Makes a copy of the image of the gallery item from 'Temp_images/' to 'Images/'
        /// </summary>
        /// <returns>The filename of the image, no path</returns>
        private string saveGalleryImage()
        {
            string fileName = Path.GetFileName(tempGalleryImage);
            File.Copy(tempGalleryImage, "../../../Images/"+fileName, true);            

            return fileName;
        }

        /// <summary>
        /// Triggered when the window is closed (also by 'this.Close()'), makes sure to always
        /// delete the unnecessary images in 'Temp_images/'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                removeTempImage();
            }
            catch (Exception)
            {
                return;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Deletes the currently selected image from 'Temp_images/'
        /// </summary>
        private void removeTempImage()
        {
            if (tempGalleryImage != "")
                File.Delete(tempGalleryImage);
        }
    }
}
