using pos_system.DTOs;
using pos_system.Models;

namespace pos_system.Repository
{
    public interface IInventoryRepo
    {
        ICrudRepo<TblPart> GetRepo();
        Task<InventoryViewModel> GetListPart(string search, string searchPartType);
        Task<InventoryFormModel> GetPartTypeAndUnit();
        Task DeletePart(string id);
        Task<InventoryFormModel> LoadEditModal(string id);
        Task Update(InventoryFormModel data);
        Task<EditStockFormModal> GetEditStockModal(string partId);
        Task AddPartMovement(TblPartMovement data);
        Task<InventoryMoveViewModel> GetListPartMovement(string partId, string partTypeId, DateTime date, string month, string year);
    }
}
