using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class AuditLog
    {
        public Guid Id { get; set; }
        
        [Required, MaxLength(100)]
        public string Action { get; set; } = "";
        
        [Required, MaxLength(100)]
        public string EntityName { get; set; } = "";
        
        [Required, MaxLength(100)]
        public string EntityId { get; set; } = "";
        
        public Guid? UserId { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        [MaxLength(500)]
        public string? AdditionalInfo { get; set; }
    }
}
