using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using System.Threading.Tasks;

namespace ShopTARpe25.Core.ServiceInterface
{
    public interface IFileServices
    {
        void FilesToApi(LibraryDto dto, Library domain);
    }
}
