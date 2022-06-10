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

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(fileSource);
            bitmap.EndInit();
            GalleryItemImage.Source = bitmap;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            List<GalleryItemData>? currentGallery =  GalleryItemData.getCurrentGallery();
            if (currentGallery == null)
            {
                MessageBox.Show("Error encountered reading GallerySave.json", "File Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //TODO: veryfiy title, desc and that tempGalleryImage is not ""
            // to be done in seperate method(s)

            try
            {
                GalleryItemData newItem = new GalleryItemData()
                {
                    Img_path = saveGalleryImage(),
                    Title = GalleryItemTitle.Text,
                    Desc = new TextRange(GalleryItemDesc.Document.ContentStart, GalleryItemDesc.Document.ContentEnd).Text
                };
                    
                currentGallery.Add(newItem);

                GalleryItemData.setCurrentGallery(currentGallery);

                this.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error occurred trying to create this item, check if all details are correct", "File Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string saveGalleryImage()
        {
            string newPath = "../../../Images/" + Path.GetFileName(tempGalleryImage);
            File.Copy(tempGalleryImage, newPath, true);            

            return newPath;
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
