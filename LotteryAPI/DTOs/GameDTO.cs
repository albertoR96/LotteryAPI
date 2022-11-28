
namespace LotteryAPI.DTOs
{
    public class GameDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Begin { get; set; }
        public string End { get; set; }
        public List<ParticipantDTO> Participants { get; set; }
        public List<PrizeDTO> Prizes { get; set; }
        public List<ParticipantDTO> Winners { get; set; }
    }
}
