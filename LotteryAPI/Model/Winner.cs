namespace LotteryAPI.Model
{
    public class Winner
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; }
        public int ParticipantId { get; set; }
        public Participant Participant { get; set; }
        public int PrizeId { get; set; }
        public Prize Prize { get; set; }
    }
}
