// See https://aka.ms/new-console-template for more information


using System.Security.Cryptography;
using System.Text;
using Main;

//Console.WriteLine(JwtHelper.GenerateJwToken(266,"YuanBo521"));
//return;

var generateJwtToken = JwtHelper.GenerateJwToken(266, "YuanBo521");
var a = JwtHelper.ValidateToken(generateJwtToken,out var payload);
if (a)
{
    Console.WriteLine($"aId:{payload["aId"]} username:{payload["username"]} SId:{payload["SId"]}");
    return;
}
Console.WriteLine(a);


//PKCS#1
// using (RSA rsa = RSA.Create())
// {
//     //一般长度通常为2048或者4096，密钥越长、安全性越高、性能越差
//     rsa.KeySize = 2048;
//     var exportRsaPublicKey = rsa.ExportRSAPublicKey();
//     var exportRsaPrivateKey = rsa.ExportRSAPrivateKey();
//     var base64PublicString = Convert.ToBase64String(exportRsaPublicKey);
//     var base64PrivateString = Convert.ToBase64String(exportRsaPrivateKey);
//     Console.WriteLine($"Public key:{base64PublicString}");
//     Console.WriteLine($"Private key:{base64PrivateString}");
// }
// //PKCS#8
// using (RSA rsa = RSA.Create())
// {
//     //一般长度通常为2048或者4096，密钥越长、安全性越高、性能越差
//     rsa.KeySize = 2048;
//     var exportRsaPublicKey = rsa.ExportSubjectPublicKeyInfo();
//     var exportRsaPrivateKey = rsa.ExportPkcs8PrivateKey();
//     var base64PublicString = Convert.ToBase64String(exportRsaPublicKey);
//     var base64PrivateString = Convert.ToBase64String(exportRsaPrivateKey);
//     Console.WriteLine($"PKCS#8 Public key:{base64PublicString}");
//     Console.WriteLine($"PKCS#8 Private key:{base64PrivateString}");
// }
//
// //PKCS#8 加密私钥
// using (RSA rsa = RSA.Create())
// {
//     //一般长度通常为2048或者4096，密钥越长、安全性越高、性能越差
//     rsa.KeySize = 2048;
//     //设备加密算法的参数：使用 AES256-CBC 进行加密
//     var pbeParameters = new PbeParameters(PbeEncryptionAlgorithm.Aes256Cbc, HashAlgorithmName.SHA256, iterationCount:10000);
//     //需要保护私钥的密码
//     var password = "yuanbo521";
//     
//     var exportRsaPublicKey = rsa.ExportSubjectPublicKeyInfo();
//     var exportRsaPrivateKey = rsa.ExportEncryptedPkcs8PrivateKey(Encoding.UTF8.GetBytes(password),pbeParameters);
//     var base64PublicString = Convert.ToBase64String(exportRsaPublicKey);
//     var base64PrivateString = Convert.ToBase64String(exportRsaPrivateKey);
//     Console.WriteLine($"PKCS#8 m Public key:{base64PublicString}");
//     Console.WriteLine($"PKCS#8 m Private key:{base64PrivateString}");
// }
