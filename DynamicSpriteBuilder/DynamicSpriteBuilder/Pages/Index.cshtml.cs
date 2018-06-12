using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DynamicSpriteBuilder.Pages
{
    public class IndexModel : PageModel
    {
        public string Message { get; private set; } = "It works!";

        public void OnGet()
        {

        }
    }
}
