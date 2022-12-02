
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LotteryAPI.DTOs
{
    public class GameDTO
    {
        [Required]
        public string Name { get; set; }
        public string PlayedAt { get; set; }
        [NotMapped]
        public List<ParticipantDTO> Participants { get; set; }
        [NotMapped]
        public List<PrizeDTO> Prizes { get; set; }
        [NotMapped]
        public List<ParticipantDTO> Winners { get; set; }
    }
}
