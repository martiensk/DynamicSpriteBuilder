using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace DynamicSpriteBuilder.classes
{
    // This class is responsible for all sprite tool handlings.
    public class Spriter
    {
        private string _webroot;
        private List<Sprite> _spriteList;

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

        public string buildSpritesheet(int[] idList)
        {
            string name = "";
            foreach(int id in idList)
            {
                name += id;
                // imagesharp //
                // CoreCompat.System.Drawing
            }
            return name;
            // Add some code.
        }

    }

    // This class represents a single image.
    public class Sprite
    {
        public int ID { get; set; }
        public string ClassName { get; set; }
        public string Image { get; set; }
    }
}
