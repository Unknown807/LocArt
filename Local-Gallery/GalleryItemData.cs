using System.Collections.Generic;
using System.IO;
using System.Text.Json;

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

    }
}
