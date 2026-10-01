using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Identity.Client;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.Serviceinterface;
using ShopTARpe25.Data;
using System;
using System.Threading.Tasks;

namespace ShopTARpe25.ApplicationServices.Services
{
    public class KindergardenServices : IKindergardenServices
    {
        private readonly ShopTARpe25Context _context;

        public KindergardenServices(ShopTARpe25Context context)
        {
            _context = context;
        }

        public async Task<Kindergarden> Create(KindergardenDto dto)
        {
            Kindergarden domain = new();

            domain.Id = dto.Id;
            domain.GroupName = dto.GroupName;
            domain.ChildrenCount = dto.ChildrenCount;
            domain.KindergartenName = dto.KindergartenName;
            domain.TeacherName = dto.TeacherName;
            domain.CreatedAt = DateTime.Now;
            domain.UpdatedAt = DateTime.Now;
          
            var realEstateDomain = new RealEstate
            {
                Id = domain.Id,
                CreatedAt = (DateTime)domain.CreatedAt,
                UpdatedAt = (DateTime)domain.UpdatedAt,
                Address = dto.GroupName ?? "Määramata aadress" 
            };


            await _context.RealEstates.AddAsync(realEstateDomain);
            await _context.SaveChangesAsync();

            return domain;
        }

        public async Task<Kindergarden> DetailsAsync(Guid id)
        {
            var realEstate = await _context.RealEstates
                .FirstOrDefaultAsync(x => x.Id == id);

            if (realEstate == null) return null;

            return new Kindergarden
            {
                Id = realEstate.Id,
                CreatedAt = realEstate.CreatedAt,
                UpdatedAt = realEstate.UpdatedAt
            };
        }

        public async Task<Kindergarden> Update(KindergardenDto dto)
        {
            var realEstate = await _context.RealEstates
                .FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (realEstate != null)
            {
                realEstate.UpdatedAt = DateTime.Now;

                _context.RealEstates.Update(realEstate);
                await _context.SaveChangesAsync();
            }

            return new Kindergarden { Id = dto.Id };
        }

        public async Task<Kindergarden> Delete(Guid id)
        {
            var result = await _context.RealEstates
                .FirstOrDefaultAsync(x => x.Id == id);

            if (result != null)
            {
                _context.RealEstates.Remove(result);
                await _context.SaveChangesAsync();
            }

            return new Kindergarden { Id = id };
        }
    }
}
