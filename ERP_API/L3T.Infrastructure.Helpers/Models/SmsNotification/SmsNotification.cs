namespace L3T.Infrastructure.Helpers.Models.SmsNotification;

public class SmsNotification
{
    public long ID { get; set; }
    public long MessageId { get; set; }
    public string? Sender { get; set; }
    public string? Reciver { get; set; }
    public string? Rcv_Date_Bl { get; set; }
    public string? SMS { get; set; }
    public string? Cust_ID { get; set; }
    public string? TKT_CR { get; set; }
    public string? NewComment { get; set; }
    public string? AddBy { get; set; }
    public string? Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int TtyCount { get; set; }

    public string? frm { get; set; }
    public string? rcv_date { get; set; }
    public string? rcv_time { get; set; }
    public string? msg { get; set; }
    public int? stat { get; set; }
    public int? call { get; set; }
    public int? snd_sms { get; set; }
    public int? call_bk { get; set; }
    public string? sys_time { get; set; }
    public string? called { get; set; }
    public string? link_stat { get; set; }
    public string? address_by { get; set; }
    public string? ticket_cr { get; set; }
    public string? comment { get; set; }
    public DateTime? date_convert { get; set; }

    public int? acknowledge { get; set; }
    public int? sms_replied { get; set; }
    public int? VIP { get; set; }
    public string? replied_msg { get; set; }
    public DateTime? my_date { get; set; }
    public string? RSM_STAT { get; set; }
    public string? New_Comment { get; set; }
}