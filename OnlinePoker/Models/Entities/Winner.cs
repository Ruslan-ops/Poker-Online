using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePoker.Models
{
    public class Winner : EntityBase
    {
        [Required]
        public Guid PlayerId { get; set; }
        [Required]
        public Player Player { get; set; }

        [Required]
        public Guid DealId { get; set; }
        [Required]
        public Deal Deal { get; set; }

        [Required]
        public int WonPotSize { get; set; }

        [Required]
        [MaxLength(255)]
        public string Combination { get; set; }
        [Required]
        [MaxLength(127)]
        public string Hand { get; set; }
        
    }
}
