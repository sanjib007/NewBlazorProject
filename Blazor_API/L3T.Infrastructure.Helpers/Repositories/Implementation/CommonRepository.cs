using L3T.Infrastructure.Helpers.DataContext;
using L3T.Infrastructure.Helpers.Repositories.Interface;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.ResponseModel;
using L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.RequestModel;

namespace L3T.Infrastructure.Helpers.Repositories.Implementation
{
    public class CommonRepository : ICommonRepository
    {
        //private readonly LnkDbContext _lnkDbContext;
        private readonly ChangeRequestDataContext _context;
        public CommonRepository(ChangeRequestDataContext context
            //LnkDbContext lnkDbContext
            )
        {
            //_lnkDbContext = lnkDbContext;
            _context = context;
        }

        //public async Task<List<string>> GetAllDepartment()
        //{
        //    var strSql = $@"SELECT Dept AS Department from dbo.Emp_Details group by Dept order by Dept";
        //    var allDepartment = await _lnkDbContext.GetAllDepartment.FromSqlRaw(strSql).Select(x=> x.Department).ToListAsync();
        //    return allDepartment;
        //}

        //public async Task<List<string>> SearchEmployee(string searchText)
        //{
        //    var strSql = $@"SELECT CONCAT(EmpID, ' - ', EmpName, ' - ', Email) as Employee from [LNK].dbo.Emp_Details WHERE 1=1 
        //                    EmpStatus='N' and (EmpID like '%{searchText}%' OR EmpName like '%{searchText}%' OR Email like '%{searchText}%')";
        //    var allEmployee = await _lnkDbContext.SearchEmployeeModel.FromSqlRaw(strSql).Select(x => x.Employee).ToListAsync();
        //    return allEmployee;
        //}

        public async Task<List<SP_AssignEmployeeListResponse>> GetAllAssignEmployeeFromDB(AssignEmployeeListReqModel FilterReqModel)
        {
            string sql = "EXEC SP_AssignEmployeeListWithLastApproximateWorkingDay " +
                "'" + FilterReqModel.CrId + "'," +
                "'" + FilterReqModel.EmpId + "'," +
                "'" + FilterReqModel.PageNumber + "'," +
                "'" + FilterReqModel.PageSize + "'";

            var res = await _context.AssignEmployeeListResponse.FromSqlRaw(sql).ToListAsync();
            return res;
        }
    }
}
