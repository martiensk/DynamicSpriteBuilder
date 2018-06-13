using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace DynamicSpriteBuilder.classes
{
    // This class is responsible for all sprite tool handlings.
    // If this method doesn't work., try // imagesharp //
    public class Spriter
    {
        private string _webroot;
        private List<Sprite> _spriteList;
        private int _maxWidth = 160; // Forcing images to wrap for testing purposes. Should be 1024.
        private int _maxHeight = 1024;

        public Spriter(string webroot)
        {
            // Getting the public folder path
            _webroot = webroot;

            // Building sprite list from file - it would ideally come from a DB, perhaps via a repository.
            _spriteList = new List<Sprite>();
            string[] sprites = Directory.GetFiles(_webroot + "\\images");
            foreach(string sprite in sprites)
            {
                string image = sprite.Replace(_webroot + "\\images\\", "");
                _spriteList.Add(new Sprite
                {
                    ID = Convert.ToInt32(image.Replace("isometric_pixel_", "").Replace(".png", "")),
                    ClassName = image.Replace(".png", "").Replace("_", ""),
                    Image = image
                });
            }
        }

        public SpriteSheet buildSpritesheet(int[] idList)
        {
            string name = "sh_";
            string css = "";

            int height = 0;
            int width = 0;
            int x = 0;
            int y = 0;
            bool wrapX = false;

            List<KeyValuePair<int, int>> points = new List<KeyValuePair<int, int>>(); 
            List<Image> spriteImages = new List<Image>();
            SpriteSheet sheet = new SpriteSheet();

            if(idList.Length > 20)
            {
                throw new Exception("Must be 20 sprites or less!");
            }

            foreach(int id in idList)
            {
                name += id;
                string path = $"{_webroot}//images//{this._spriteList.Where(z => z.ID == id).First().Image}";
                Image img = Image.FromFile(path);
                spriteImages.Add(img);

                points.Add(new KeyValuePair<int, int>(x, y));
                if (!wrapX)
                {
                    width = width + img.Width < _maxWidth ? width + img.Width : width;
                    x += img.Width;
                    wrapX = x + img.Width > _maxWidth;
                    if (height == 0)
                    {
                        height = img.Height;
                    }
                }
                else if (height + img.Height < _maxHeight)
                {
                    wrapX = false;
                    x = 0;
                    y = height;
                    height += img.Height;
                }
                else
                {
                    throw new Exception("Sprite image is exceeding max dimensions");
                }
            }

            using (Bitmap bmp = new Bitmap(width, height))
            {
                //var ms = new MemoryStream();
                using (var g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.Transparent);
                    x = 0;
                    y = 0;
                    foreach (Image img in spriteImages)
                    {
                        if (x + img.Width < _maxWidth)
                        {
                            g.DrawImage(img, new Point(x, y));
                            x += img.Width;
                        }
                        else
                        {
                            y += img.Height;
                            x = 0;
                            g.DrawImage(img, new Point(x, y));
                            x += img.Width;
                        }
                    }
                    Directory.CreateDirectory($"{_webroot}//images//sprites//");

                    css = $"{name}{{background: url('images/sprites {name}.png}} no-repeat;}}";

                    bmp.Save($"{_webroot}//images//sprites//{name}.png");
                }
            }

            sheet.Name = name;
            sheet.CSS = css;

            return sheet;
        }

    }

    // This class represents a single image.
    public class Sprite
    {
        public int ID { get; set; }
        public string ClassName { get; set; }
        public string Image { get; set; }
    }

    [Serializable]
    public class SpriteSheet
    {
        public string Name { get; set; }
        public string CSS { get; set; }
        public List<string> Sprites { get; set; }
    }
}
