using pos_system.DTOs;
using pos_system.Models;

namespace pos_system.Services
{
    public interface IInventoryService
    {
        Task<InventoryViewModel> GetListPart(string search, string searchPartType);
        Task Save(InventoryFormModel data);
        Task<InventoryFormModel> GetPartTypeAndUnit();
        Task DeletePart(string id);
    }
}
