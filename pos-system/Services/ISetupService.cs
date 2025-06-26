using pos_system.Models;

namespace pos_system.Services
{
    public interface ISetupService
    {
        void SetUsername(string username);
        Task<TblSetup> GetSetup();
        Task UpdateSetup(TblSetup data);
    }
}
