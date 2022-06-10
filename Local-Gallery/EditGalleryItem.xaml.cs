using Microsoft.Win32;
using System;
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
            string fileDest = "../../../Temp_images/"+Randomiser.generateRandAlphaNumStr()+"_"+System.IO.Path.GetFileName(fileSource);

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
