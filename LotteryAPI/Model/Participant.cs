using System.ComponentModel.DataAnnotations.Schema;

namespace LotteryAPI.Model
{
    public class Participant : User
    {
        [NotMapped]
        public List<Game> Games { get; set; }
    }
}
