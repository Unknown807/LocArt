﻿using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Local_Gallery
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class GalleryItem : UserControl
    {
        private readonly int _ListIndex;
        private readonly string _ImgName, _Title, _Desc;
        public int ListIndex { get { return _ListIndex; } }
        public string Title { get { return _Title; } }
        public string Desc { get { return _Desc; } }
        public string ImgName { get { return _ImgName; } }
        public GalleryItem(int index, string imgname, string title, string desc)
        {
            InitializeComponent();
            _ListIndex = index;
            _Title = title;
            _Desc = desc;
            _ImgName = imgname;

            GalleryItemTitle.Content = title;
            GalleryItemImage.Source = new BitmapImage(new Uri("pack://application:,,,/Images/" + imgname));
        }
    }
}
