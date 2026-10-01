using System;

namespace ShopTARpe25.Core.Domain
{
    public class RealEstate
    {
        public Guid Id { get; set; }
        public string Address { get; set; }
        public double SizeSqm { get; set; }
        public int Rooms { get; set; }
        public int Price { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
