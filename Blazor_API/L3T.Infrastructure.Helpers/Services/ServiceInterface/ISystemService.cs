namespace L3T.Infrastructure.Helpers.Services.ServiceInterface;

public interface ISystemService
{
    Task ErrorLogEntry(string methodName, string errorMessage, string errorDescription);
}