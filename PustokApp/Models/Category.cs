namespace PustokApp.Models
{
	public class Category:AuditTable
	{
        public string Name { get; set; } = null!;
        public Category? ParentId { get; set; }
        public List<Category>? ChildCategories { get; set; }
        public List<Product>? Products { get; set; }
    }
}
