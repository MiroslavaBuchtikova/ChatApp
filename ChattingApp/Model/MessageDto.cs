using System.ComponentModel.DataAnnotations;

namespace ChattingApp.Model
{
    public class MessageDto
    {
        [Required]
        public string User { get; set; }
        [MaxLength(250)]
        public string MsgText { get; set; }
    }
}
