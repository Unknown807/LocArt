using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Utilities;

namespace Local_Gallery
{
    public partial class EditGalleryItem : Window
    {
        private int itemIndex = -1;
        private bool editing = false;
       
        private string tempGalleryImage = "";
        public EditGalleryItem()
        {
            InitializeComponent();
            GalleryItemDesc.Document.Blocks.Clear();
            GalleryItemDesc.Document.Blocks.Add(new Paragraph(new Run("Enter description here")));
        }

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

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {

            //TODO: veryfiy title, desc and that tempGalleryImage is not ""
            // to be done in seperate method(s)

            try {
                List<GalleryItemData>? currentGallery = (editing) ? editExistingItem() : createNewItem();
                GalleryItemData.setCurrentGallery(currentGallery);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred trying to modify this item, check if all details are correct", "File Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private List<GalleryItemData> createNewItem()
        {
            List<GalleryItemData>? currentGallery = GalleryItemData.getCurrentGallery();
            GalleryItemData newItem = new GalleryItemData()

            {
                ImgName = saveGalleryImage(),
                Title = GalleryItemTitle.Text,
                Desc = new TextRange(GalleryItemDesc.Document.ContentStart, GalleryItemDesc.Document.ContentEnd).Text
            };

            currentGallery.Add(newItem);

            return currentGallery;
        }

        private List<GalleryItemData> editExistingItem()
        {
            List<GalleryItemData>? currentGallery = GalleryItemData.getCurrentGallery();
            GalleryItemData item = currentGallery[itemIndex];

            if (item.ImgName != Path.GetFileName(tempGalleryImage) & tempGalleryImage != "")
            {
                File.Delete("../../../Images/"+item.ImgName);
                item.ImgName = saveGalleryImage();
            }

            item.Title = GalleryItemTitle.Text;
            item.Desc = new TextRange(GalleryItemDesc.Document.ContentStart, GalleryItemDesc.Document.ContentEnd).Text;

            return currentGallery;
        }

        public void populateControlsForEditing(int index, string imgname, string title, string desc)
        {
            editing = true;
            itemIndex = index;
            GalleryItemImage.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/../../../Images/" + imgname));
            GalleryItemTitle.Text = title;
            GalleryItemDesc.Document.Blocks.Clear();
            GalleryItemDesc.Document.Blocks.Add(new Paragraph(new Run(desc)));
        }

        private string saveGalleryImage()
        {
            string fileName = Path.GetFileName(tempGalleryImage);
            File.Copy(tempGalleryImage, "../../../Images/"+fileName, true);            

            return fileName;
        }

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
        private void removeTempImage()
        {
            if (tempGalleryImage != "")
                File.Delete(tempGalleryImage);
        }
    }
}
