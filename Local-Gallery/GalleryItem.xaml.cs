using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Local_Gallery
{
    /// <summary>
    /// The custom control for gallery items in the MainWindow
    /// </summary>
    public partial class GalleryItem : UserControl
    {
        private readonly int _DictIndex;
        private readonly string _ImgName, _Title, _Desc;
        public int DictIndex { get { return _DictIndex; } }
        public string Title { get { return _Title; } }
        public string Desc { get { return _Desc; } }
        public string ImgName { get { return _ImgName; } }

        private bool RemoveToggle = false;

        public bool GetRemoveToggle()
        {
            return RemoveToggle;
        }

        /// <summary>
        /// When the user right clicks gallery items, their background and a boolean is
        /// toggled for removal
        /// </summary>
        /// <param name="value"></param>
        public void SetRemoveToggle(bool value)
        {
            RemoveToggle = value;
            Color color = (RemoveToggle) ? Colors.LightBlue : Colors.White;
            GalleryItemGrid.Background = new SolidColorBrush(color);
        }

        /// <summary>
        /// The constructor for a gallery item
        /// </summary>
        /// <param name="index">The ID of the gallery item in the save file</param>
        /// <param name="imgname"></param>
        /// <param name="title"></param>
        /// <param name="desc"></param>
        public GalleryItem(int index, string imgname, string title, string desc)
        {
            InitializeComponent();
            _DictIndex = index;
            _Title = title;
            _Desc = desc;
            _ImgName = imgname;

            GalleryItemTitle.Content = title;

            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad; //So that an image can be replaced when editing, even while its being used elsewhere
            image.UriSource = new Uri(Directory.GetCurrentDirectory() + "/../../../Images/" + imgname);
            image.EndInit();

            GalleryItemImage.Source = image;
        }
    }
}
