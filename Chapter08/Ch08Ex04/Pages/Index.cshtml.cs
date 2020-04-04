using System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.WebKey;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

using System.IdentityModel.Tokens.Jwt;

namespace csharpguitar_auth.Pages
{
    public class IndexModel : PageModel
    {
        private static IConfiguration _configuration;
        private readonly ILogger<IndexModel> _logger;
        private HttpContext _httpContext;
        public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration, IHttpContextAccessor httpContext)
        {
            _logger = logger;
            _configuration = configuration;
            _httpContext = httpContext.HttpContext;
        }
        private static string GetKeyVaultEndpoint() => "https://csharpguitar.vault.azure.net";
        public async Task<IActionResult> OnGetAsync()
        {
            var guitarDbConnectionString = _configuration["GuitarDbConnectionString"];
            string KEYID = "https://csharpguitar.vault.azure.net/keys/DAR/69a0749593b6400ba6283759665b3fb9";
            try
            {
                var azureServiceTokenProvider = new AzureServiceTokenProvider();
                var keyVaultClient = new KeyVaultClient(
                    new KeyVaultClient.AuthenticationCallback(
                        azureServiceTokenProvider.KeyVaultTokenCallback));

                #region Database / Encryption

                using var connection = new SqlConnection(guitarDbConnectionString);

                #region INSERT               
                var encrypted = EncryptText("Takamine", keyVaultClient, KEYID);
                var insert = "INSERT INTO dbo.GUITARS (GUITAR_ID, BRAND) VALUES (@GID, @BRAND)";
                using SqlCommand commandI = new SqlCommand(insert, connection);
                commandI.Parameters.Add("@GID", SqlDbType.Int).Value = 3;
                commandI.Parameters.Add("@BRAND", SqlDbType.VarChar).Value = encrypted;
                await connection.OpenAsync();
                ViewData["Message"] = connection.State.ToString();
                await commandI.ExecuteNonQueryAsync();
                #endregion

                #region SELECT
                var select = "SELECT BRAND FROM dbo.GUITARS WHERE GUITAR_ID = @GID";
                using SqlCommand commandS = new SqlCommand(select, connection);
                commandS.Parameters.Add("@GID", SqlDbType.Int).Value = 3;
                SqlDataReader reader = commandS.ExecuteReader();
                if (reader.Read())
                {
                    var brand = reader[0].ToString();
                    ViewData["Encrypt"] = $"{brand.Substring(0, 15)}...";
                    var decrypted = DecryptText(brand, keyVaultClient, KEYID);
                    ViewData["Decrypt"] = decrypted;
                }
                reader.Close();
                #endregion

                connection.Close();

                #endregion

                #region Authenticcation

                var token = _httpContext.Request.Headers["X-MS-TOKEN-AAD-ID-TOKEN"];

                ViewData["RemoteIP"] = _httpContext.Connection.RemoteIpAddress.ToString();
                if (token.Count > 0)
                {
                    ViewData["TokenId"] = $"{token.ToString().Substring(0, 15)}...";
                    var accessToken = _httpContext.Request.Headers["X-MS-TOKEN-AAD-ACCESS-TOKEN"];
                    ViewData["AccessToken"] = $"{accessToken.ToString().Substring(0, 15)}...";
                    ViewData["PrincipleName"] = _httpContext.Request.Headers["X-MS-CLIENT-PRINCIPAL-NAME"];
                    ViewData["TokenIdFull"] = token;

                    JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                    var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
                    //You must validate the token to consider the requested authenticated
                    //tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                    ViewData["TokenIssuer"] = securityToken.Issuer;
                    ViewData["TokenValidFrom"] = securityToken.ValidFrom;
                    ViewData["TokenValidTo"] = securityToken.ValidTo;
                }
                #endregion
            }
            catch (Exception ex)
            {
                ViewData["Exception"] = $"Something happened: {ex.Message}";
            }
            return Page();
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
    }
}