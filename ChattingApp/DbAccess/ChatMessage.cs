using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ChattingApp.DbAccess
{
    public class ChatMessage
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        [MaxLength(250)]
        public string MessageText { get; set; }
        public DateTime SentAt { get; set; }

        [ForeignKey("UserId")]
        public IdentityUser User { get; set; }
    }
}
