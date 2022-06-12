using Local_Gallery;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Utilities
{
    class GalleryItemData
    {
        public string ImgName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Desc { get; set; } = string.Empty;

        public static Dictionary<int, GalleryItemData>? getCurrentGallery()
        {
            string jsonString = File.ReadAllText("../../../GallerySave.json");
            return JsonSerializer.Deserialize<Dictionary<int, GalleryItemData>>(jsonString);
        }

        public static void setCurrentGallery(Dictionary<int, GalleryItemData> data)
        {
            string jsonString = JsonSerializer.Serialize(data);
            File.WriteAllText("../../../GallerySave.json", jsonString);
        }

        public static Dictionary<int, GalleryItemData>? searchGallery(string text)
        {
            Dictionary<int, GalleryItemData> newItems = new Dictionary<int, GalleryItemData>();

            // Search by keywords
            if (text.Contains("#"))
            {
                searchByKeyWords(ref newItems, text);
            } else // Search by title
            {
                searchByTitle(ref newItems, text);
            }

            return newItems;
        }

        private static void searchByKeyWords(ref Dictionary<int, GalleryItemData>? newItems, string text)
        {
            Dictionary<int, GalleryItemData>? currentItems = getCurrentGallery();
            MatchCollection matches = Regex.Matches(text, @"#[A-Za-z0-9]+");
            foreach (GalleryItemData item in currentItems.Values)
            {
                bool flag = true;
                foreach (Match match in matches)
                {
                    flag &= item.Desc.Contains(match.Value);
                    if (!flag) break;
                }
                
                if (flag) newItems[newItems.Count] = item;
            }
        }

        private static void searchByTitle(ref Dictionary<int, GalleryItemData>? newItems, string text)
        {
            Dictionary<int, GalleryItemData>? currentItems = getCurrentGallery();
            foreach (GalleryItemData item in currentItems.Values)
            {
                if (item.Title.ToLower().Contains(text.ToLower())) newItems[newItems.Count] = item;
            }
        }

        public static Dictionary<int, GalleryItemData>? removeGalleryItems(ObservableCollection<GalleryItem> items)
        {
            Dictionary<int, GalleryItemData>? currentItems = getCurrentGallery();

            foreach (GalleryItem item in items)
            {
                if (item.GetRemoveToggle())
                {
                    if (!currentItems.Remove(item.DictIndex)) throw new ArgumentException("Gallery item to remove doesn't exist");
                }
            }

            setCurrentGallery(currentItems);
            return currentItems;
        }
    }
}
