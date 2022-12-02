namespace LotteryAPI.Model
{
    public class GamePrize
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; }
        public int PrizeId { get; set; }
        public Prize Prize { get; set; }
    }
}
