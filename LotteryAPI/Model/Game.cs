namespace LotteryAPI.Model
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Begin { get; set; }
        public string End { get; set; }
        public List<Participant> Participants { get; set; }
        public List<Prize> Prizes{ get; set; }
        public List<Participant> Winners { get; set; }
    }
}
