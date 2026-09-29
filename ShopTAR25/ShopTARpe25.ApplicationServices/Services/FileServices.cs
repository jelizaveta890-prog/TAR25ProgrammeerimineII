using Microsoft.Extensions.Hosting;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;

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


        public void FilesToApi(SpaceshipDto dto, Spaceship domain)
        {
            //kindlasti peab ankeedil olema üks fail
            if (dto.Files != null && dto.Files.Count > 0)
            {
                //kui ei ole wwroot multiple fileUpload directoryt
                if (!Directory.Exists(_webHost.ContentRootPath + "\\wwwroot\\multipleFileUpload\\"))
                {
                    //tee directory wwwrooti alla
                    Directory.Exists(_webHost.ContentRootPath + "\\wwwroot\\multipleFileUpload\\");

                    foreach (var file in dto.Files)
                    {
                        //meil on vaja teha muutuja nimega uploadsFolder
                        //sinna muutuja taha on vaja Path kombineerida
                        string uniqueFileName = Path.Combine(_webHost.ContentRootPath + "\\wwwroot\\multipleFileUpload\\");
                        string iniqueFileName = Path.NewGuid().ToString() + "_" + file.Name;

                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                            //domaini teha File to Api
                            FileToApiDto
                        }
                    }
                }
            }
        }
    }
}
