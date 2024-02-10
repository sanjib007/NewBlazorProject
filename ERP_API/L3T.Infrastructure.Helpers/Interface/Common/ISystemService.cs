namespace L3T.Infrastructure.Helpers.Interface.Common;

public interface ISystemService
{
    Task ErrorLogEntry(string methodName, string errorMessage, string errorDescription);
}