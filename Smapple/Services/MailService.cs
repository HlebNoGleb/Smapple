namespace Smapple.Services;

public class MailService
{
    public async Task<bool> SendVerificationMail(string name, string email, string url)
    {
        var response =await new HttpClient().PostAsync("https://api.emailjs.com/api/v1.0/email/send", JsonContent.Create(new
        {
            service_id = "service_9i13b62",
            template_id = "template_szcp0to",
            user_id = "Nyf5SggNyCFbH2rUQ",
            template_params = new
            {
                to_name = name,
                ver_url = url,
                to_email = email,
            },
            accessToken = "YYW-ky7OtiKqV1pV3bI2a"
        }));

        return response.IsSuccessStatusCode;
    }
}