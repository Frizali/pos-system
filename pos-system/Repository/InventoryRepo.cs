using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
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
                        select new PartDTO
                        {
                            PartId = part.PartId,
                            PartTypeId = part.PartTypeId,
                            PartTypeName = partType.PartTypeName,
                            UnitId = part.UnitId,
                            UnitName = unit.UnitName,
                            UnitCd = unit.UnitCd,
                            PartCd = part.PartCd,
                            PartName = part.PartName,
                            LowerLimit = part.LowerLimit,
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
            partData.LowerLimit = data.Part.LowerLimit;
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

        public async Task<EditStockFormModal> GetEditStockModal(string partId)
        {
            var query = from part in _context.TblParts
                        join unit in _context.TblUnits on part.UnitId equals unit.UnitId
                        join type in _context.TblPartTypes on part.PartTypeId equals type.PartTypeId
                        where part.PartId == partId
                        select new EditStockFormModal
                        {
                            PartId = partId,
                            PartName = part.PartName,
                            PartTypeName = type.PartTypeName,
                            UnitCd = unit.UnitCd,
                            PartQty = part.PartQty,
                            Price = part.Price,
                            LowerLimit = part.LowerLimit
                        };

            return await query.FirstOrDefaultAsync().ConfigureAwait(false);
        }

        public async Task AddPartMovement(TblPartMovement data)
        {
            await _context.TblPartMovements.AddAsync(data).ConfigureAwait(false);
        }

        public async Task<InventoryMoveViewModel> GetListPartMovement(string partId, string partTypeId, DateTime date, string month, string year)
        {
            partId ??= "";
            partTypeId ??= "";
            month ??= "";
            year ??= "";

            var query = from partMov in _context.TblPartMovements
                        join part in _context.TblParts on partMov.PartId equals part.PartId
                        join partType in _context.TblPartTypes on part.PartTypeId equals partType.PartTypeId
                        join unit in _context.TblUnits on part.UnitId equals unit.UnitId
                        where partMov.PartId.Contains(partId) &&
                              partType.PartTypeId.Contains(partTypeId)
                        select new
                        {
                            PartMov = partMov,
                            Part = part,
                            PartType = partType,
                            Unit = unit
                        };

            if (date != DateTime.MinValue)
            {
                query = query.Where(x => x.PartMov.CreatedAt.Date == date.Date);
            }

            if (!string.IsNullOrEmpty(month) && !string.IsNullOrEmpty(year))
            {
                if (int.TryParse(month, out int m) && int.TryParse(year, out int y))
                {
                    query = query.Where(x => x.PartMov.CreatedAt.Month == m && x.PartMov.CreatedAt.Year == y);
                }
            }

            var partMoves = await query
                .OrderByDescending(x => x.PartMov.CreatedAt)
                .Select(x => new PartMovDTO
                {
                    PartName = x.Part.PartName,
                    Category = x.PartType.PartTypeName,
                    UnitCD = x.Unit.UnitCd,
                    LastPartQry = x.PartMov.LastPartQty,
                    StockIn = x.PartMov.PartMovQty > 0 ? x.PartMov.PartMovQty : 0,
                    StockOut = x.PartMov.PartMovQty < 0 ? x.PartMov.PartMovQty : 0,
                    CurrPartQry = x.Part.PartQty,
                    Note = x.PartMov.Remark ?? "",
                    InputedBy = x.PartMov.InputedBy ?? "",
                    CreatedAt = x.PartMov.CreatedAt,
                })
                .ToListAsync()
                .ConfigureAwait(false);

            var parts = await _context.TblParts.ToListAsync().ConfigureAwait(false);
            var category = await _context.TblPartTypes.ToListAsync().ConfigureAwait(false);

            return new InventoryMoveViewModel
            {
                PartMovs = partMoves,
                Parts = parts,
                PartTypes = category
            };
        }

        public ICrudRepo<TblPart> GetRepo()
        {
            return _repo;
        }
    }
}
