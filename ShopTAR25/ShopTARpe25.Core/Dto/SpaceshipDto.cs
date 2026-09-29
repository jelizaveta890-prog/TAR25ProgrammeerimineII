using Microsoft.AspNetCore.Http;

namespace ShopTARpe25.Core.Dto
{
    //Dto slass vahendab andmeid controlleri  ja servise klassi vahel  vahel.
    public class SpaceshipDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Classification { get; set; } = string.Empty;
        public DateTime? Builddate { get; set; }
        public int? Crew { get; set; }
        public int? EnginePower { get; set; }

        public List<IFormFile> Files { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

    }
}
