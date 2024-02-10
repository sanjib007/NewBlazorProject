using L3T.Infrastructure.Helpers.DataContext;
using L3T.Infrastructure.Helpers.Models.SystemErrorLog;
using L3T.Infrastructure.Helpers.Services.ServiceInterface;
using Microsoft.Extensions.Logging;
using ILogger = Serilog.ILogger;

namespace L3T.Infrastructure.Helpers.Services.ServiceImplementation;

public class SystemService : ISystemService
{
    private readonly ChangeRequestDataContext _context;
    private readonly ILogger<SystemService> _logger;

    public SystemService(ChangeRequestDataContext context, ILogger<SystemService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task ErrorLogEntry(string methodName, string errorMessage, string errorDescription)
    {
        try
        {
            SystemErrorLog errorLogObj = new SystemErrorLog();
            errorLogObj.MethodName = methodName;
            errorLogObj.ErrorMessage = errorMessage;
            errorLogObj.ErrorDescription = errorDescription;
            errorLogObj.InsertedDate = DateTime.Now;
            await _context.AddAsync(errorLogObj);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogInformation("Error saving UserAccount-", ex.Message.ToString());
        }
    }
}