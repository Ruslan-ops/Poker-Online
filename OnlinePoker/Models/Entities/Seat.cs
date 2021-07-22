using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePoker.Models
{
    public class Seat : EntityBase
    {
        [Required]
        public int Number { get; set; }

        [Required]
        public Guid TableId { get; set; }

        [Required]
        public Table Table { get; set; }

        public Player Player { get; set; }
    }
}
