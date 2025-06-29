using pos_system.DTOs;
using pos_system.Helpers;
using pos_system.Models;
using pos_system.Repository;

namespace pos_system.Services
{
    public class InventoryService(IInventoryRepo inventoryRepo, ISetupRepo setupRepo, IEmailService emailService) : IInventoryService
    {
        IInventoryRepo _inventoryRepo = inventoryRepo;
        ISetupRepo _setupRepo = setupRepo;
        IEmailService _emailService = emailService;
        readonly int codeLength = 4;
        private string _username = "System";

        public void SetUsername(string username)
        {
            _username = username;
            _inventoryRepo.GetRepo().SetUsername(username);
        }

        public async Task<InventoryViewModel> GetListPart(string search, string searchPartType)
        {
            InventoryViewModel list = await _inventoryRepo.GetListPart(search, searchPartType).ConfigureAwait(false);
            return list;
        }

        public async Task Save(InventoryFormModel data)
        {
            data.Part.PartCd = Unique.GenerateCode(data.Part.PartName, codeLength);

            await _inventoryRepo.GetRepo().Add(data.Part).ConfigureAwait(false);
        }

        public async Task<InventoryFormModel> GetPartTypeAndUnit()
        {
            var result = await _inventoryRepo.GetPartTypeAndUnit().ConfigureAwait(false);
            return result;
        }

        public async Task DeletePart(string id)
        {
            await _inventoryRepo.DeletePart(id).ConfigureAwait(false);
        }

        public async Task<InventoryFormModel> LoadEditModal(string id)
        {
            var result = await _inventoryRepo.LoadEditModal(id).ConfigureAwait(false);
            return result;
        }

        public async Task Update(InventoryFormModel data)
        {
            await _inventoryRepo.Update(data).ConfigureAwait(false);
        }

        public async Task<EditStockFormModal> GetEditStockModal(string partId)
        {
            var result = await _inventoryRepo.GetEditStockModal(partId).ConfigureAwait(false);
            return result;
        }

        public async Task AddPartMovement(EditStockFormModal param)
        {
            if (param.PartMovQty != 0)
            {
                var part = await _inventoryRepo.GetRepo().GetById(param.PartId).ConfigureAwait(false);
                if (param.PartMovQty < 0 && (part.PartQty - Math.Abs(param.PartMovQty)) < 0) throw new Exception($"PartMovQty, Can't input value more than Quantity");

                param.PartMovType = param.PartMovQty > 0 ? 1 : 2;
                TblPartMovement data = new TblPartMovement()
                {
                    PartMovementId = param.PartMovementId,
                    PartId = param.PartId,
                    PartMovQty = param.PartMovQty,
                    Remark = param.Remark,
                    PartMovType = param.PartMovType,
                    InputedBy = param.InputedBy,
                };

                data.LastPartQty = part.PartQty;
                var qty = part.PartQty += param.PartMovQty;
                part.PartQty = qty <= 0 ? 0 : qty;

                await _inventoryRepo.AddPartMovement(data).ConfigureAwait(false);
                await _inventoryRepo.GetRepo().Update(part).ConfigureAwait(false);

                if(part.PartQty <= part.LowerLimit)
                {
                    var setup = await _setupRepo.GetRepo().GetAll().ConfigureAwait(false);
                    var toEmail = setup.Select(s => s.Email).First();
                    var parts = await _inventoryRepo.GetListPart(param.PartName,"").ConfigureAwait(false);
                    var partDTO = parts.PartList.Where(p => p.PartId == param.PartId).First();

                    await _emailService.SendStockNotification(partDTO, toEmail).ConfigureAwait(false);
                }
            }
        }

        public async Task<InventoryMoveViewModel> GetListPartMovement(string partId, string partTypeId, DateTime date, string month, string year)
        {
            InventoryMoveViewModel list = await _inventoryRepo.GetListPartMovement(partId, partTypeId, date, month, year).ConfigureAwait(false);
            return list;
        }
    }
}
