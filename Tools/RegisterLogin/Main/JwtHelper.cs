using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace Main;

public static class JwtHelper
{
    private const string PublicKeyPem =
        "MIIBCgKCAQEAvgYN+Gf9DmJXVnPUEmYm9s24eA40M6TRt2CuMe21cZabEu0HCBAfXbLiVJQpb2esoPBmH41rgdT4E+++Xluk0YQCtQYDubSwByRq7PQQLm7ntGzAo/aO58xJYHHSZ8+ZrMQGewCtCC5CFv2O2NmZPbnzQr9PETBfZ+A8iclQFRrDGNMCakGao90YtTZ1/SJDfbJLaES6kmE5e3xdUj4A6C86KDG9zfyI6/hIgpd9ZUtGEPejKwcFYUiH5OPfXvSynQZiQzNMsFNE7ZJi2JP40bAWR42vOkewc7C+PyiZvgPO3iwSo5vClpQ5QESZcFLyF43L1ePLX0lpYI8U+CH3CQIDAQAB";

    private const string PrivateKeyPem = "MIIEowIBAAKCAQEAvgYN+Gf9DmJXVnPUEmYm9s24eA40M6TRt2CuMe21cZabEu0HCBAfXbLiVJQpb2esoPBmH41rgdT4E+++Xluk0YQCtQYDubSwByRq7PQQLm7ntGzAo/aO58xJYHHSZ8+ZrMQGewCtCC5CFv2O2NmZPbnzQr9PETBfZ+A8iclQFRrDGNMCakGao90YtTZ1/SJDfbJLaES6kmE5e3xdUj4A6C86KDG9zfyI6/hIgpd9ZUtGEPejKwcFYUiH5OPfXvSynQZiQzNMsFNE7ZJi2JP40bAWR42vOkewc7C+PyiZvgPO3iwSo5vClpQ5QESZcFLyF43L1ePLX0lpYI8U+CH3CQIDAQABAoIBAEzivEJXKP/p8HatdRBgvsE7qbkB3kNLHAO7nZ/cE387NdGpkB/GDjrmR0d7j2xhIWsY+ekoWmh02E+QwJEDOaQAauv93AIGxvaM2Kq5cdanzx6kpilxeI73jHT2ePDCAebyhcC9HEXkGZ+dxm+dU7CzDWdjasWEsgWgqD4D82Al+grJ5uUrwu0tIhMqKLbUFkgt79AYAcEYOScY5m442+K/ohoPLzJlAu+ku2aBYyrIl8JY4HV7lOtmq86RBAV0U/NGiJcx6yQOgLPYUgqNseU1vAnA2JyBYwjqFqm+9y7Nt/7foKScHdO8VYs8tfuEst21Fu71GpEgHwTn64inRpUCgYEAzk6Ein3WdwIbRQi2Ct4Rog4aUmHtlocpksuGAWb7tJg12jBKeWi3fICTwSYfKL0lGIPS+Hdj6OHZj15fLhqVSZesOFYeOziUC0WAKXU172wpe5q48kZf++rxNtz+spkBrWsfBYyLmzL5sy/cVAUJRxthAGv0cSVzsoN5C21CD6sCgYEA68t5p5hc43/tpQ+Axt18ayNho5qwNjZGd8jDcBNjfPav7WLFvtCQdrwV55eB5rGrpExi8Ct0cGmAzNGU0aSGZkrn6t4ajueh/Cppf8S0D5EH98Z+McjHpeoyQOtOEXOQvAu507t3bht+tRCWmXdZUsLeQGmT0OmIydL9B1hu8BsCgYB2WM5pj9RmgpvYFy4uv8NHvmVVGv4rGrA7mGrwBP6hU6uY1ZjzWqHfVvbrlw3K1e4gyQZOKFb08hJWzyE0lmVLSSmvS5+eA0/Rw3XI0oc1KEwHrvMncD2Biv4CpfWpyGIQ9GFgUoaHak+ZffwbaqQu2ULk9gjMm1pqbkcSygNabwKBgAsq9/gYH04nIPpQYakJlHr+kgFNskrfBzdlKtyEDpI8nNiBdRw0hKMbBW6SnnRutdJyS71UUY+Bb7hDtOi5AiSWJ6XYHynljqaC27xRdLXICLiTjiaNe+c/0GGCw4/QCWreo06D8oQkiTvKLVfXb0OcNyqV1YpvhSsJ8zIF3jtBAoGBAMx0fxW+gKQUqJF9XZtCrwxQZu2wH3GRjhAQb8ZbIPnE8TV1B70tKVTxLEP0/Oasm0jMwfdPytWfOz+w39rfwwxyBr9JZmiPvFf2eRAQAp8wgLh4HJdC6ZNls6KIjb8mVCudW8HntGyU6dJM/IVuY+w+mJIfkAUB+DNtFoZyfQaK";
    private static SigningCredentials _signingCredentials;
    private static TokenValidationParameters _tokenValidationParameters;
    static JwtHelper()
    {
        var rsa = RSA.Create();
        rsa.ImportRSAPublicKey(Convert.FromBase64String(PublicKeyPem),out _);
        rsa.ImportRSAPrivateKey(Convert.FromBase64String(PrivateKeyPem),out _);
        _signingCredentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);
        //创建 TokenValidationParameters 对象，用于配置验证参数
        _tokenValidationParameters=new TokenValidationParameters()
        {
            ValidateLifetime = false,                  //禁止令牌考验时间是否过去
            ValidateIssuer = true,                     //验证发行者
            ValidateAudience = true,                   //验证受众
            ValidateIssuerSigningKey = true,           //验证签名密钥
            ValidIssuer = "YuanBo",                    //有效的发行者
            ValidAudience = "YuanBo",                  //有效的受众
            IssuerSigningKey = new RsaSecurityKey(rsa),//RSA公钥作为签名密钥
        };
    }

    public static string GenerateJwToken(int aId,string username)
    {
        var jwtPayload = new JwtPayload()
        {
            { "aId", aId },
            { "username", username },
            { "SId", "YuanBo" }
        };
        var jwtSecurityToken = new JwtSecurityToken(
            issuer: "YuanBo",
            audience: "YuanBo",
            claims: jwtPayload.Claims,
            expires: DateTime.UtcNow.AddMilliseconds(3000),
            signingCredentials: _signingCredentials//null
        );
        return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    }

    public static bool ValidateToken(string token,out JwtPayload payload)
    {
        payload = null;
        try
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            jwtSecurityTokenHandler.ValidateToken(token,_tokenValidationParameters,out _);
            payload = jwtSecurityTokenHandler.ReadJwtToken(token).Payload;
            return true;
        }
        catch (SecurityTokenInvalidAudienceException e)
        {
            Console.WriteLine("验证受众失败");
            return false;
        }
        catch (SecurityTokenInvalidIssuerException e)
        {
            Console.WriteLine("验证发行者失败");
            return false;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}