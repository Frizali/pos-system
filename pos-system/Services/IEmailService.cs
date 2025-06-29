using pos_system.DTOs;

namespace pos_system.Services
{
    public interface IEmailService
    {
        Task SendStockNotification(PartDTO data, string toEmail);
    }
}
