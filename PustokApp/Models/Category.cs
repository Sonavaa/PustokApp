using System.ComponentModel.DataAnnotations.Schema;

namespace PustokApp.Models
{
	public class Category:AuditTable
	{
        public string Name { get; set; } = null!;
        public Category? ParentId { get; set; }
        public List<Category>? ChildCategories { get; set; }
        [NotMapped]
        public IFormFile File { get; set; } 
        public List<Product>? Products { get; set; }
    }
}
