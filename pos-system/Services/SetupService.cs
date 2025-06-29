using pos_system.Models;
using pos_system.Repository;

namespace pos_system.Services
{
    public class SetupService(ISetupRepo setupRepo) : ISetupService
    {
        private ISetupRepo _setupRepo = setupRepo;
        private string _username;

        public void SetUsername(string username)
        {
            _username = username;
            _setupRepo.GetRepo().SetUsername(username);
        }

        public async Task<TblSetup> GetSetup()
        {
            var setup = await _setupRepo.GetRepo().GetAll();
            return setup.First();
        }

        public async Task UpdateSetup(TblSetup setup)
        {
            if (setup == null)
            {
                throw new ArgumentNullException(nameof(setup), "Setup cannot be null");
            }

            var existingSetup = await _setupRepo.GetRepo().GetById(setup.ID);
            if (existingSetup == null)
            {
                throw new KeyNotFoundException("Setup not found");
            }

            existingSetup.CompanyName = setup.CompanyName;
            existingSetup.CompanyAddress = setup.CompanyAddress;
            existingSetup.Email = setup.Email;

            await _setupRepo.GetRepo().Update(existingSetup);
        }
    }
}
