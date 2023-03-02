namespace ChatWS.Models
{
    public class DbConfig : IDbConfig
    {
        public string Server { get; set; }

        public string Database { get; set; }
    }
}
