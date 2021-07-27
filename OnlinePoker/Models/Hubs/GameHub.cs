using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePoker.Models.Hubs
{
    public class GameHub : Hub
    {
        private readonly PokerBusinessLogic _pokerLogic;
        private readonly AppDbContext _appDbContext;
        public GameHub(PokerBusinessLogic pokerLogic, AppDbContext appDbContext)
        {
            _pokerLogic = pokerLogic;
            _appDbContext = appDbContext;
        }

        [Authorize]
        public async Task SitAtRandomPlace(string tableId)
        {
            var user = Context.User;
            await _pokerLogic.SitPlayerAtTableAsync(tableId, user.Identity.Name, _appDbContext);

            
        }
    }
}
