using L3T.Infrastructure.Helpers.Models.TicketEntity;

namespace L3T.Infrastructure.Helpers.Interface.Ticket
{
    public interface ITicketService
    {
        #region TicketEntry
        Task<long> TicketEntrySubmitAsync(TicketEntry input);
        Task<List<TicketEntry>> GetAllTicket();
        Task<TicketEntry> GetTicketById(long Id);
        Task<bool> DeleteTicket(long id);
        Task<bool> UpdateTicket(long id, TicketEntry model);
        #endregion
    }
}
