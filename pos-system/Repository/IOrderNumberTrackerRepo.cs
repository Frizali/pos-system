namespace pos_system.Repository
{
    public interface IOrderNumberTrackerRepo
    {
        Task GenerateOrderNumber();
        Task<string> GetOrderNumber();
    }
}
