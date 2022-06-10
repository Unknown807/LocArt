using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Local_Gallery
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class GalleryItem : UserControl
    {
        private readonly int _ListIndex;
        private readonly string _ImgPath, _Title, _Desc;
        public int ListIndex { get { return _ListIndex; } }
        public string Title { get { return _Title; } }
        public string Desc { get { return _Desc; } }
        public string ImgPath { get { return _ImgPath; } }
        public GalleryItem(int index, string imgpath, string title, string desc)
        {
            InitializeComponent();
            _ListIndex = index;
            _Title = title;
            _Desc = desc;
            _ImgPath = imgpath;
        }
    }
}
