
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DB
{
    [Table("messages")]
    public class Message
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("text")]
        public string Text { get; set; }
        
        [Column("created_time")]
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("chat_id")]
        public int ChatId { get; set; }
    }
}
