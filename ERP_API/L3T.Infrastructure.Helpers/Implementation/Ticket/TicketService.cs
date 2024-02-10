using L3T.Infrastructure.Helpers.DataContext;
using L3T.Infrastructure.Helpers.Interface.Ticket;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L3T.Infrastructure.Helpers.Models.TicketEntity;

namespace L3T.Infrastructure.Helpers.Implementation.Ticket
{
    public class TicketService: ITicketService
    {
        private readonly TicketDataWriteContext context;
        private readonly TicketDataReadContext _context4read;
        private readonly ILogger<TicketService> _logger;
        public TicketService(TicketDataWriteContext context, ILogger<TicketService> logger, TicketDataReadContext context4read)
        {
            this.context = context;
            _logger = logger;
            _context4read = context4read;
        }
        public async Task<long> TicketEntrySubmitAsync(TicketEntry input)
        {
            try
            {
                context.TicketEntry.Add(input);
                await context.SaveChangesAsync();
                return input.TicketId;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error saving TicketEntry SubmitAsync-", ex);
            }
            return -1;
        }
        public async Task<List<TicketEntry>> GetAllTicket()
        {
            try
            {
                var item = await _context4read.TicketEntry.ToListAsync();
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("An error occurred when fetching the getAllowForeignPhone method  Exception: " + ex.Message.ToString());
            }
            return null;
        }
        public async Task<TicketEntry> GetTicketById(long Id)
        {
            _logger.LogInformation("Fetching GetTicketById by Id" + Id);
            try
            {
                var item = await _context4read.TicketEntry.FirstOrDefaultAsync(f => f.TicketId == Id).ConfigureAwait(false);
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("An error occurred when fetching the GetTicketById by id:" + Id + " Exception: " + ex.Message.ToString());
            }
            return null;
        }
        public async Task<bool> DeleteTicket(long id)
        {
            _logger.LogInformation("Fetching  Ticket Data by id : " + id);
            try
            {
                var item = await context.TicketEntry.SingleOrDefaultAsync(f => f.TicketId == id);
                if (item != null)
                {
                    context.TicketEntry.Remove(item);
                    await context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("An error occurred when deleteing the Ticket by id:" + id + " Exception: ", ex);
                return false;
            }
        }
        public async Task<bool> UpdateTicket(long id, TicketEntry model)
        {
            _logger.LogInformation("updating TicketEntry id is : " + id);
            try
            {
                var OldEntity = await context.TicketEntry.FirstOrDefaultAsync(x => x.TicketId == id);
                if (OldEntity != null)
                {
                    context.TicketEntry.Update(model);
                    await context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("An error occurred when updating  the TicketEntry  id is :" + id + " Exception: ", ex);
                return false;
            }
        }
    }
}
