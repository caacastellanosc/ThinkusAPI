namespace ThinkusAPI.Configurations
{
    public class AppSettings
    {
        public ConnectionStrings ConnectionStrings { get; set; }
        public InsurancestoreDatabase InsurancestoreDatabase { get; set; }
        public JwtConfig Jwt { get; set; }
        public Logging Logging { get; set; }
        public string AllowedHosts { get; set; }
    }

    public class ConnectionStrings
    {
        public string mongoConnection { get; set; }
    }

    public class InsurancestoreDatabase
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string InsuranceCollectionName { get; set; }
    }

    public class JwtConfig
    {
        public string ValidAudience { get; set; }
        public string ValidIssuer { get; set; }
        public string Secret { get; set; }
    }

    public class Logging
    {
        public LogLevel LogLevel { get; set; }
    }
}
