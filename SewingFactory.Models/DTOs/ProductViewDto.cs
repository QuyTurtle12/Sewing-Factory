﻿namespace SewingFactory.Models.DTOs
{
    public class ProductViewDto
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public double? Price { get; set; }
        public bool? Status { get; set; }
    }

}