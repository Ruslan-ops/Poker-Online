using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePoker.Models
{
    public class IdentityPokerUser : IdentityUser
    {
        public List<Player> Players { get; set; }
    }
}
