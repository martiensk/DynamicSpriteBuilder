using System;
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
        private IHostingEnvironment _env;

        public Spriter(IHostingEnvironment env)
        {
            // Getting the public folder path
            _env = env;
            Webroot = _env.WebRootPath;

            // Building sprite list from file - it would ideally come from a DB, perhaps via  arepository class.
        }

        public string Webroot { get; set; }
        public List<Sprite> SpriteList { get; set; }
    }

    // This class represents a single image.
    public class Sprite
    {
        public int ID { get; set; }
        public string ClassName { get; set; }
        public string Image { get; set; }
    }
}
