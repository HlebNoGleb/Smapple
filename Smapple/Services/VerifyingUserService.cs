using Headstart.Common.Helpers;
using Smapple.Models;

namespace Smapple.Services;

public class VerifyingUserService
{
    private const string UrlFormat = "http://localhost:5173/verifying?code={0}";
    
    private readonly MailService mail;

    public VerifyingUserService(MailService mail)
    {
        this.mail = mail;
    }

    public async Task<bool> SendVerificationMail(User user)
    {
        var stringKey = $"{user.Id}-{user.NickName}";

        var encodedKey = AesEncryptionHelper.EncryptString(stringKey);

        var url = string.Format(UrlFormat, encodedKey);

        var emailSended = await mail.SendVerificationMail(user.NickName, user.Email, url);

        return emailSended;
    }

    public int? GetUserIdByCode(string code)
    {
        var decodedKey = AesEncryptionHelper.DecryptString(code);

        return int.TryParse(decodedKey?.Split("-")?.FirstOrDefault(), out var result) 
                ? result 
                : null;
    }
}