using FootballPlayers.Models;
using System.ComponentModel.DataAnnotations;

namespace FootballPlayers.Models
{
    public class Team
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Player> Players { get; set; }
    }
}  