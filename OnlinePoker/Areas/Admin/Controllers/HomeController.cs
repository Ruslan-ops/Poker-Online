using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlinePoker.Models;
using OnlinePoker.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePoker.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly PokerBusinessLogic _pokerLogic;
        private readonly AppDbContext _appDbContext;
        public HomeController(PokerBusinessLogic pokerLogic, AppDbContext appDbContext)
        {
            _pokerLogic = pokerLogic;
            _appDbContext = appDbContext;
        }

        public async Task<IActionResult> Index()
        {
            List<TableModel> tables = await _pokerLogic.GetTablesAsync(_appDbContext);
            return View(tables);
        }
    }
}
