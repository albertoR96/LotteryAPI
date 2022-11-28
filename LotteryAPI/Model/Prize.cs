namespace LotteryAPI.Model
{
    public class Prize
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Participant Winner { get; set; }
    }
}
