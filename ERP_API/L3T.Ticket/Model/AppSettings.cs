namespace L3T.Ticket.Model
{
    public class AppSettings
    {
        public ConnectionStrings ConnectionStrings { get; set; }

        public string AllowedHosts { get; set; }

        public Serilog Serilog { get; set; }

        public string[] CORSTrustedOrigins { get; set; }

        public Security Security { get; set; }
        public QueueSetting QueueSetting { get; set; }
        public string IdentityServerAddress { get; set; }

        public Elk Elk { get; set; }
        public Redis Redis { get; set; }
    }

    public class ConnectionStrings
    {
        public string MSSQL { get; set; }
    }

    public class Override
    {
        public string Microsoft { get; set; }
        public string System { get; set; }
    }

    public class MinimumLevel
    {
        public string Default { get; set; }
        public Override Override { get; set; }
    }

    public class Serilog
    {
        public MinimumLevel MinimumLevel { get; set; }
        public List<string> Enrich { get; set; }
        public string LogFileName { get; set; }
    }

    public class Security
    {
        public string EncDecHelperKey { get; set; }

        public string TokenSecret { get; set; }
    }

    public class QueueSetting
    {
        public string HostName { get; set; }

        public string VirtualHost { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string QueueName { get; set; }
    }
    public class Elk
    {
        public string Elasticsearch { get; set; }

        public string Kibana { get; set; }
        public string Logstash { get; set; }

    }
    public class Redis
    {
        public string Address { get; set; }
        public string InstanceName { get; set; }
    }
}
