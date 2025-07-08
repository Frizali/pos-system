using pos_system.DTOs;
using pos_system.Models;

namespace pos_system.Services
{
    public interface IEmailService
    {
        Task SendStockNotification(PartDTO data, string toEmail);
        Task SendPreOrderNotification(PreOrderMailDTO order, string toEmail);
        Task SendPreOrderFeedbackNotification(TblOrder order, string toEmail);

    }
}
