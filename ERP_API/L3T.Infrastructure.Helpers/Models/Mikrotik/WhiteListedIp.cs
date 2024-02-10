namespace L3T.Infrastructure.Helpers.Models.Mikrotik
{
    public class WhiteListedIp
    {
        public long Id { get; set; }
        public string Ip { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}