using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Cultivatrade.Api.Services
{
    public class Base64Resizer
    {
        public string ResizeImage(string imagePath)
        {
            if (!File.Exists(imagePath)) return null;

            using (var image = Image.Load(imagePath))
            {
                image.Mutate(x => x.Resize(200, 200));
                using (var ms = new MemoryStream())
                {
                    image.SaveAsPng(ms);
                    var base64String = Convert.ToBase64String(ms.ToArray());
                    return $"data:image/png;base64,{base64String}";
                }
            }
        }
    }
}
