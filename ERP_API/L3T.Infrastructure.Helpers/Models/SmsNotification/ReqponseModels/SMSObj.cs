namespace L3T.Infrastructure.Helpers.Models.SmsNotification.ReqponseModels;

public class Data
{
    public string sender { get; set; }
    public string receiver { get; set; }
    public string received_date { get; set; }
    public string message { get; set; }
    public string udh_hex { get; set; }
}

public class Root
{
    public List<SMSObj> SMSObj { get; set; }
}

public class SMSObj
{
    public string messageID { get; set; }
    public Data Data { get; set; }
}