namespace L3T.OAuth2DotNet7.Services.Interface
{
    public interface IRSAEncryptionSerivce
    {
        Task<string> DecryptUsingCertificate(string data);
        Task<string> EncryptUsingCertificate(string data);
    }
}
