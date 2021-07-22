using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePoker.Models
{
    public class Deal : EntityBase
    {
        [Required]
        public int PotSize { get; set; }

        [Required]
        public Guid TableId { get; set; }

        [Required]
        public Table Table { get; set; }
        
        public bool? FinishedWithShowDown { get; set; }
        public DateTime? FinishedAt { get; set; }

        [Required]
        [MaxLength(30)]
        public string LastRound { get; set; }
        
        [Required]
        public bool IsFinished { get; set; }

        [Required]
        public IEnumerable<Player> Players { get; set; }

        public IEnumerable<Winner> Winners { get; set; }

        protected Deal()
        {
            PotSize = 0;
            LastRound = "Preflop";
            IsFinished = false;
        }


    }
}
