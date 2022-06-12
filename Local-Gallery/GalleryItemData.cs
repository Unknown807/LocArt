using System.Collections.Generic;
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

        public static List<GalleryItemData>? getCurrentGallery()
        {
            string jsonString = File.ReadAllText("../../../GallerySave.json");
            return JsonSerializer.Deserialize<List<GalleryItemData>>(jsonString);
        }

        public static void setCurrentGallery(List<GalleryItemData> data)
        {
            string jsonString = JsonSerializer.Serialize(data);
            File.WriteAllText("../../../GallerySave.json", jsonString);
        }

        public static List<GalleryItemData>? searchGallery(string text)
        {
            List<GalleryItemData> newItems = new List<GalleryItemData>();

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

        private static void searchByKeyWords(ref List<GalleryItemData>? newItems, string text)
        {
            List<GalleryItemData>? currentItems = getCurrentGallery();
            MatchCollection matches = Regex.Matches(text, @"#[A-Za-z0-9]+");
            foreach (GalleryItemData item in currentItems)
            {
                bool flag = true;
                foreach (Match match in matches)
                {
                    flag &= item.Desc.Contains(match.Value);
                    if (!flag) break;
                }

                if (flag) newItems.Add(item);
            }
        }

        private static void searchByTitle(ref List<GalleryItemData>? newItems, string text)
        {
            List<GalleryItemData>? currentItems = getCurrentGallery();
            foreach (GalleryItemData item in currentItems)
            {
                if (item.Title.ToLower().Contains(text.ToLower())) newItems.Add(item);
            }
        }

    }
}
