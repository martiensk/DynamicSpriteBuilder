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
            int spriteCount = r.Next(1, 10);
            int[] idList = new int[spriteCount];
            for (int i = 0; i < spriteCount; i++)
            {
                idList[i] = r.Next(1, 30);
            }
            SpriteSheet image =_sprite.buildSpritesheet(idList);
            return new JsonResult(image);
        }
    }
}
