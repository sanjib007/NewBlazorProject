using L3T.Infrastructure.Helpers.DataContext.ScheduleDBContext;
using L3T.Infrastructure.Helpers.Interface.Schedule;
using L3T.Infrastructure.Helpers.Models.Schedule;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace L3T.Infrastructure.Helpers.Implementation.Schedule
{
    public class ScheduleService : IScheduleService
    {
        private readonly ScheduleDataContext _contex;
        private readonly ILogger<ScheduleService> _logger;

        public ScheduleService(ScheduleDataContext contex, ILogger<ScheduleService> logger)
        {
            _contex = contex;
            _logger = logger;
        }

        public async Task<List<Test>> GetAllTestQuery()
        {
            try
            {
                var item = await _contex.Test.ToListAsync();
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("An error occurred when fetching the GetAllTestQuery method  Exception: " + ex.Message.ToString());
            }
            return null;
        }

        public async Task<List<NetworkInformation>> GetInactiveHydraCustomer(DateTime expireDate)
        {
            _logger.LogInformation("Fetching the Hydra_Customer Data by date" + expireDate);
            try
            {
                //List<tbl_Hydra_Customer> hydraLists = await _contex.tbl_Hydra_Customer.Where(t => t.ServiceExpireDate >= expireDate && t.CustomerStatus == "Active").ToListAsync();
                List<NetworkInformation> item = await _contex.NetworkInformation.Where(t => t.ServiceExpireDate >= expireDate && t.CustomerStatus == "Active" && t.ServiceStatus == "Inactive" && t.Status == 0).ToListAsync();
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("An error occurred when fetching the NIDQueue by id:" + expireDate + " Exception: " + ex.Message.ToString());
            }
            return null;
        }
        public async Task<bool> UpdateInactiveHydraCustomer(string customerid)
        {
            string oradb = "Data Source=(DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = 123.200.0.67)(PORT = 1521)) (CONNECT_DATA = (SERVER = DEDICATED) (SERVICE_NAME = hydrast))); User Id=Ais_NET;Password=OxcjtkzYWxx125PnyQ3mqrR0;Connection Timeout=1800; ";

            OracleConnection con = new OracleConnection(oradb);

            OracleCommand cmd = new OracleCommand();
            cmd.Connection = con;
            con.Open();
            try
            {


                cmd.CommandText = @"BEGIN
                          FOR rl IN (
                            SELECT N_SUBJECT_ID
                            FROM SI_V_SUBJECTS
                            WHERE VC_CODE = '" + customerid + "'  ) LOOP    SI_SUBJECTS_PKG.CHANGE_STATE(" +
                            "      num_N_SUBJECT_ID => rl.N_SUBJECT_ID," +

                            "      num_N_SUBJ_STATE_ID => SYS_CONTEXT('CONST', 'SUBJ_STATE_Off'));     COMMIT;" +
                            "  END LOOP; END;";
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                con.Close();

                //query = @"UPDATE [WFA2].[dbo].[tbl_Hydra_Customer] SET UpdateStatus=1  where CustomerID='" + customerid + "'";
                //DataProcess.ExecuteQuery(ConnectionStr, query);

                return true;
            }
            catch (Exception ex)
            {

                //query = @"UPDATE [WFA2].[dbo].[tbl_Hydra_Customer] SET UpdateStatus=3  where CustomerID='" + customerid + "'";
                //DataProcess.ExecuteQuery(ConnectionStr, query);

                con.Close();
                _logger.LogInformation("Error saving SaveAllPaymentlog For phoneNumber - : " + customerid + " Exception : " + ex.Message.ToString());
                return false;
            }


        }
        public async Task<NetworkInformation> UpdateHydra_CustomerStatus(string customerid, int status)
        {
            _logger.LogInformation("Fetching Hydra_Customer by customerid: " + customerid);
            try
            {
                var items = _contex.NetworkInformation.Where(c => c.CustomerID == customerid).FirstOrDefault();
                if (items != null)
                {
                    items.Status = status;

                    _contex.NetworkInformation.Update(items);
                    await _contex.SaveChangesAsync();
                    return items;
                }
                return items;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("An error occurred when fetching the Hydra_Customer by ParamName:" + customerid + " Exception: " + ex.Message.ToString());
                return null;
            }
        }
    }
}
