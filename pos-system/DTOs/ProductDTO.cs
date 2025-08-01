﻿using pos_system.Helpers;
using System.ComponentModel.DataAnnotations;

namespace pos_system.DTOs
{
    public class ProductDTO
    {
        public string? ProductId { get; set; } = Unique.ID();

        public string CategoryId { get; set; } = null!;

        public string? ProductCode { get; set; } = null!;

        [Required(ErrorMessage = "Product name is required")]
        public string ProductName { get; set; } = null!;

        public string? ProductDescription { get; set; }

        public double? ProductStock { get; set; } = 0;

        public bool IsLimitedStock { get; set; } = false;

        public bool IsAvailable { get; set; } = true;

        public double? Price { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public string? ProductImage { get; set; }
        public string? ImageType { get; set; }
        public bool IsRecommended { get; set; } = false;
        public ProductCategoryDTO Category { get; set; } = new ProductCategoryDTO();
        public List<ProductVariantDTO> ProductVariants { get; set; } = [];
    }
}
