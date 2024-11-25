using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

    namespace FootballPlayers.Models
    {
        public class Player
        {
            public int Id { get; set; }

            [Required]
            public string FirstName { get; set; }

            [Required]
            public string LastName { get; set; }

            [Required]
            public string Gender { get; set; }

            [Required]
            public DateTime BirthDate { get; set; }

            [Required]
            public string Country { get; set; }

            public int? TeamId { get; set; }
            public Team Team { get; set; }
        }
    }