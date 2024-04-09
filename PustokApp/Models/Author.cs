namespace PustokApp.Models
{
    public class Author:AuditTable
    {
        public string Name { get; set; } = null!;
        public List<Product> Products { get; set; }=new List<Product>();
    }
}
