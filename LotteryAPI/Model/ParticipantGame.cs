namespace LotteryAPI.Model
{
    public class ParticipantGame
    {
        public int Id { get; set; }
        public int ParticipantId { get; set; }
        public int GameId { get; set; }
        public int GameNumber { get; set; }
        public Participant Participant { get; set; }
        public Game Game { get; set; }
    }
}
