using System.ComponentModel.DataAnnotations.Schema;

namespace LotteryAPI.Model
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime PlayedAt { get; set; }
        public int OwnerId { get; set; }
        public Administrator Owner { get; set; }
        public string GameStatus { get; set; }
        [NotMapped]
        public List<Participant> Participants { get; set; }
        [NotMapped]
        public List<Prize> Prizes{ get; set; }
        [NotMapped]
        public List<Participant> Winners { get; set; }
    }
}
