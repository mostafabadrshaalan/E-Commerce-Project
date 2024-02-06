﻿using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class BasketItemDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [Range(1, int.MaxValue,ErrorMessage = "Quentity must be at least 1")]
        public int Quentity { get; set; }
        [Required]
        [Range(0.1, double.MaxValue, ErrorMessage = "Price must be at least than zero")]
        public decimal Price { get; set; }
        [Required]
        public string PictureUrl { get; set; }
        [Required]
        public string Product { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Category { get; set; }
    }
}