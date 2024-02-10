using L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels;

namespace L3T.Infrastructure.Helpers.CommonModel
{
    public class ApiResponse
    {
        public string  Status { get; set; }
        public int  StatusCode { get; set; }
        public string  Message { get; set; }
        public object  Data { get; set; }
    }

    public class ApiResponseForTicketListAndCount
    {
        public int AllCount { get; set; }
        public List<GetAllPendingTicketByAssignUserResponseModel> AllData { get; set; }

        public int  PendingCount { get; set; }
        public List<GetAllPendingTicketByAssignUserResponseModel> PendingData { get; set; }

        public int CloseCount { get; set; }
        public List<GetAllPendingTicketByAssignUserResponseModel> CloseData { get; set; }

        public int ProcessingCount { get; set; }
        public List<GetAllPendingTicketByAssignUserResponseModel> ProcessingData { get; set; }
    }
} 