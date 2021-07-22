using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePoker.Models
{
    public class Table : EntityBase
    {
        [Required (ErrorMessage ="Не указано максимальное число игроков")]
        [Display(Name ="Максимальное число игроков")]
        [Range(2, 15, ErrorMessage = "Невозможно создать стол с таким числом игроков")]
        public int MaxPlayersAmount { get; set; }

        [Required]
        [MaxLength(30)]
        public string Status { get; set; }

        [Required(ErrorMessage = "Не указан минимальный стартовый стек")]
        [Display(Name = "Минимальный стартовый стек")]
        public int MinStartStack { get; set; }

        [Required(ErrorMessage = "Не указан максимальный стартовый стек")]
        [Display(Name = "Максимальный стартовый стек")]
        public int MaxStartStack { get; set; }

        [Required]
        public int BigBlindSize { get; set; }

        [Required(ErrorMessage = "Не указан размер блайндов")]
        [Display(Name = "Pазмер блайндов")]
        public int SmallBlindSize { get; set; }

        [Required]
        public IEnumerable<Seat> Seats { get; set; }
        public IEnumerable<Player> Players { get; set; }
        public IEnumerable<Deal> Deals { get; set; }
    }
}
