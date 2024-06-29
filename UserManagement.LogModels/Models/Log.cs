using System;

namespace UserManagement.Models
{
    public class Log
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string? Title { get; set; } = string.Empty;
        public string LogType { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
