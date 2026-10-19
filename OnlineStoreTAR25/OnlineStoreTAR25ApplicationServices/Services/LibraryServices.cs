using Microsoft.EntityFrameworkCore;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;
using System;
using System.Threading.Tasks;

namespace ShopTARpe25.ApplicationServices.Services
{
    public class LibraryServices : ILibraryServices
    {
        private readonly ShopTARpe25Context _context;
        private readonly IFileServices _fileServices;

        public LibraryServices
            (
                ShopTARpe25Context context,
                IFileServices fileServices
            )
        {
            _context = context;
            _fileServices = fileServices;
        }

        // 1. Kirje loomine andmebaasi
        public async Task<Library> Create(LibraryDto dto)
        {
            Library domain = new();

            domain.Id = Guid.NewGuid();
            domain.Name = dto.Name;
            domain.Classification = dto.Classification;
            domain.BuiltDate = DateTime.Now;

            // LISATUD KONVERTEERIMINE: tagab tüüpide sobivuse andmebaasiga
            domain.Crew = Convert.ToInt32(dto.Crew);
            domain.EnginePower = Convert.ToInt32(dto.EnginePower);

            domain.CreatedAt = DateTime.Now;
            domain.ModifiedAt = DateTime.Now;

            // Kutsume faili teenust tervislikult (sisu on tühi, vigu ei teki)
            _fileServices.FilesToApi(dto, domain);

            // Salvestame andmed õigesse Library tabelisse
            await _context.Library.AddAsync(domain);
            await _context.SaveChangesAsync();

            return domain;
        }

        // 2. Kirje detailide pärimine andmebaasist
        public async Task<Library> DetailsAsync(Guid id)
        {
            var result = await _context.Library
                .FirstOrDefaultAsync(x => x.Id == id);

            return result;
        }

        // 3. Kirje uuendamine andmebaasis
        public async Task<Library> Update(LibraryDto dto)
        {
            var domain = await _context.Library.FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (domain == null) return null;

            domain.Name = dto.Name;
            domain.Classification = dto.Classification;
            domain.BuiltDate = DateTime.Now;

            // LISATUD KONVERTEERIMINE: tagab tüüpide sobivuse andmebaasiga
            domain.Crew = Convert.ToInt32(dto.Crew);
            domain.EnginePower = Convert.ToInt32(dto.EnginePower);

            domain.ModifiedAt = DateTime.Now;

            _fileServices.FilesToApi(dto, domain);

            _context.Library.Update(domain);
            await _context.SaveChangesAsync();

            return domain;
        }

        // 4. Kirje kustutamine andmebaasist (pildivaba ja tervislik)
        public async Task<Library> Delete(Guid id)
        {
            var result = await _context.Library
                .FirstOrDefaultAsync(x => x.Id == id);

            if (result == null) return null;

            _context.Library.Remove(result);
            await _context.SaveChangesAsync();

            return result;
        }
    }
}
