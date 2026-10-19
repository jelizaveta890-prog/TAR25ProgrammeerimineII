using Microsoft.Extensions.Hosting;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;
using System.Threading.Tasks;

namespace ShopTARpe25.ApplicationServices.Services
{
    public class FileServices : IFileServices
    {
        private readonly ShopTARpe25Context _context;
        private readonly IHostEnvironment _webHost;

        public FileServices
            (
                ShopTARpe25Context context,
                IHostEnvironment webHost
            )
        {
            _context = context;
            _webHost = webHost;
        }

        // Tervislik meetod, mis vastab sinu uuele IFileServices liidesele
        // Kuna raamatukogu projektis me faile ei kasuta, jätame selle meetodi sisu tühjaks
        public void FilesToApi(LibraryDto dto, Library domain)
        {
            // Failide üleslaadimise loogika on eemaldatud, kuna FileToApi klassi andmebaasis pole
        }
    }
}
