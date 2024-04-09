using System.ComponentModel.DataAnnotations.Schema;

namespace PustokApp.Models
{
	public class Category:AuditTable
	{
        public string Name { get; set; } = null!;
        public int? ParentId { get; set; }
        public Category? Parent { get; set; }
        public List<Category> ChildCategories { get; set; }=new List<Category>();
        [NotMapped]
        public IFormFile File { get; set; } 
        public List<Product> Products { get; set; }=new List<Product>();
     
    }
}
