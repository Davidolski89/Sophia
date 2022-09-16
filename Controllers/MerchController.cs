using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sophia.Models.DataBase;
using Microsoft.AspNetCore.Identity;

namespace Sophia.Controllers
{
    // Habe ich dummy mässig erstellt falls du Videos etc verkaufen wollen würdest
    // Hier passiert noch nicht wirklich was
    public class MerchController : Controller
    {
        private readonly SophiaDB _sophiaDB;
        private readonly UserManager<SophiaUser> _userManager;
        public MerchController(SophiaDB db ,UserManager<SophiaUser> man)
        {
            _userManager = man;
            _sophiaDB = db;
            
        }
        [HttpGet]
        public IActionResult Index()
        {
            SophiaUser user = new SophiaUser();
            
            return View(_sophiaDB.Merch.ToList());
        }
        [HttpGet]
        public IActionResult Goodie(int id)
        {
            var merch = _sophiaDB.Merch.Where(x => x.Id == id);
            if (merch != null)
                return View();
            else
                return View("NotFound");
        }
        [HttpGet]
        public async Task<IActionResult> BuyGoodie(int id)
        {
            var goodie = _sophiaDB.Merch.Where(c => c.Id == id).FirstOrDefault();
            SophiaUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            SophiaUser dbuser = _sophiaDB.SophiaUsers.Where(u => u.Id == currentUser.Id).FirstOrDefault();
            if (dbuser.Merch.Contains(goodie))
                return View("Index");
            dbuser.Merch.Add(goodie);
            await _sophiaDB.SaveChangesAsync();
            
            return View();
        }
    }
}
