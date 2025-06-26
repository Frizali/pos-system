using pos_system.Models;

namespace pos_system.Repository
{
    public class SetupRepo : ISetupRepo
    {
        private readonly ICrudRepo<TblSetup> _repo;

        public SetupRepo(ICrudRepo<TblSetup> repo)
        {
            _repo = repo;
        }

        public ICrudRepo<TblSetup> GetRepo()
        {
            return _repo;
        }
    }
}
