namespace LotteryAPI.Model
{
    public class Participant : User
    {
        public List<Game> Games { get; set; }
    }
}
