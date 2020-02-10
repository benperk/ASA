using System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Azure.KeyVault;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Azure.KeyVault.WebKey;
using Microsoft.Azure.KeyVault.Models;

namespace authzonecore.Pages
{
    public class IndexModel : PageModel
    {
        private static IConfiguration _configuration;
        private readonly ILogger<IndexModel> _logger;
        public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        private static string GetKeyVaultEndpoint() => "https://csharpguitar.vault.azure.net";
        public void OnGet()
        {
            ViewData["Message"] = "Unencrypted ConnectionString value = " + _configuration["ConnectionString"];
            string KEYID = "https://csharpguitar.vault.azure.net/keys/DAR/69a0749593b6400ba6283759665b3fb9";
            
            try
            {
                var azureServiceTokenProvider = new AzureServiceTokenProvider();
                var keyVaultClient = new KeyVaultClient(
                    new KeyVaultClient.AuthenticationCallback(
                        azureServiceTokenProvider.KeyVaultTokenCallback));

		            //Just for fun
                var encrypted = EncryptText("Stratocaster", keyVaultClient, KEYID);
                var decrypted = DecryptText(encrypted, keyVaultClient, KEYID);
                ViewData["Encrypt"] = $"Encrypted: {encrypted}";
                ViewData["Decrypt"] = $"Decrypted: {decrypted}";

                //Encrypt the unencrypted Azure Key Vault Secret 
                var encryptedConnectionString = EncryptText(_configuration["ConnectionString"], keyVaultClient, KEYID);
                ViewData["EncryptConnectionString"] = $"Encrypted ConnectionString: {encryptedConnectionString}";                

                //Create a new / update the connection string with an encrypted version
                SetKeyVaultSecret("EncryptedConnectionString", encryptedConnectionString, keyVaultClient);

                //Get the encrypted connection string
                var secretBundle = GetKeyVaultSecret("EncryptedConnectionString", keyVaultClient);
                //var ecs = secretBundle.Value;

                //Decrypt the connection string for use with an application
                var decryptedConnectionString = DecryptText(secretBundle.Value, keyVaultClient, KEYID);
                ViewData["DecryptConnectionString"] = $"Decrypted ConnectionString: {decryptedConnectionString}";
            }
            catch(Exception ex)
            {
                ViewData["Key"] = $"Something happened: {ex.Message}";
            }
        }

        public string DecryptText(string textToDecrypt, KeyVaultClient keyVaultClient, string keyidentifier)
        {
            var kv = keyVaultClient;
            try
            {
                var key = kv.GetKeyAsync(keyidentifier).Result;
                var publicKey = Convert.ToBase64String(key.Key.N);
                using var rsa = new RSACryptoServiceProvider();
                var p = new RSAParameters()
                {
                    Modulus = key.Key.N,
                    Exponent = key.Key.E
                };
                rsa.ImportParameters(p);
                var encryptedTextNew = Convert.FromBase64String(textToDecrypt);
                var decryptedData = kv.DecryptAsync(key.KeyIdentifier.Identifier.ToString(), JsonWebKeyEncryptionAlgorithm.RSAOAEP, encryptedTextNew).GetAwaiter().GetResult();
                var decryptedText = Encoding.Unicode.GetString(decryptedData.Result);
                return decryptedText;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return default;
            }
        }
        public string EncryptText(string textToEncrypt, KeyVaultClient keyVaultClient, string keyidentifier)
        {
            var kv = keyVaultClient;
            try
            {
                var key = kv.GetKeyAsync(keyidentifier).GetAwaiter().GetResult();
                var publicKey = Convert.ToBase64String(key.Key.N);
                using var rsa = new RSACryptoServiceProvider();
                var p = new RSAParameters()
                {
                    Modulus = key.Key.N,
                    Exponent = key.Key.E
                };
                rsa.ImportParameters(p);
                var byteData = Encoding.Unicode.GetBytes(textToEncrypt);
                var encryptedText = rsa.Encrypt(byteData, true);
                string encText = Convert.ToBase64String(encryptedText);
                return encText;
            }
            catch (Exception ex)
            {
                return default;
            }
        }
        public void SetKeyVaultSecret(string secretName, string secretValue, KeyVaultClient keyVaultClient)
        {
            try
            {
                keyVaultClient.SetSecretAsync(GetKeyVaultEndpoint(), secretName, secretValue);
            }
            catch(Exception ex)
            {
                throw;
            }
        }
        public SecretBundle GetKeyVaultSecret(string secretName, KeyVaultClient keyVaultClient)
        {
            try
            {
                return keyVaultClient.GetSecretAsync(GetKeyVaultEndpoint(), secretName).Result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
