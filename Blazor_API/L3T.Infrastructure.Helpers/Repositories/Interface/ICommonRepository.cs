using L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.RequestModel;
using L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Repositories.Interface
{
    public interface ICommonRepository
    {
        //Task<List<string>> GetAllDepartment();
        //Task<List<string>> SearchEmployee(string searchText);
        Task<List<SP_AssignEmployeeListResponse>> GetAllAssignEmployeeFromDB(AssignEmployeeListReqModel FilterReqModel);


    }
}
