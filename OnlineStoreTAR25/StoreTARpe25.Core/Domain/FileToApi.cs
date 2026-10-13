using System;

namespace ShopTARpe25.Core.Domain
{
    public class FileToApi
    {
        public Guid Id { get; set; }
        public string FileUrl { get; set; }
        public Guid? RealEstateId { get; set; }
    }
}
