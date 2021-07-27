using Microsoft.AspNetCore.Mvc;
using OnlinePoker.Models;
using OnlinePoker.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePoker.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TablesEditController : Controller
    {
        private readonly PokerBusinessLogic _pokerLogic;
        private readonly AppDbContext _appDbContext;

        public TablesEditController(PokerBusinessLogic pokerLogic, AppDbContext appDbContext)
        {
            _pokerLogic = pokerLogic;
            _appDbContext = appDbContext;
        }


        public IActionResult Create()
        {
            return View(new TableModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(TableModel model)
        {
            if(ModelState.IsValid)
            {
                await _pokerLogic.CreateTableAsync(model, _appDbContext);
                RedirectToAction(nameof(HomeController.Index), nameof(HomeController)); //нужно nameof(HomeController).CutController();
            }
            return View(model);
        }

        /*public async Task<IActionResult> Delete()
        {

        }*/
    }
}
