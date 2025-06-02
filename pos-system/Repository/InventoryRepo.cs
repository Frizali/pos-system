using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pos_system.Data;
using pos_system.DTOs;
using pos_system.Models;

namespace pos_system.Repository
{
    public class InventoryRepo(AppDbContext context, IMapper mapper, ICrudRepo<TblPart> repo) : IInventoryRepo
    {
        readonly AppDbContext _context = context;
        readonly IMapper _mapper = mapper;
        readonly ICrudRepo<TblPart> _repo = repo;

        public async Task<InventoryViewModel> GetListPart(string search, string searchPartType)
        {
            search = search ?? "";
            searchPartType = searchPartType ?? "";

            var query = from part in _context.TblParts
                        join partType in _context.TblPartTypes on part.PartTypeId equals partType.PartTypeId
                        join unit in _context.TblUnits on part.UnitId equals unit.UnitId
                        where part.PartName.Contains(search) && partType.PartTypeName.Contains(searchPartType)
                        select new PartListDTO
                        {
                            PartId = part.PartId,
                            PartTypeId = part.PartTypeId,
                            PartTypeName = partType.PartTypeName,
                            UnitId = part.UnitId,
                            UnitName = unit.UnitName,
                            UnitCd = unit.UnitCd,
                            PartCd = part.PartCd,
                            PartName = part.PartName,
                            PartQty = part.PartQty,
                            Price = part.Price,
                            Note = part.Note
                        };
            var partData = await query.ToListAsync().ConfigureAwait(false);
            var partTypeData = await _context.TblPartTypes.ToListAsync().ConfigureAwait(false);

            return new InventoryViewModel
            {
                PartList = partData,
                PartType = partTypeData
            };
        }

        public async Task<InventoryFormModel> GetPartTypeAndUnit()
        {
            var partType = await _context.TblPartTypes.ToListAsync().ConfigureAwait(false);
            var unit = await _context.TblUnits.ToListAsync().ConfigureAwait(false);

            return new InventoryFormModel
            {
                PartTypes = partType,
                Units = unit
            };
        }

        public async Task Update(InventoryFormModel data)
        {
            var partData = await _context.TblParts.Where(x => x.PartId == data.Part.PartId).FirstOrDefaultAsync().ConfigureAwait(false);

            partData.PartName = data.Part.PartName;
            partData.PartTypeId = data.Part.PartTypeId;
            partData.UnitId = data.Part.UnitId;
            partData.PartQty = data.Part.PartQty;
            partData.Price = data.Part.Price;
            partData.Note = data.Part.Note;

            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeletePart(string id)
        {
            var data = await _context.TblParts.FirstOrDefaultAsync(x => x.PartId == id).ConfigureAwait(false);
            _context.TblParts.Remove(data);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<InventoryFormModel> LoadEditModal(string id)
        {
            var part = await _context.TblParts.FirstOrDefaultAsync(p => p.PartId == id);

            var partType = await _context.TblPartTypes.ToListAsync().ConfigureAwait(false);
            var unit = await _context.TblUnits.ToListAsync().ConfigureAwait(false);

            return new InventoryFormModel
            {
                Part = part,
                PartTypes = partType,
                Units = unit
            };
        }

        public ICrudRepo<TblPart> GetRepo()
        {
            return _repo;
        }
    }
}
