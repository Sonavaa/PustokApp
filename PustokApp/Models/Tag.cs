namespace PustokApp.Models
{
    public class Tag:AuditTable
    {
        public string Name { get; set; } = null!;
        public List<Product>? Products { get; set; }
    }
}
