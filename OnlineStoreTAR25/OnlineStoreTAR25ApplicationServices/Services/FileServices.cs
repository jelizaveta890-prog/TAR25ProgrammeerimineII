using Microsoft.Extensions.Hosting;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.Serviceinterface;
using ShopTARpe25.Data;

namespace ShopTARpe25.ApplicationServices.Services
{
    public class FileServices : IFileServices
    {
        //teha constructor, mis ühendab DB-d

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

        public void FileToApi(SpaceshipDto dto, Spaceship domain)
        {
            //kindlasti peab ankeedi olema üksi fail
            if (dto.Files != null && dto.Files.Count > 0)
            {
                //kuio ei ole wwwroot-s multipleFileUpload directory
                if (!Directory.Exists(_webHost.ContentRootPath + "\\wwwroot\\multipleFileUpload\\"))
                {
                    //,siis tee directory wwwrooti alla
                    Directory.CreateDirectory(_webHost.ContentRootPath + "\\wwwroot\\multipleFileUpload\\");
                }

                foreach (var file in dto.Files)
                {
                    //meil on aja teha muutuja nimega uploadFolder.
                    //sinna muutuja taha on vaja Path kombineerida

                    string uploadFolder = Path.Combine(_webHost.ContentRootPath, "wwwroot", "multipleFileUpload");
                    //igale failile unikaalne Guid selle nime ette
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.Name;
                    string filePath = Path.Combine(uploadFolder, uniqueFileName);

                    using (var filestream = new FileStream(filePath, FileMode.Create))
                       
                        file.CopyTo(filestream);
                    
                    FileToApi path = new FileToApi
                    {
                        

                        Id = Guid.NewGuid(),
                        ExistingFilePath = uniqueFileName,
                        SpaceshipId = domain.Id

                    };

                    _context.FileToApis.AddAsync(path);


                }
            }
        }
    }
}
