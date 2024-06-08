using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dms.Models
{
    [Table("chatmessages")]
    public class ChatMessage
    {
        [Key]
        public int Id { get; set; }

        [Column("sender_id")]
        public int SenderId { get; set; }

        [Column("receiver_id")]
        public int ReceiverId { get; set; }

        [Column("message")]
        public string Message { get; set; }

        [Column("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
