using pos_system.Models;

namespace pos_system.Repository
{
    public interface ISetupRepo
    {
        ICrudRepo<TblSetup> GetRepo();
    }
}
