namespace Dexpa.ApiClient
{
    public class ApiCredentials
    {
        public string ApiUrl { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public ApiCredentials(string url, string login, string password)
        {
            ApiUrl = url;
            Login = login;
            Password = password;
        }
    }
}
