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
    }
}
