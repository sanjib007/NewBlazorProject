using L3T.Infrastructure.Helpers.DataContext;
using L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.Entities;
using L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.RequestModel;
using L3T.Infrastructure.Helpers.Models.ChangeRequest.v1.ResponseModel;
using L3T.Infrastructure.Helpers.Models.CommonModel;
using L3T.Infrastructure.Helpers.Pagination;
using L3T.Infrastructure.Helpers.Repositories.Interface.ChangeRequest.v1;
using L3T.Infrastructure.Helpers.Services.ServiceInterface;
using L3T.Infrastructure.Helpers.Services.ServiceInterface.Service.Interface.ChangeRequest.v1;
using L3T.Utility.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace L3T.Infrastructure.Helpers.Services.ServiceImplementation.Service.Implementation.ChangeRequest.v1
{
    public class TempChangeRequestedInfoService : ITempChangeRequestedInfoService
    {
        private readonly ITempChangeRequestedInfoRepository _tempChangeRequestedInfoRepository;
        private readonly ChangeRequestDataContext _context;
        private readonly ILogger<TempChangeRequestedInfoService> _logger;
        private readonly IHostingEnvironment _environment;
        private readonly ICRRequestResponseService _cRRequestResponseService;
        private readonly ICrAttatchedFilesRepository _crAttatchedFilesRepository;

       
        public TempChangeRequestedInfoService(
            ITempChangeRequestedInfoRepository tempChangeRequestedInfoRepository,  
            IHostingEnvironment environment, 
            ChangeRequestDataContext context,
            ILogger<TempChangeRequestedInfoService> logger, 
            ICRRequestResponseService cRRequestResponseService,
            ICrAttatchedFilesRepository crAttatchedFilesRepository)

        {
            _tempChangeRequestedInfoRepository = tempChangeRequestedInfoRepository;            
            _logger = logger;
            _context = context;
            _environment = environment;
            _cRRequestResponseService = cRRequestResponseService;
            _crAttatchedFilesRepository = crAttatchedFilesRepository;
        }
        
        
        public async Task<ApiResponse> AddChangeRequirement(AddTempChangeRequestModel requestModel, string userId, string ip)
        {
             

            var methodName = "TempChangeRequestedInfoService/AddChangeRequirement";
            TempChangeRequestedInfo crInfo = new TempChangeRequestedInfo();
            try 
            {
                crInfo = await _tempChangeRequestedInfoRepository.FirstOrDefaultAsync(x => x.EmployeeId == userId);

                if (crInfo != null)
                {
                    if (requestModel.StepNo == "1")
                    {
                        crInfo.Subject = requestModel.Subject;
                        crInfo.ChangeRequestFor = requestModel.ChangeRequestFor;
                    }
                    else if (requestModel.StepNo == "2")
                    {
                        crInfo.ChangeFromExisting = requestModel.ChangeFromExisting;
                        crInfo.ChangeToAfter = requestModel.ChangeToAfter;
                        crInfo.ChangeImpactDescription = requestModel.ChangeImpactDescription;
                        crInfo.Justification = requestModel.Justification;
                    }
                    else if (requestModel.StepNo == "3")
                    {
                        crInfo.AddReference = requestModel.AddReference;
                        if (requestModel.AttachFile != null)
                        {
                            var crFile = new CrAttatchedFile()
                            {
                                CrId = crInfo.Id,
                                FileName = await uploadImage(requestModel.AttachFile, userId, crInfo.Id, ip),
                                CreatedAt = DateTime.Now,
                            };
                            await _crAttatchedFilesRepository.CreateAsync(crFile);
                        }
                        
                    }
                    else if (requestModel.StepNo == "4")
                    {
                        crInfo.LevelOfRisk = requestModel.LevelOfRisk;
                        crInfo.LevelOfRiskDescription = requestModel.LevelOfRiskDescription;
                        crInfo.AlternativeDescription = requestModel.AlternativeDescription;
                    }
                    
                    crInfo.StepNo = crInfo.StepNo.Contains(requestModel.StepNo) == false ? crInfo.StepNo + "," + requestModel.StepNo : crInfo.StepNo;
                    crInfo.LastModifiedAt = DateTime.Now;
                    _tempChangeRequestedInfoRepository.Update(crInfo);
                    _context.SaveChanges();

                    var Message = "Change Request is updated successfully!";

                    return await _cRRequestResponseService.CreateResponseRequest(requestModel, await SetData(crInfo), ip, methodName, userId, Message);

                     
                }
                else
                {
                    var requestDbModel = new TempChangeRequestedInfo()
                    {
                        Subject = requestModel.Subject,
                        ChangeRequestFor = requestModel.ChangeRequestFor,
                        RequestorName = requestModel.RequestorName,
                        DepartName = requestModel.DepartName,
                        RequestorDesignation = requestModel.RequestorDesignation,
                        EmployeeId = requestModel.EmployeeId,
                        ContactNumber = requestModel.ContactNumber,
                        Email = requestModel.Email,
                        StepNo = requestModel.StepNo,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = userId

                    };
                    await _tempChangeRequestedInfoRepository.CreateAsync(requestDbModel);
                    var isExecuted = await _context.SaveChangesAsync();
                    if (isExecuted > 0)
                    {
                        if (crInfo == null)
                        {
                            crInfo = await _tempChangeRequestedInfoRepository.FirstOrDefaultAsync(x => x.EmployeeId == userId);
                        }
                        var Message = "Change Request save successfully!";

                        return await _cRRequestResponseService.CreateResponseRequest(requestModel, await SetData(crInfo), ip, methodName, userId, Message);

                         
                    }
                }                
                _logger.LogInformation("Add ChangeRequest : " + requestModel);

                throw new Exception("Change Request Save some problem.");
            }
            catch (Exception ex)
            {
                
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _cRRequestResponseService.CreateResponseRequest(null, ex, ip, methodName, userId,"Error", ex.Message.ToString());
                 
            }

        }

        private async Task<TempCrInfoResponseModel> SetData(TempChangeRequestedInfo crInfo)
        {
            var allFiles = await _crAttatchedFilesRepository.FindAsync(x => x.CrId == crInfo.Id);
            var crInfoResponce = new TempCrInfoResponseModel()
            {
                Id = crInfo.Id,
                AddReference = crInfo.AddReference,
                AlternativeDescription = crInfo.AlternativeDescription,
                Subject = crInfo.Subject,
                RequestorName = crInfo.RequestorName,
                DepartName = crInfo.DepartName,
                EmployeeId = crInfo.EmployeeId,
                RequestorDesignation = crInfo.RequestorDesignation,
                ContactNumber = crInfo.ContactNumber,
                Email = crInfo.Email,
                ChangeRequestFor =  crInfo.ChangeRequestFor,
                ChangeFromExisting = crInfo.ChangeFromExisting,
                ChangeToAfter = crInfo.ChangeToAfter,
                ChangeImpactDescription = crInfo.ChangeImpactDescription,
                Justification = crInfo. Justification,
                LevelOfRisk = crInfo.LevelOfRisk,
                LevelOfRiskDescription = crInfo.LevelOfRiskDescription,
                AttachFile = crInfo.AttachFile,
                StepNo = crInfo.StepNo,
                CreatedBy = crInfo.CreatedBy,
                CreatedAt = crInfo.CreatedAt,
                LastModifiedBy = crInfo.LastModifiedBy,
                LastModifiedAt = crInfo.LastModifiedAt,
                AttachedFiles = allFiles

            };
            return crInfoResponce;
        }

        

        public async Task<ApiResponse> ChangeRequirementDelete(long Id, string userId, string ip)
        {
             
            var methodName = "TempChangeRequestedInfoRepository/ChangeRequirementDelete";
            try
            {

                var res = await _tempChangeRequestedInfoRepository.FirstOrDefaultAsync(x => x.Id == Id);
                if (res != null)
                {

                    await _tempChangeRequestedInfoRepository.Delete(res);
                    await _context.SaveChangesAsync();

					var Message = "Data Deleted Successfully.";

					return await _cRRequestResponseService.CreateResponseRequest(Id, res, ip, methodName, userId, Message);
                }
                else
                {
                    _logger.LogInformation("ChangeRequirementDelete : " + Id);
                    throw new Exception("Data not found.");

                }
                 
            }
            catch (Exception ex)
            {
                
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _cRRequestResponseService.CreateResponseRequest(Id, ex, ip, methodName, userId,"Error", ex.Message.ToString());
                 
            }
        }

        public async Task<ApiResponse> GetChangeRequirementById(long Id, string userId, string ip)
        {
             
            var methodName = "TempChangeRequestedInfoRepository/GetChangeRequirementById";
            try {

                var res = await _tempChangeRequestedInfoRepository.FirstOrDefaultAsync(x => x.Id == Id);
                if (res != null)
                {
                    var crResponse = new TempCrInfoResponseModel();
                    crResponse = await SetData(res);
                    
                    return await _cRRequestResponseService.CreateResponseRequest(Id, crResponse, ip, methodName, userId, "Ok");
                }
                else
                {
                    _logger.LogInformation("GetChangeRequirementById : " + Id);
                    throw new Exception("Data not found.");
                    
                }

            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _cRRequestResponseService.CreateResponseRequest(null, ex, ip, methodName, userId,"Error", ex.Message.ToString());
                 
            }
            
        }

        private async Task<string> uploadImage(IFormFile AttachFile, string userId, long crId, string ip)
        {
            var methodName = "TempChangeRequestedInfoRepository/uploadImage";
            try
            {
                var extentionArray = CommonHelper.FileExtentionList.Split(",");
                var imageName = AttachFile.FileName;
                var imageExtension = imageName.Split('.');
                var extension = imageExtension[1];
                var matchExtention = extentionArray.FirstOrDefault(x => x.Trim() == extension);
                if (string.IsNullOrEmpty(matchExtention))
                {
                    throw new Exception("File format is not match");
                }
                if (!Directory.Exists(_environment.WebRootPath + "\\uploads\\"))
                {
                    Directory.CreateDirectory(_environment.WebRootPath + "\\uploads\\");
                }
                if (string.IsNullOrWhiteSpace(_environment.WebRootPath))
                {
                    _environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                }
                
                Guid gUId = Guid.NewGuid();
                string imgName = gUId.ToString() + "_" + userId + "_" + crId.ToString() + "." + extension;
                using (FileStream filestream = System.IO.File.Create(_environment.WebRootPath + "\\uploads\\" + imgName))
                {
                    AttachFile.CopyTo(filestream);
                    filestream.Flush();
                }
                await _cRRequestResponseService.CreateResponseRequest(AttachFile, imgName, ip, methodName, userId, "Ok");
                return imgName;
            }
            catch (Exception ex)
            {

                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                await _cRRequestResponseService.CreateResponseRequest(AttachFile, ex, ip, methodName, userId,"Error", ex.Message.ToString());
                return string.Empty;
            }
        }

        public async Task<ApiResponse> UpdateChangeRequirement(AddTempChangeRequestModel ReqModel, string userId, string ip)
        {
             
            var methodName = "TempChangeRequestedInfoRepository/UpdateChangeRequirement";
            try
            {
                var getData = await _tempChangeRequestedInfoRepository.FirstOrDefaultAsync(x => x.Id == ReqModel.Id);
                if (getData != null)
                {

                    //var imageName = ReqModel.AttachFile.FileName;
                    //var imageExtension = imageName.Split('.', ' ');
                    //var extension = imageExtension[1];

                    //if (extension != "jpg" || extension != "jpeg" || extension != "png")
                    //{

                    //    OutputResponse.StatusCode = 111;
                    //    OutputResponse.Status = "only jpg/jpeg/PNG formats are allowed";
                    //    return OutputResponse;
                    //}


                    if (!Directory.Exists(_environment.WebRootPath + "\\uploads\\"))
                    {
                        Directory.CreateDirectory(_environment.WebRootPath + "\\uploads\\");
                    }
                    if (string.IsNullOrWhiteSpace(_environment.WebRootPath))
                    {
                        _environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    }

                    using (FileStream filestream = System.IO.File.Create(_environment.WebRootPath + "\\uploads\\" + "FileDoc" + ReqModel.AttachFile.FileName))
                    {
                        ReqModel.AttachFile.CopyTo(filestream);
                        filestream.Flush();

                    }

                    getData.Subject = ReqModel.Subject;
                    getData.RequestorName = ReqModel.RequestorName;
                    getData.RequestorDesignation = ReqModel.RequestorDesignation;
                    getData.DepartName = ReqModel.DepartName;                   
                    getData.EmployeeId = ReqModel.EmployeeId;
                    getData.ContactNumber = ReqModel.ContactNumber;
                    getData.Email = ReqModel.Email;
                    getData.ChangeRequestFor = ReqModel.ChangeRequestFor;
                    getData.ChangeFromExisting = ReqModel.ChangeFromExisting;
                    getData.ChangeToAfter = ReqModel.ChangeToAfter;
                    getData.ChangeImpactDescription = ReqModel.ChangeImpactDescription;
                    getData.Justification = ReqModel.Justification;
                    getData.LevelOfRisk = ReqModel.LevelOfRisk;
                    getData.LevelOfRiskDescription = ReqModel.LevelOfRiskDescription;
                    getData.AlternativeDescription = ReqModel.AlternativeDescription;
                    getData.AddReference = ReqModel.AddReference;
                    getData.AttachFile = ReqModel.AttachFile.FileName;
                    getData.StepNo = ReqModel.StepNo;
                    getData.LastModifiedBy = userId;
                    getData.LastModifiedAt = DateTime.UtcNow;

                    _tempChangeRequestedInfoRepository.UpdateLatest(getData);
                    if (await _context.SaveChangesAsync() > 0)
                    {
                        var Message = "ChangeRequest update successfully!";
                        return await _cRRequestResponseService.CreateResponseRequest(ReqModel, getData, ip, methodName, userId, Message);
                    }
                    throw new Exception("something is wrong.");
                }
                else
                {

                    _logger.LogInformation("UpdateChangeRequirement : " + ReqModel);
                    throw new Exception("Data not found.");
                }
                 

            }
            catch (Exception ex)
            {
                
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _cRRequestResponseService.CreateResponseRequest(null, ex, ip, methodName, userId,"Error", ex.Message.ToString());
                 
            }
        }


        public async Task<ApiResponse> UncompleteChangeRequest(string userId, string ip)
        {
            var methodName = "TempChangeRequestedInfoRepository/UncompleteChangeRequest";
            try
            {

                var res = await _tempChangeRequestedInfoRepository.FirstOrDefaultAsync(x => x.EmployeeId == userId);
                return await _cRRequestResponseService.CreateResponseRequest(null, await SetData(res), ip, methodName, userId, "Ok");

            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _cRRequestResponseService.CreateResponseRequest(null, ex, ip, methodName, userId,"Error", ex.Message.ToString());
                 
            }

        }

        public async Task<ApiResponse> RemovedFile(RemovedFileRequestModel model, string userId, string ip)
        {
            var methodName = "TempChangeRequestedInfoRepository/RemovedFile";
            try
            {
                var getCr = await _tempChangeRequestedInfoRepository.FindFirstOrDefaultAsync(x => x.EmployeeId == userId);
                var aFile = await _crAttatchedFilesRepository.FindFirstOrDefaultAsync(x => x.CrId == getCr.Id && x.FileName == model.fileName);

                if(aFile != null)
                {
                    await _crAttatchedFilesRepository.Delete(aFile);
                    await _context.SaveChangesAsync();
                }

                var allFiles = await _crAttatchedFilesRepository.FindAsync(x => x.CrId == getCr.Id);
                return await _cRRequestResponseService.CreateResponseRequest(model, allFiles, ip, methodName, userId, "File Deleted Successfull.");

                 

            }
            catch (Exception ex)
            {
                _logger.LogInformation(@$"Exception {DateTime.Now} : {JsonConvert.SerializeObject(ex)}");
                return await _cRRequestResponseService.CreateResponseRequest(model, ex, ip, methodName, userId,"Error", ex.Message.ToString());
                 
            }
        }
    }
}
