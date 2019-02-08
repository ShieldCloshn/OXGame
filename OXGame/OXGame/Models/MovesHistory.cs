using System.ComponentModel.DataAnnotations;

namespace OXGame.Models
{
    public class MovesHistory
    {
        [Key]
        public int TurnId { get; set; }
        public int GameId { get; set; }
        public string Move { get; set; }
        public string Player { get; set; }
    }
}