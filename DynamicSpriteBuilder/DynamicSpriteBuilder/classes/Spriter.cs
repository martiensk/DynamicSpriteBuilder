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
            string name = "sh-";
            string css = "";

            int height = 0;
            int width = 0;
            int x = 0;
            int y = 0;
            bool wrapX = false;

            List<KeyValuePair<int, int>> points = new List<KeyValuePair<int, int>>(); 
            List<Image> spriteImages = new List<Image>();
            SpriteSheet sheet = new SpriteSheet();
            string[] spriteNames = new string[idList.Length];

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

            css = $".{name}{{background: url('images/sprites/{name}.png') no-repeat;}}";

            using (Bitmap bmp = new Bitmap(width, height))
            {
                //var ms = new MemoryStream();
                using (var g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.Transparent);
                    x = 0;
                    y = 0;

                    for (int i = 0; i < spriteImages.Count; i++)
                    {
                        Image img = spriteImages[i];
                        // Sprite start and end co-ordinates.
                        int s_x = x;
                        int s_y = y;
                        int e_x = x + img.Width;
                        int e_y = y + img.Height;

                        if (e_x < _maxWidth)
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
                            // Reset x position for sprite
                            s_x = 0;
                            e_x = x;
                            s_y = y;
                            e_y = y + img.Height;
                        }

                        spriteNames[i] = $"s-{idList[i]}";

                        css += $".{name}.s-{idList[i]}{{" +
                            $"background-size: {this.Calculate(width, s_x, e_x, true)}% {this.Calculate(height, s_y, e_y, true)}%;" +
                            $"background-position: {this.Calculate(width, s_x, e_x, false)}% {this.Calculate(height, s_y, e_y, false)}%;}}";

                    }
                    Directory.CreateDirectory($"{_webroot}//images//sprites//");


                    bmp.Save($"{_webroot}//images//sprites//{name}.png");
                }
            }

            sheet.Name = name;
            sheet.CSS = css;
            sheet.Sprites = spriteNames;
            sheet.Path = $"/images/sprites/{name}.png";

            return sheet;
        }

        /**
         * Method accepts three parameters:
         * dimension - the spritesheet height / width as required.
         * n1 - The smaller value of x / y, as required
         * n2 - The bigger value of x / y, as required
         * calcSize - If true will calculate size, otherwise will calculate position.
         */
        public decimal Calculate(int dimension, int n1, int n2, bool calcSize)
        {
            try
            {
                decimal result;
                if (calcSize)
                {
                    result = (decimal)dimension / (n2 - n1) * 100;
                }
                else
                {
                    result = (decimal)n1 / (dimension - (n2 - n1)) * 100;
                }
                return result < 0 || Math.Round(result) == 0 ? 0 : result;
            }
            catch
            {
                return 0;
            }
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
        public string[] Sprites { get; set; }
        public string Path { get; set; }
    }
}
