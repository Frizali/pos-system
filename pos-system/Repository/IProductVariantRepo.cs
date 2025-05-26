using pos_system.Models;

namespace pos_system.Repository
{
    public interface IProductVariantRepo
    {
        ICrudRepo<TblProductVariant> GetRepo();
    }
}
