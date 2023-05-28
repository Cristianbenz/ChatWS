namespace ChatWS.Models.Responses
{
    public class SignInResponse
    {
        public int Id { get; set; }
        public string Token { get; set; }

        public string Name { get; set; }

        public string Avatar { get; set; }
    }
}
