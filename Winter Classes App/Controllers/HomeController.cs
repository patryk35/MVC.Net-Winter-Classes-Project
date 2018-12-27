using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CommunityCertForT;
using CommunityCertForT.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Winter_Classes_App.EntityFramework;
using Winter_Classes_App.Models;

namespace Winter_Classes_App.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IConfiguration Configuration, DataContext context) : base(Configuration, context) { }
        public async Task<IActionResult> Index()
        {
            PrivilegesLevel privilegesLevel = await CheckGroup();
            ViewBag.PrivilegesLevel = (int)privilegesLevel;
            return View();
        }

    }
}
