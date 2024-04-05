namespace PustokApp.Models
{
	public class ProductImage:Base
	{
        public string Url { get; set; } = null!;
		public bool IsMain { get; set; }
		public int ProductId { get; set; }
		public Product? Product { get; set;}
    }
}
