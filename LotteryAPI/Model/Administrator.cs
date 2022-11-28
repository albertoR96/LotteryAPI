namespace LotteryAPI.Model
{
    public class Administrator : User
    {
        public List<Game> Games { get; set; }
    }
}
