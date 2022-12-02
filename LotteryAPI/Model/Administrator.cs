using System.ComponentModel.DataAnnotations.Schema;

namespace LotteryAPI.Model
{
    public class Administrator : User
    {
        [NotMapped]
        public List<Game> Games { get; set; }
    }
}
