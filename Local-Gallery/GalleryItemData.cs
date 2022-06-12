using Local_Gallery;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Utilities
{
    /// <summary>
    /// Encapsulates each gallery item in the saved json data, with some additional methods
    /// relevant to the data.
    /// </summary>
    class GalleryItemData
    {
        public string ImgName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Desc { get; set; } = string.Empty;

        /// <returns>The saved gallery items</returns>
        public static Dictionary<int, GalleryItemData>? getCurrentGallery()
        {
            string jsonString = File.ReadAllText("../../../GallerySave.json");
            return JsonSerializer.Deserialize<Dictionary<int, GalleryItemData>>(jsonString);
        }

        /// <summary>
        /// Overwrites the saved data with new data
        /// </summary>
        /// <param name="data"></param>
        public static void setCurrentGallery(Dictionary<int, GalleryItemData> data)
        {
            string jsonString = JsonSerializer.Serialize(data);
            File.WriteAllText("../../../GallerySave.json", jsonString);
        }

        /// <summary>
        /// Takes a string and either searches by keywords (if there are any) or by gallery item titles
        /// </summary>
        /// <param name="text">search query</param>
        /// <returns>The resulting items, used in MainWindow to update the view</returns>
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

        /// <param name="newItems">A pointer to the dictionary for search results</param>
        /// <param name="text">for pattern matching</param>
        private static void searchByKeyWords(ref Dictionary<int, GalleryItemData>? newItems, string text)
        {
            Dictionary<int, GalleryItemData>? currentItems = getCurrentGallery();
            MatchCollection matches = Regex.Matches(text, @"#[A-Za-z0-9]+");
            foreach (GalleryItemData item in currentItems.Values)
            {
                // Check that each current gallery item contains every specified keyword
                bool flag = true;
                foreach (Match match in matches)
                {
                    flag &= item.Desc.Contains(match.Value);
                    if (!flag) break;
                }
                
                if (flag) newItems[newItems.Count] = item;
            }
        }

        /// <param name="newItems">A pointer to the dictionary for search results</param>
        /// <param name="text">To check if its a substring of any gallery item title</param>
        private static void searchByTitle(ref Dictionary<int, GalleryItemData>? newItems, string text)
        {
            Dictionary<int, GalleryItemData>? currentItems = getCurrentGallery();
            foreach (GalleryItemData item in currentItems.Values)
            {
                if (item.Title.ToLower().Contains(text.ToLower())) newItems[newItems.Count] = item;
            }
        }

        /// <summary>
        /// Removes items from the current gallery and then overwrites the save file with those unremoved items
        /// </summary>
        /// <param name="items"></param>
        /// <returns>The items that were not removed, used in MainWindow to update the view</returns>
        /// <exception cref="ArgumentException">If for any reason it can't find an item, then something is wrong</exception>
        public static Dictionary<int, GalleryItemData>? removeGalleryItems(ObservableCollection<GalleryItem> items)
        {
            Dictionary<int, GalleryItemData>? currentItems = getCurrentGallery();

            foreach (GalleryItem item in items)
            {
                if (item.GetRemoveToggle())
                {
                    File.Delete("../../../Images/" + item.ImgName); // Remove image as it is redundant now
                    if (!currentItems.Remove(item.DictIndex)) throw new ArgumentException("Gallery item to remove doesn't exist");
                }
            }

            setCurrentGallery(currentItems);
            return currentItems;
        }
    }
}
