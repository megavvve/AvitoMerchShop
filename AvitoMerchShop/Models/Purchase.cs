﻿namespace AvitoMerchShop.Models
{
    public class Purchase
    {
        public int Id { get; set; }  
        public int UserId { get; set; }
        public int ItemId { get; set; } 
        public int Quantity { get; set; }  
    }
}
