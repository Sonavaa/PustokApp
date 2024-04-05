﻿namespace PustokApp.Models
{
	public class Product:AuditTable
	{
		public string Name { get; set; } = null!;
		public double ExTax { get; set; }
		public string ProductCode { get; set; } = null!;
		public int RewardPoint { get; set; }
		public bool IsAvailable { get; set; } = default!;
		public double Price { get; set; }
		public double DiscountedPrice { get; set; }
		public double Review {  get; set; }
		public string? Description { get; set; } 
		//public int Count { get; set; }
		public int StockCount { get; set; }
		//public int DiscountPercent { get; set; }
		public int CategoryId { get; set; }
		public Category? Category { get; set; }
		public List<ProductImage>? ProductImages { get; set; }
        public List<Tag>? Tags { get; set; }
    }
}
