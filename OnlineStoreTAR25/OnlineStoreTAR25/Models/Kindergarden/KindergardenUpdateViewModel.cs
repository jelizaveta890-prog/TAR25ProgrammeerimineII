using System.Collections.Generic; 
using System.IO;

namespace ShopTARpe25.Models.Spaceship
{
    public class KindergardenUpdateViewModel
    {
        public System.Guid Id { get; set; }
        public string GroupName { get; set; }
        public int? ChildrenCount { get; set; }
        public string KindergartenName { get; set; } = string.Empty;
        public string TeacherName { get; set; } = string.Empty;
        public System.DateTime? CreatedAt { get; set; }
        public System.DateTime? UpdatedAt { get; set; }
        public List<Microsoft.AspNetCore.Http.IFormFile> Files { get; set; }

        public List<FileToApiViewModel> FilesToApiViewModels { get; set; } = new List<FileToApiViewModel>();
    }
}
