using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using System;
using System.Threading.Tasks;

namespace ShopTARpe25.Core.ServiceInterface
{
    public interface ILibraryServices
    {
        Task<Library> Create(LibraryDto dto);

        Task<Library> DetailsAsync(Guid id);

        Task<Library> Update(LibraryDto dto);

        Task<Library> Delete(Guid id);
    }
}
