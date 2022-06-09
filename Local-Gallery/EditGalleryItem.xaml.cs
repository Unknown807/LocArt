using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Local_Gallery
{
    public partial class EditGalleryItem : Window
    {
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
            string fileDest = "../../../Temp_images/"+System.IO.Path.GetFileName(fileSource);

            GalleryItemDesc.Document.Blocks.Clear();
            GalleryItemDesc.Document.Blocks.Add(new Paragraph(new Run("")));

            if (!fileDest.Contains(".jpg") & !fileDest.Contains(".png"))
            {
                MessageBox.Show("Incorrect file extension, file must be jpg or png", "File Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            File.Copy(fileSource, fileDest, true);

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(fileSource);
            bitmap.EndInit();
            GalleryItemImage.Source = bitmap;
        }
    }
}
