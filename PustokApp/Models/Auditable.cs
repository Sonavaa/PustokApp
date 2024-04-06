namespace PustokApp.Models
{
    public class AuditTable:Base
    {
        public string CreatedBy { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = default!;
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; } 
        public bool IsIdentity { get; set; }
    }
}
