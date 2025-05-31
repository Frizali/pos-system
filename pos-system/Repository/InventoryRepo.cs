using AutoMapper;
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

        public ICrudRepo<TblPart> GetRepo()
        {
            return _repo;
        }
    }
}
