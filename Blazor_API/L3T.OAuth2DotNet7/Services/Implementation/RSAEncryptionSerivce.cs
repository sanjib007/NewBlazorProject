using L3T.OAuth2DotNet7.Services.Interface;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;
using System.Reflection;

namespace L3T.OAuth2DotNet7.Services.Implementation
{
    public class RSAEncryptionSerivce : IRSAEncryptionSerivce
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ILogger<RSAEncryptionSerivce> _logger;
        public RSAEncryptionSerivce(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }


        public async Task<string> EncryptUsingCertificate(string data)
        {
            var methodName = "RSAEncryptionSerivce/EncryptUsingCertificate";
            try
            {
                byte[] byteData = Encoding.UTF8.GetBytes(data);
                string path = Path.Combine(_hostEnvironment.WebRootPath, "key", "mycert.pem");
                var collection = new X509Certificate2Collection();
                collection.Import(path);
                var certificate = collection[0];
                var output = "";
                using (RSA csp = (RSA)certificate.PublicKey.Key)
                {
                    byte[] bytesEncrypted = csp.Encrypt(byteData, RSAEncryptionPadding.OaepSHA1);
                    output = Convert.ToBase64String(bytesEncrypted);
                }
                return output.Replace('/', '*');
            }
            catch (Exception ex)
            {
                await errorMethord(ex, methodName);
                return "";
            }
        }

        public async Task<string> DecryptUsingCertificate(string data)
        {
            var methodName = "RSAEncryptionSerivce/DecryptUsingCertificate";
            try
            {
                data = data.Replace('*', '/');
                byte[] byteData = Convert.FromBase64String(data);
                string path = Path.Combine(_hostEnvironment.WebRootPath, "key", "mycertprivatekey.pfx");
                var Password = "L3t@2089"; //Note This Password is That Password That We Have Put On Generate Keys  
                var collection = new X509Certificate2Collection();
                collection.Import(System.IO.File.ReadAllBytes(path), Password, X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);
                X509Certificate2 certificate = new X509Certificate2();
                certificate = collection[0];
                foreach (var cert in collection)
                {
                    if (cert.FriendlyName.Contains("my certificate"))
                    {
                        certificate = cert;
                    }
                }
                if (certificate.HasPrivateKey)
                {
                    RSA csp = (RSA)certificate.PrivateKey;
                    var privateKey = certificate.PrivateKey as RSACryptoServiceProvider;
                    var keys = Encoding.UTF8.GetString(csp.Decrypt(byteData, RSAEncryptionPadding.OaepSHA1));
                    return keys;
                }
                throw new Exception("PrivateKey is not found.");
            }
            catch (Exception ex) {
                await errorMethord(ex, methodName);
                return "";
            }
            
        }


        private async Task<string> errorMethord(Exception ex, string info)
        {
            string errormessage = "Error : " + ex.Message.ToString();
            _logger.LogInformation("Method Name : " + info + ", Exception " + errormessage);
            //await _systemService.ErrorLogEntry(info, info, errormessage);
            //_mailrepo.sendMail(ex.ToString(), "Error occure on" + info + "method");
            return errormessage;
        }
    }
}
