using pos_system.Models;

namespace pos_system.Repository
{
    public interface IVariantGroupRepo
    {
        ICrudRepo<TblVariantGroup> GetRepo();
    }
}
