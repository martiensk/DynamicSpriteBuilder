using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using DynamicSpriteBuilder.classes;

namespace DynamicSpriteBuilder.Pages
{
    public class IndexModel : PageModel
    {
        private Spriter _sprite;

        public IndexModel(IHostingEnvironment env)
        {
            _sprite = new Spriter(env.WebRootPath);
        }

        public void OnGet()
        {
            
        }

        [HttpGet]
        public JsonResult OnGetSpriteSheet()
        {
            Random r = new Random();
            int[] idList = { r.Next(1, 30), r.Next(1, 30), r.Next(1, 30), r.Next(1, 30), r.Next(1, 30) };
            string name =_sprite.buildSpritesheet(idList);
            return new JsonResult(name);
        }
    }
}
