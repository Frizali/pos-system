using MailKit;
using pos_system.DTOs;
using pos_system.Helpers;
using pos_system.Services;


namespace pos_test
{
    public class MailTest
    {

        [Test]
        public async Task SendEmail_ShouldSend()
        {
            var service = new EmailService();
            var param = new PartDTO()
            {
                PartId = Unique.ID(),
                PartName = "Minyak Goreng",
                PartCd = "l",
                PartQty = 1,
                Price = 14000,
                UnitName = "liter",
                LowerLimit = 2
            };

            await service.SendStockNotification(param, "jesenwijaya04@gmail.com").ConfigureAwait(false);
        }
    }
}
