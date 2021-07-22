using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePoker.Models
{
    public class Player : EntityBase
    {
        [Required]
        public Guid SeatId { get; set; }

        [Required]
        public Seat Seat { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public IdentityPokerUser User { get; set; }

        [Required]
        public Guid TableId { get; set; }
        
        [Required]
        public Table Table { get; set; }

        public IEnumerable<Winner> Winners { get; set; }
    }
}
