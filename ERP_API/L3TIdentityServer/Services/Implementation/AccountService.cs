using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Serilog;
using Microsoft.AspNetCore.Identity;
//using DAB.Infrastructure.IdentityServer.API.Models;
using System.Security.Claims;
//using DAB.Infrastructure.IdentityServer.API.Models.Account;
//using DAB.Infrastructure.Helpers.MessageBroker.Models.Account;
using Microsoft.EntityFrameworkCore;
//using DAB.Infrastructure.IdentityServer.DataLayer.Models;
//using DAB.Infrastructure.IdentityServer.DataLayer;
using Microsoft.Extensions.Options;
//using DAB.Infrastructure.IdentityServer.API.Data;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
//using DAB.Infrastructure.Helpers.SecurityHelper;
using AutoMapper;
using IdentityModel.Client;
using System.Net.Http;
using static IdentityModel.OidcConstants;
using L3TIdentityServer.Models.Account;
using L3TIdentityServer.Models;
using Microsoft.Extensions.Logging;
using L3TIdentityServer.DataAccess;
//using MassTransit;
//using DAB.Infrastructure.Helpers.MessageBroker.Models.Notification;
//using DAB.Infrastructure.Helpers.MessageBroker.Models;

namespace L3TIdentityServer.Services.Implementation
{
    public class AccountService : IAccountService
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        ////private readonly IPublishEndpoint endpoint;
        //private readonly ILogger logger;
        private readonly L3TIdentityContext context;
        //private readonly AppSettings appSettings;
        ////private readonly EncryptionDecryptionHelper encryptionDecryptionHelper;
        private readonly RoleManager<IdentityRole> _roleManager;
        //private readonly IHttpClientFactory httpClientFactory;
        private readonly IMapper mapper;
        //private readonly IRequestClient<MB_MessageSend> messageSendClient;


        public AccountService(SignInManager<IdentityUser> signInManager,
                              UserManager<IdentityUser> userManager,
                              //IOptions<AppSettings> options,
                              ////IPublishEndpoint endpoint,
                              //ILogger logger,
                              L3TIdentityContext dbContext,
                              ////EncryptionDecryptionHelper encryptionDecryptionHelper,
                              RoleManager<IdentityRole> roleManager,
                              //IHttpClientFactory httpClientFactory
                              IMapper mapper
            //IRequestClient<MB_MessageSend> messageSendClient
            )
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
            ////this.endpoint = endpoint;
            //appSettings = options.Value;
            //this.logger = logger;
            context = dbContext;

            ////this.encryptionDecryptionHelper = encryptionDecryptionHelper;
            this._roleManager = roleManager;
            //this.httpClientFactory = httpClientFactory;
            this.mapper = mapper;
            //this.messageSendClient = messageSendClient;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var validationResponse = LoginValidation(request);
            if (!validationResponse.Succeeded)
                return validationResponse;

            var response = new LoginResponse()
            {
                Succeeded = false,
                Message = AccountOptions.InvalidCredentialsErrorMessage,
                HttpStatusCode = System.Net.HttpStatusCode.BadRequest
            };

            try
            {
                var user = await _signInManager.UserManager.FindByNameAsync(request.Username).ConfigureAwait(false);
                //var userExists = await _userManager.FindByNameAsync(request.Username);
                if (user == null)
                {
                    response.Message = AccountOptions.InvalidCredentialsErrorMessage;
                    return response;
                }

                if ((user.LockoutEnd != null && user.LockoutEnd.Value.Date >= DateTime.Now))
                {
                    ////logger.Error(AccountOptions.AccountLocked);
                    ////logger.Error(AccountOptions.AccountLocked);
                    response.Message = AccountOptions.AccountLocked;
                    return response;
                }

                //if (!user.IsActive)
                //{
                //    ////logger.Error(AccountOptions.AccountLocked);
                //    response.Message = AccountOptions.AccountLocked;
                //    return response;
                //}

                var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberLogin, lockoutOnFailure: true).ConfigureAwait(false);
                if (result.Succeeded)
                {

                    if (!user.PhoneNumberConfirmed)
                    {
                        ////logger.Error(AccountOptions.PhoneNumberNotConfirm);
                        response.Message = AccountOptions.PhoneNumberNotConfirm;
                        response.PhoneNumberNotConfirmed = true;
                        response.PhoneNumber = user.PhoneNumber;
                        return response;
                    }

                    // Get the Dashboard App Client and its allowed scopes
                    // var clientId = "dashboard";
                    // var clientScopes = String.Join(" ", Data.Clients.identityClients.Where(c => c.ClientId == clientId).SelectMany(c => c.AllowedScopes));
                    // Get the token requesting client
                    // var tokenClient = httpClientFactory.CreateClient("TokenClient");
                    // Get the token using OAuth2 password grant
                    /* var tokenResponse = await new HttpClient().RequestPasswordTokenAsync(new PasswordTokenRequest() {
                        Address = "https://localhost:1003/connect/token",
                        ClientId = clientId,
                        Scope = clientScopes,
                        UserName = request.Username,
                        Password = request.Password
                    }); */

                    //response.AccessToken = await GetJWTToken(user, request.RememberLogin);
                    // response.RefreshToken = tokenResponse.RefreshToken;
                    // response.ExpireDate = DateTime.Now.AddSeconds(tokenResponse.ExpiresIn);
                    response.Succeeded = true;
                    response.Claims = await GetUserClaims(user);
                    response.Message = string.Format(AccountOptions.LoginSuccessfully, user.UserName, DateTime.Now);
                    response.HttpStatusCode = System.Net.HttpStatusCode.OK;
                }

                if (result.IsLockedOut)
                {
                    ////logger.Error(AccountOptions.AccountLocked);
                    response.Message = AccountOptions.AccountLocked;
                }
            }
            catch (Exception ex)
            {
                ////logger.Error(ex, string.Format(AccountOptions.RaisedError, DateTime.Now, ex.Message, nameof(LoginAsync)));
                response.Message = ex.Message.ToString();
                response.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
            }

            return response;
        }
        //public async Task<ResponseModel> RegisterAsync(RegisterUserRequest request)
        //{
        //    var validation = UserRegisterValidation(request);

        //    if (!validation.Succeeded)
        //        return validation;

        //    var response = new ResponseModel()
        //    {
        //        Succeeded = false,
        //        HttpStatusCode = System.Net.HttpStatusCode.BadRequest
        //    };

        //    //logger.Information(string.Format(AccountOptions.BeginTransactionRegister, DateTime.Now, request.Username));

        //    var executionStrategy = context.Database.CreateExecutionStrategy();

        //    return await executionStrategy.Execute(async () =>
        //    {
        //        // execute your logic here
        //        using var transaction = context.Database.BeginTransaction();
        //        try
        //        {
        //            var user = new User
        //            {
        //                UserName = request.Username,
        //                Email = request.Email,
        //                EmailConfirmed = false,
        //                FirstName = request.FirstName,
        //                LastName = request.LastName,
        //                MiddleName = request.MiddleName,
        //                PhoneNumber = request.PhoneNumber,
        //                IsActive = true,
        //                PhoneNumberConfirmed = false,
        //            };
        //            var result = await signInManager.UserManager.CreateAsync(user, request.Password).ConfigureAwait(false);
        //            if (result.Succeeded)//Add user to a role
        //            {
        //                //var addUserToRoleResult = await AddUserToRole(user, Roles.Customer).ConfigureAwait(false);
        //                var addUserToRoleResult = await _userManager.AddToRoleAsync(user, Roles.Admin);
        //                if (addUserToRoleResult.Succeeded)
        //                {
        //                    transaction.Commit();
        //                    response.HttpStatusCode = System.Net.HttpStatusCode.OK;
        //                    response.Succeeded = addUserToRoleResult.Succeeded;
        //                    return response;
        //                }
        //                throw new Exception(addUserToRoleResult.Errors.ToString());
        //            }

        //            throw new Exception(ExtractErrorMessage(result));
        //        }
        //        catch (Exception ex)
        //        {
        //            transaction.Rollback();
        //            ////logger.Error(ex, string.Format(AccountOptions.RaisedError, DateTime.Now, ex.Message, nameof(RegisterAsync)));
        //            response.Message = ex.Message.ToString();
        //            response.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
        //        }
        //        return response;
        //    });
        //}

        public async Task<ResponseModel> ChangePhoneNumberAsync(ChangePhoneNumberRequest request)
        {

            var response = new ResponseModel<string>
            {
                HttpStatusCode = System.Net.HttpStatusCode.BadRequest,
                Succeeded = false
            };

            var user = await _signInManager.UserManager.FindByNameAsync(request.Username);
            if (user == null)
            {
                response.Message = AccountOptions.UserNotFound;
                return response;
            }

            var passIsCorrect = await _signInManager.UserManager.CheckPasswordAsync(user, request.Password);
            if (!passIsCorrect)
            {
                response.Message = AccountOptions.PasswordIsIncorrect;
                return response;
            }

            //Generate phone token
            var token = await _signInManager.UserManager.GenerateChangePhoneNumberTokenAsync(user, user.PhoneNumber);
            //logger.Debug("PhoneNumber is:" + user.PhoneNumber + "-" + token);
            ////Send token via SMS
            //if (!string.IsNullOrEmpty(token) && false)
            //{
            //    var res = await messageSendClient.GetResponse<MB_MessageSend>(new MB_MessageSend
            //    {
            //        Body = $"Daralber Verification Code: {token}",
            //        Receiver = user.PhoneNumber,
            //        UniCode = "UTF"
            //    });
            //    //logger.Debug("res:" + request.Password);
            //    response.Succeeded = res?.Message?.IsSent ?? false;

            //}

            response.Succeeded = true;
            response.Response = user.PhoneNumber + ": " + token;
            response.HttpStatusCode = System.Net.HttpStatusCode.OK;
            return response;
        }


        public async Task<ResponseModel> ConfirmPhoneNumberAsync(ConfirmPhoneNumberRequest request)
        {
            var response = new ResponseModel
            {
                Succeeded = false,
                HttpStatusCode = System.Net.HttpStatusCode.BadRequest
            };


            var user = await _signInManager.UserManager.FindByNameAsync(request.Username);
            if (user == null)
            {
                response.Message = AccountOptions.UserNotFound;
                return response;
            }

            var passIsCorrect = await _signInManager.UserManager.CheckPasswordAsync(user, request.Password);
            if (!passIsCorrect)
            {
                response.Message = AccountOptions.PasswordIsIncorrect;
                return response;
            }

            var res = await _signInManager.UserManager.ChangePhoneNumberAsync(user, user.PhoneNumber, request.Token);
            if (!res.Succeeded)
            {
                response.Message = String.Join(", ", res.Errors.Select(a => a.Code + ": " + a.Description).ToList());
                return response;
            }

            response.HttpStatusCode = System.Net.HttpStatusCode.OK;
            response.Succeeded = true;
            return response;
        }

        public async Task<ResponseModel> GeneratePhoneNumberTokenAsync(PhoneNumberTokenRequest request)
        {
            var response = new ResponseModel
            {
                Succeeded = false,
                HttpStatusCode = System.Net.HttpStatusCode.BadRequest
            };

            var user = await _signInManager.UserManager.FindByNameAsync(request.Username);
            if (user == null)
            {
                response.Message = AccountOptions.UserNotFound;
                return response;
            }

            var token = await _userManager .GenerateChangePhoneNumberTokenAsync(user, user.PhoneNumber);

            response.HttpStatusCode = System.Net.HttpStatusCode.OK;
            response.Message = token;
            response.Succeeded = true;

            return response;
        }

        public async Task<ResponseModel> CreateUserAsync(CreateUserByAdminRequest request)
        {
            var response = new ResponseModel()
            {
                Succeeded = false,
                HttpStatusCode = System.Net.HttpStatusCode.BadRequest
            };

            //if (string.IsNullOrEmpty(request.FirstName))
            //{
            //    response.Message = AccountOptions.FirstNameIsRequired;
            //    return response;
            //}

            //if (string.IsNullOrEmpty(request.LastName))
            //{
            //    response.Message = AccountOptions.LastNameIsRequired;
            //    return response;
            //}

            if (string.IsNullOrEmpty(request.Password))
            {
                response.Message = AccountOptions.LastNameIsRequired;
                return response;
            }

            if (await _signInManager.UserManager.FindByNameAsync(request.UserName) != null)
            {
                response.Message = AccountOptions.UsernameIsAlreadyExists;
                return response;
            }

            var executionStrategy = context.Database.CreateExecutionStrategy();

            return await executionStrategy.Execute(async () =>
            {
                // execute your logic here
                using var transaction = context.Database.BeginTransaction();
                try
                {
                    var user = new User
                    {
                        UserName = request.UserName,
                        EmailConfirmed = false,
                        Email = request.Email,
                        //FirstName = request.FirstName,
                        //LastName = request.LastName,
                        PhoneNumber = request.PhoneNumber,

                        //IsActive = true
                    };

                    var result = await _signInManager.UserManager.CreateAsync(user, request.Password).ConfigureAwait(false);

                    if (!result.Succeeded)
                    {
                        throw new Exception(string.Join(',', result.Errors?.ToList().Select(a => a.Description).ToList()));
                    }

                    foreach (var item in request.Roles)
                    {
                        var res = await _signInManager.UserManager.AddToRoleAsync(user, item);
                        if (!res.Succeeded)
                        {
                            throw new Exception(string.Join(',', res.Errors.ToList().Select(a => a.Description).ToList()));
                        }
                    }

                    transaction.Commit();
                    response.HttpStatusCode = System.Net.HttpStatusCode.OK;
                    response.Message = AccountOptions.UserRegisterSuccessfully;
                    response.Succeeded = true;
                    return response;
                    
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ////logger.Error(ex, string.Format(AccountOptions.RaisedError, DateTime.Now, ex.Message, nameof(RegisterAsync)));
                    response.Message = ex.Message.ToString();
                    response.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
                }
                return response;
            });
        }

        //public async Task<ResponseModel> GenerateEmailConfirmationTokenAsync(EmailConfirmationRequest request)
        //{
        //    var response = new ResponseModel()
        //    {
        //        Succeeded = false,
        //        HttpStatusCode = System.Net.HttpStatusCode.BadRequest
        //    };

        //    try
        //    {
        //        if (string.IsNullOrEmpty(request.Email))
        //            return response;

        //        var user = await signInManager.UserManager.FindByEmailAsync(request.Email.Trim()).ConfigureAwait(false);

        //        if (user != null)
        //        {
        //            response.Message = await signInManager.UserManager.GenerateEmailConfirmationTokenAsync(user).ConfigureAwait(false);
        //            response.Succeeded = true;
        //            return response;
        //        }

        //        response.Message = AccountOptions.UsernameOrEmailIsIncorrect;
        //    }
        //    catch (Exception ex)
        //    {
        //        ////logger.Error(ex, string.Format(AccountOptions.RaisedError, DateTime.Now, ex.Message, nameof(GenerateForgetPasswordTokenAsync)));
        //        response.Message = ex.Message.ToString();
        //        response.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
        //    }

        //    return response;
        //}
        public async Task<ResponseModel> ConfirmEmailAsync(string token, string email)
        {
            var response = new ResponseModel
            {
                Succeeded = false,
                HttpStatusCode = System.Net.HttpStatusCode.BadRequest
            };

            try
            {
                if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
                    return response;

                var user = await _signInManager.UserManager.FindByEmailAsync(email).ConfigureAwait(false);
                if (user == null)
                {
                    response.Message = AccountOptions.EmailAddressIncorrect;
                    return response;
                }
                var result = await _signInManager.UserManager.ConfirmEmailAsync(user, token).ConfigureAwait(false);
                if (result.Succeeded)
                {
                    response.Succeeded = result.Succeeded;
                    response.Message = AccountOptions.EmailConfirmation;
                    response.HttpStatusCode = System.Net.HttpStatusCode.OK;
                    return response;
                }

                throw new Exception(ExtractErrorMessage(result));
            }
            catch (Exception ex)
            {
                ////logger.Error(ex, string.Format(AccountOptions.RaisedError, DateTime.Now, ex.Message, nameof(ConfirmEmailAsync)));
                response.Message = ex.Message.ToString();
                response.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
            }

            return response;
        }
        public async Task<ResponseModel> GenerateForgetPasswordTokenAsync(ForgetPasswordRequest request)
        {
            var response = new ResponseModel()
            {
                Succeeded = false,
                HttpStatusCode = System.Net.HttpStatusCode.BadRequest
            };

            try
            {
                if (string.IsNullOrEmpty(request.Username) && string.IsNullOrEmpty(request.Email))
                    return response;

                var user = new IdentityUser();
                if (!string.IsNullOrEmpty(request.Email))

                    user = await _signInManager.UserManager.FindByEmailAsync(request.Email.Trim()).ConfigureAwait(false);
                else
                    user = await _signInManager.UserManager.FindByNameAsync(request.Username.Trim()).ConfigureAwait(false);

                if (user != null)
                {
                    response.Message = await _signInManager.UserManager.GeneratePasswordResetTokenAsync(user).ConfigureAwait(false);
                    response.Succeeded = true;
                    return response;
                }

                response.Message = AccountOptions.UsernameOrEmailIsIncorrect;
            }
            catch (Exception ex)
            {
                ////logger.Error(ex, string.Format(AccountOptions.RaisedError, DateTime.Now, ex.Message, nameof(GenerateForgetPasswordTokenAsync)));
                response.Message = ex.Message.ToString();
                response.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
            }

            return response;
        }
        //public async Task<ResponseModel> ChangeEmailAsync(string userName, string token, string newEmail, string oldEmail)
        //{
        //    var response = new ResponseModel
        //    {
        //        Succeeded = false,
        //        HttpStatusCode = System.Net.HttpStatusCode.BadRequest
        //    };

        //    try
        //    {
        //        if (string.IsNullOrEmpty(token) ||
        //            string.IsNullOrEmpty(newEmail) ||
        //            string.IsNullOrEmpty(oldEmail))
        //            return response;

        //        var user = await signInManager.UserManager.FindByEmailAsync(oldEmail).ConfigureAwait(false);
        //        if (user == null)
        //        {
        //            response.Message = AccountOptions.EmailAddressIncorrect;
        //            return response;
        //        }

        //        user = await signInManager.UserManager.FindByEmailAsync(newEmail).ConfigureAwait(false);
        //        if (user != null)
        //        {
        //            response.Message = AccountOptions.EmailIsAlreadyExists;
        //            return response;
        //        }

        //        user = await signInManager.UserManager.FindByNameAsync(userName).ConfigureAwait(false);
        //        if (user != null)
        //        {
        //            var result = await signInManager.UserManager.ChangeEmailAsync(user, newEmail, token).ConfigureAwait(false);
        //            if (result.Succeeded)
        //            {
        //                response.Succeeded = result.Succeeded;
        //                response.Message = AccountOptions.EmailConfirmation;
        //                response.HttpStatusCode = System.Net.HttpStatusCode.OK;
        //                return response;
        //            }

        //            throw new Exception(ExtractErrorMessage(result));
        //        }

        //        response.Message = AccountOptions.InvalidCredentialsErrorMessage;
        //        response.HttpStatusCode = System.Net.HttpStatusCode.Unauthorized;
        //    }
        //    catch (Exception ex)
        //    {
        //        ////logger.Error(ex, string.Format(AccountOptions.RaisedError, DateTime.Now, ex.Message, nameof(ConfirmEmailAsync)));
        //        response.Message = ex.Message.ToString();
        //        response.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
        //    }

        //    return response;
        //}
        //public async Task<ResponseModel> SetEmailAsync(SetEmailRequest request)
        //{
        //    var response = new ResponseModel
        //    {
        //        Succeeded = false,
        //        HttpStatusCode = System.Net.HttpStatusCode.BadRequest
        //    };

        //    try
        //    {
        //        if (string.IsNullOrEmpty(request.Email) ||
        //            string.IsNullOrEmpty(request.UserName))
        //            return response;

        //        var user = await signInManager.UserManager.FindByNameAsync(request.UserName).ConfigureAwait(false);
        //        if (user != null)
        //        {
        //            var result = await signInManager.UserManager.SetEmailAsync(user, request.Email).ConfigureAwait(false);
        //            if (result.Succeeded)
        //            {
        //                response.Succeeded = result.Succeeded;
        //                response.Message = AccountOptions.EmailConfirmation;
        //                response.HttpStatusCode = System.Net.HttpStatusCode.OK;
        //                return response;
        //            }

        //            throw new Exception(ExtractErrorMessage(result));
        //        }

        //        response.Message = AccountOptions.InvalidCredentialsErrorMessage;
        //        response.HttpStatusCode = System.Net.HttpStatusCode.Unauthorized;
        //    }
        //    catch (Exception ex)
        //    {
        //        ////logger.Error(ex, string.Format(AccountOptions.RaisedError, DateTime.Now, ex.Message, nameof(ConfirmEmailAsync)));
        //        response.Message = ex.Message.ToString();
        //        response.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
        //    }

        //    return response;
        //}
        public async Task<UserProfileResponse> GetUserProfile(string userId)
        {
            // Prepare response
            var response = new UserProfileResponse()
            {
                Succeeded = false,
                HttpStatusCode = System.Net.HttpStatusCode.BadRequest
            };

            // Validate User ID
            if (string.IsNullOrEmpty(userId))
            {
                response.Message = AccountOptions.InvalidUserID;
                return response;
            }

            // Get user by their Id
            var user = await _signInManager.UserManager.FindByIdAsync(userId).ConfigureAwait(false);

            // Validate User
            if (user == null)
            {
                response.HttpStatusCode = System.Net.HttpStatusCode.NotFound;
                response.Message = AccountOptions.UserNotFound;
                return response;
            }

            // Get user roles
            var userRoles = await _signInManager.UserManager.GetRolesAsync(user).ConfigureAwait(false);

            // Map user data to profile response
            mapper.Map(user, response);

            response.Succeeded = true;
            response.HttpStatusCode = System.Net.HttpStatusCode.OK;
            return response;

        }

        //public async Task<ResponseModel> SyncUserProfileAsync(UpdateUserProfileRequest request, string username)
        //{
        //    return await UpdateUserProfileInternaly(request, username);
        //}


        public async Task<ResponseModel> UpdateUserProfileAsync(UpdateUserProfileRequest request, string username)
        {
            var response = await UpdateUserProfileInternaly(request, username);

            try
            {
                //var res = mapper.Map(request, new MB_UserProfileChange());
                //res.UserId = (await userManager.FindByNameAsync(username)).Id;
                //await endpoint.Publish<MB_UserProfileChange>(res);
            }
            catch (Exception ex)
            {
                //logger.Error(ex, string.Format(AccountOptions.RaisedError, DateTime.Now, ex.Message, nameof(UpdateUserProfileAsync)));
                response.Message = AccountOptions.PublicErrorMessage;
                response.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
            }

            return response;
        }

        private async Task<ResponseModel> UpdateUserProfileInternaly(UpdateUserProfileRequest request, string username)
        {
            // Prepare response
            var response = new ResponseModel()
            {
                Succeeded = false,
                HttpStatusCode = System.Net.HttpStatusCode.BadRequest
            };

            try
            {
                var validation = UpdateUserPrfileValidation(request);
                if (!validation.Succeeded)
                    return validation;

                var user = await _signInManager.UserManager.FindByNameAsync(username).ConfigureAwait(false);
                //mapper.Map(request, user);

                var result = await _signInManager.UserManager.UpdateAsync(user).ConfigureAwait(false);

                if (result.Succeeded)
                {
                    response.Succeeded = true;
                    response.Message = AccountOptions.UpdateProfileSuccessfully;
                    response.HttpStatusCode = System.Net.HttpStatusCode.OK;
                    return response;
                }

                throw new Exception(ExtractErrorMessage(result));
            }
            catch (Exception ex)
            {
                //logger.Error(ex, string.Format(AccountOptions.RaisedError, DateTime.Now, ex.Message, nameof(UpdateUserProfileInternaly)));
                response.Message = AccountOptions.PublicErrorMessage;
                response.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
            }

            return response;
        }

        public async Task<ResponseModel> ChangePasswordAsync(ChangePasswordRequest request, string username)
        {
            var response = new ResponseModel()
            {
                Succeeded = false,
                HttpStatusCode = System.Net.HttpStatusCode.BadRequest
            };

            try
            {
                if (string.IsNullOrEmpty(request.CurrentPassword) || string.IsNullOrEmpty(request.NewPassword))
                    return response;

                var user = await _signInManager.UserManager.FindByNameAsync(username).ConfigureAwait(false);
                var result = await _signInManager.UserManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword).ConfigureAwait(false);

                if (result.Succeeded)
                {
                    response.Succeeded = result.Succeeded;
                    response.HttpStatusCode = System.Net.HttpStatusCode.OK;
                    response.Message = AccountOptions.ChangingPasswordSuccessfully;
                    return response;
                }

                throw new Exception(ExtractErrorMessage(result));
            }
            catch (Exception ex)
            {
                //logger.Error(ex, string.Format(AccountOptions.RaisedError, DateTime.Now, ex.Message, nameof(ChangePasswordAsync)));
                response.Message = ex.Message.ToString();
                response.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
            }

            return response;
        }
        //public async Task<ResponseModel> GenerateChangeEmailTokenAsync(ChangeEmailRequest request, string userName)
        //{
        //    var response = new ResponseModel()
        //    {
        //        Succeeded = false,
        //        HttpStatusCode = System.Net.HttpStatusCode.BadRequest
        //    };

        //    try
        //    {
        //        if (string.IsNullOrEmpty(request.Email))
        //            return response;

        //        var user = await signInManager.UserManager.FindByNameAsync(userName).ConfigureAwait(false);

        //        if (user != null)
        //        {
        //            response.Message = await signInManager.UserManager.GenerateChangeEmailTokenAsync(user, request.Email).ConfigureAwait(false);
        //            response.Succeeded = true;
        //            return response;
        //        }

        //        response.Message = AccountOptions.UsernameOrEmailIsIncorrect;
        //    }
        //    catch (Exception ex)
        //    {
        //        //logger.Error(ex, string.Format(AccountOptions.RaisedError, DateTime.Now, ex.Message, nameof(GenerateForgetPasswordTokenAsync)));
        //        response.Message = ex.Message.ToString();
        //        response.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
        //    }

        //    return response;
        //}
        public async Task<ResponseModel> ResetPasswordAsync(ResetPasswordModel request)
        {
            var response = new ResponseModel()
            {
                Succeeded = false,
                HttpStatusCode = System.Net.HttpStatusCode.BadRequest
            };

            try
            {
                if (string.IsNullOrEmpty(request.Token) ||
                    string.IsNullOrEmpty(request.Email) ||
                    string.IsNullOrEmpty(request.Password))
                    return response;

                var user = await _signInManager.UserManager.FindByEmailAsync(request.Email.Trim()).ConfigureAwait(false);

                if (user != null)
                {
                    var result = await _signInManager.UserManager.ResetPasswordAsync(user, request.Token, request.Password).ConfigureAwait(false);
                    if (result.Succeeded)
                    {
                        response.Succeeded = result.Succeeded;
                        response.HttpStatusCode = System.Net.HttpStatusCode.OK;
                        response.Message = AccountOptions.ResetingPasswordSuccessfully;
                        return response;
                    }

                    throw new Exception(ExtractErrorMessage(result));
                }

                response.Message = AccountOptions.InvalidCredentialsErrorMessage;

            }
            catch (Exception ex)
            {
                //logger.Error(ex, string.Format(AccountOptions.RaisedError, DateTime.Now, ex.Message, nameof(ResetPasswordAsync)));
                response.Message = ex.Message.ToString();
                response.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
            }

            return response;
        }
        //public async Task<ResponseModel> ActiveDeactiveUserAsync(DeactiveUserRequest request)
        //{
        //    var response = new ResponseModel()
        //    {
        //        Succeeded = false,
        //        HttpStatusCode = System.Net.HttpStatusCode.BadRequest
        //    };

        //    try
        //    {
        //        if (string.IsNullOrEmpty(request.Username))
        //            return response;

        //        var user = await signInManager.UserManager.FindByNameAsync(request.Username).ConfigureAwait(false);

        //        user.IsActive = !user.IsActive;

        //        var result = await signInManager.UserManager.UpdateAsync(user).ConfigureAwait(false);

        //        if (result.Succeeded)
        //        {
        //            response.Succeeded = result.Succeeded;
        //            response.HttpStatusCode = System.Net.HttpStatusCode.OK;
        //            response.Message = AccountOptions.DeactivationUserSuccessfully;
        //            return response;
        //        }

        //        throw new Exception(ExtractErrorMessage(result));
        //    }
        //    catch (Exception ex)
        //    {
        //        //logger.Error(ex, string.Format(AccountOptions.RaisedError, DateTime.Now, ex.Message, nameof(ActiveDeactiveUserAsync)));
        //        response.Message = ex.Message.ToString();
        //        response.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
        //    }

        //    return response;
        //}

        public async Task<ResponseModel> EditRole(EditRoleNameRequest editRole)
        {
            var response = new ResponseModel()
            {
                Succeeded = false,
                HttpStatusCode = System.Net.HttpStatusCode.BadRequest
            };

            if (string.IsNullOrEmpty(editRole.RoleName))
            {
                return response;
            }

            if (await _roleManager.RoleExistsAsync(editRole.RoleName))
            {
                response.Message = AccountOptions.RoleNameIsDuplicte;
                return response;
            }

            var role = await _roleManager.FindByIdAsync(editRole.Id);
            role.Name = editRole.RoleName;
            await _roleManager.UpdateAsync(role);

            response.HttpStatusCode = System.Net.HttpStatusCode.OK;
            response.Succeeded = true;
            response.Message = AccountOptions.AddRoleSuccessfully;
            return response;
        }

        public async Task<ResponseModel> DeleteRole(string id)
        {
            var response = new ResponseModel()
            {
                Succeeded = false,
                HttpStatusCode = System.Net.HttpStatusCode.BadRequest
            };

            try
            {
                await _roleManager.DeleteAsync(await _roleManager.FindByIdAsync(id));
                response.HttpStatusCode = System.Net.HttpStatusCode.OK;
                response.Succeeded = true;
                response.Message = AccountOptions.DeleteRoleSuccessfully;
                return response;
            }
            catch (Exception ex)
            {
                //logger.Error(ex, string.Format(AccountOptions.RaisedError, DateTime.Now, ex.Message, nameof(DeleteRole)));
                response.Message = ex.Message.ToString();
                response.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
                return response;
            }
        }

        public async Task<ResponseModel> AddRole(string roleName)
        {
            var response = new ResponseModel()
            {
                Succeeded = false,
                HttpStatusCode = System.Net.HttpStatusCode.BadRequest
            };

            if (string.IsNullOrEmpty(roleName))
            {
                return response;
            }

            if (await _roleManager.RoleExistsAsync(roleName))
            {
                response.Message = AccountOptions.RoleNameIsDuplicte;
                return response;
            }

            await _roleManager.CreateAsync(new IdentityRole { Name = roleName });

            response.HttpStatusCode = System.Net.HttpStatusCode.OK;
            response.Succeeded = true;
            response.Message = AccountOptions.AddRoleSuccessfully;
            return response;
        }

        //public async Task<ResponseModel> AddClaimAsync(NewClaimRequest claim, string username)
        //{
        //    var response = new ResponseModel()
        //    {
        //        Succeeded = false,
        //        HttpStatusCode = System.Net.HttpStatusCode.BadRequest
        //    };

        //    try
        //    {
        //        if (string.IsNullOrEmpty(claim.Type) || string.IsNullOrEmpty(claim.Value))
        //            return response;

        //        var user = await signInManager.UserManager.FindByNameAsync(username).ConfigureAwait(false);
        //        var result = await signInManager.UserManager.AddClaimAsync(user, new Claim(claim.Type, claim.Value)).ConfigureAwait(false);

        //        if (result.Succeeded)
        //        {
        //            response.Succeeded = result.Succeeded;
        //            response.HttpStatusCode = System.Net.HttpStatusCode.OK;
        //            response.Message = AccountOptions.AddUserClaimSuccessfully;
        //            return response;
        //        }

        //        throw new Exception(ExtractErrorMessage(result));
        //    }
        //    catch (Exception ex)
        //    {
        //        //logger.Error(ex, string.Format(AccountOptions.RaisedError, DateTime.Now, ex.Message, nameof(AddClaimAsync)));
        //        response.Message = ex.Message.ToString();
        //        response.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
        //    }

        //    return response;
        //}
        //public async Task<ResponseModel> RemoveClaimAsync(NewClaimRequest claim, string username)
        //{
        //    var response = new ResponseModel()
        //    {
        //        Succeeded = false,
        //        HttpStatusCode = System.Net.HttpStatusCode.BadRequest
        //    };

        //    try
        //    {
        //        if (string.IsNullOrEmpty(claim.Type) || string.IsNullOrEmpty(claim.Value))
        //            return response;

        //        var user = await signInManager.UserManager.FindByNameAsync(username).ConfigureAwait(false);
        //        var result = await signInManager.UserManager.RemoveClaimAsync(user, new Claim(claim.Type, claim.Value)).ConfigureAwait(false);

        //        if (result.Succeeded)
        //        {
        //            response.Succeeded = result.Succeeded;
        //            response.HttpStatusCode = System.Net.HttpStatusCode.OK;
        //            response.Message = AccountOptions.RemoveUserClaimSuccessfully;
        //            return response;
        //        }

        //        throw new Exception(ExtractErrorMessage(result));
        //    }
        //    catch (Exception ex)
        //    {
        //        //logger.Error(ex, string.Format(AccountOptions.RaisedError, DateTime.Now, ex.Message, nameof(RemoveClaimAsync)));
        //        response.Message = ex.Message.ToString();
        //        response.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
        //    }

        //    return response;
        //}
        //public async Task<ResponseModel> AddClaimsAsync(IEnumerable<NewClaimRequest> claims, string username)
        //{
        //    var response = new ResponseModel()
        //    {
        //        Succeeded = false,
        //        HttpStatusCode = System.Net.HttpStatusCode.BadRequest
        //    };

        //    try
        //    {
        //        if (claims.Any(x => x.Type == null) || claims.Any(x => x.Value == null))
        //            return response;

        //        var user = await signInManager.UserManager.FindByNameAsync(username).ConfigureAwait(false);

        //        var _claims = new List<Claim>();
        //        foreach (var claim in claims)
        //            _claims.Add(new Claim(claim.Type, claim.Value));

        //        var result = await signInManager.UserManager.AddClaimsAsync(user, _claims).ConfigureAwait(false);

        //        if (result.Succeeded)
        //        {
        //            response.Succeeded = result.Succeeded;
        //            response.HttpStatusCode = System.Net.HttpStatusCode.OK;
        //            response.Message = AccountOptions.AddUserClaimSuccessfully;
        //            return response;
        //        }

        //        throw new Exception(ExtractErrorMessage(result));
        //    }
        //    catch (Exception ex)
        //    {
        //        //logger.Error(ex, string.Format(AccountOptions.RaisedError, DateTime.Now, ex.Message, nameof(AddClaimsAsync)));
        //        response.Message = ex.Message.ToString();
        //        response.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
        //    }

        //    return response;
        //}
        //public async Task<ResponseModel> RemoveClaimsAsync(IEnumerable<NewClaimRequest> claims, string username)
        //{
        //    var response = new ResponseModel()
        //    {
        //        Succeeded = false,
        //        HttpStatusCode = System.Net.HttpStatusCode.BadRequest
        //    };

        //    try
        //    {
        //        if (claims.Any(x => x.Type == null) || claims.Any(x => x.Value == null))
        //            return response;

        //        var user = await signInManager.UserManager.FindByNameAsync(username).ConfigureAwait(false);

        //        var _claims = new List<Claim>();
        //        foreach (var claim in claims)
        //            _claims.Add(new Claim(claim.Type, claim.Value));

        //        var result = await signInManager.UserManager.RemoveClaimsAsync(user, _claims).ConfigureAwait(false);

        //        if (result.Succeeded)
        //        {
        //            response.Succeeded = result.Succeeded;
        //            response.HttpStatusCode = System.Net.HttpStatusCode.OK;
        //            response.Message = AccountOptions.RemoveUserClaimSuccessfully;
        //            return response;
        //        }

        //        throw new Exception(ExtractErrorMessage(result));
        //    }
        //    catch (Exception ex)
        //    {
        //        //logger.Error(ex, string.Format(AccountOptions.RaisedError, DateTime.Now, ex.Message, nameof(RemoveClaimsAsync)));
        //        response.Message = ex.Message.ToString();
        //        response.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
        //    }

        //    return response;
        //}
        public async Task<ResponseModel> AddUserToRoleAsync(string role, string username)
        {
            var response = new ResponseModel()
            {
                Succeeded = false,
                HttpStatusCode = System.Net.HttpStatusCode.BadRequest
            };

            try
            {
                if (role is null)
                    return response;

                var user = await _signInManager.UserManager.FindByNameAsync(username).ConfigureAwait(false);

                if (user != null)
                {
                    var result = await _signInManager.UserManager.AddToRoleAsync(user, role).ConfigureAwait(false);

                    if (result.Succeeded)
                    {
                        response.Succeeded = result.Succeeded;
                        response.HttpStatusCode = System.Net.HttpStatusCode.OK;
                        response.Message = AccountOptions.AddUserToRoleSuccessfully;
                        return response;
                    }

                    throw new Exception(ExtractErrorMessage(result));
                }

                response.Message = AccountOptions.InvalidCredentialsErrorMessage;
                response.HttpStatusCode = System.Net.HttpStatusCode.Unauthorized;
            }
            catch (Exception ex)
            {
                //logger.Error(ex, string.Format(AccountOptions.RaisedError, DateTime.Now, ex.Message, nameof(AddUserToRoleAsync)));
                response.Message = ex.Message.ToString();
                response.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
            }

            return response;
        }
        public async Task<ResponseModel> RemoveUserFromRoleAsync(string role, string username)
        {
            var response = new ResponseModel()
            {
                Succeeded = false,
                HttpStatusCode = System.Net.HttpStatusCode.BadRequest
            };

            try
            {
                if (role is null)
                    return response;

                var user = await _signInManager.UserManager.FindByNameAsync(username).ConfigureAwait(false);

                if (user != null)
                {
                    var result = await _signInManager.UserManager.RemoveFromRoleAsync(user, role).ConfigureAwait(false);

                    if (result.Succeeded)
                    {
                        response.Succeeded = result.Succeeded;
                        response.HttpStatusCode = System.Net.HttpStatusCode.OK;
                        response.Message = AccountOptions.RemoveUserFromRoleSuccessfully;
                        return response;
                    }

                    throw new Exception(ExtractErrorMessage(result));
                }

                response.Message = AccountOptions.InvalidCredentialsErrorMessage;
                response.HttpStatusCode = System.Net.HttpStatusCode.Unauthorized;
            }
            catch (Exception ex)
            {
                //logger.Error(ex, string.Format(AccountOptions.RaisedError, DateTime.Now, ex.Message, nameof(RemoveUserFromRoleAsync)));
                response.Message = ex.Message.ToString();
                response.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
            }

            return response;
        }
        public async Task<ResponseModel> AddUserToRolesAsync(AddUserToRolesRequest request, string username)
        {
            var response = new ResponseModel()
            {
                Succeeded = false,
                HttpStatusCode = System.Net.HttpStatusCode.BadRequest
            };

            try
            {
                if (!request.Roles.Any())
                    return response;

                var user = await _signInManager.UserManager.FindByNameAsync(username).ConfigureAwait(false);

                if (user != null)
                {
                    var result = await _signInManager.UserManager.AddToRolesAsync(user, request.Roles).ConfigureAwait(false);

                    if (result.Succeeded)
                    {
                        response.Succeeded = result.Succeeded;
                        response.HttpStatusCode = System.Net.HttpStatusCode.OK;
                        response.Message = AccountOptions.AddUserToRolesSuccessfully;
                        return response;
                    }

                    throw new Exception(ExtractErrorMessage(result));
                }

                response.Message = AccountOptions.InvalidCredentialsErrorMessage;
                response.HttpStatusCode = System.Net.HttpStatusCode.Unauthorized;
            }
            catch (Exception ex)
            {
                //logger.Error(ex, string.Format(AccountOptions.RaisedError, DateTime.Now, ex.Message, nameof(AddUserToRolesAsync)));
                response.Message = ex.Message.ToString();
                response.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
            }

            return response;
        }
        public async Task<ResponseModel> RemoveUserFromRolesAsync(AddUserToRolesRequest request, string username)
        {
            var response = new ResponseModel()
            {
                Succeeded = false,
                HttpStatusCode = System.Net.HttpStatusCode.BadRequest
            };

            try
            {
                if (!request.Roles.Any())
                    return response;

                var user = await _signInManager.UserManager.FindByNameAsync(username).ConfigureAwait(false);

                if (user != null)
                {
                    var result = await _signInManager.UserManager.RemoveFromRolesAsync(user, request.Roles).ConfigureAwait(false);

                    if (result.Succeeded)
                    {
                        response.Succeeded = result.Succeeded;
                        response.HttpStatusCode = System.Net.HttpStatusCode.OK;
                        response.Message = AccountOptions.RemoveUserFromRolesSuccessfully;
                        return response;
                    }

                    throw new Exception(ExtractErrorMessage(result));
                }

                response.Message = AccountOptions.InvalidCredentialsErrorMessage;
                response.HttpStatusCode = System.Net.HttpStatusCode.Unauthorized;
            }
            catch (Exception ex)
            {
                //logger.Error(ex, string.Format(AccountOptions.RaisedError, DateTime.Now, ex.Message, nameof(RemoveUserFromRolesAsync)));
                response.Message = ex.Message.ToString();
                response.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
            }

            return response;
        }
        public async Task<ResponseModel<Claim>> GetUserClaimsAsync(GetUserClaimsRequest request)
        {
            var response = new ResponseModel<Claim>(request.PagingMode, request.PageIndex, request.PageSize)
            {
                Succeeded = false,
                HttpStatusCode = System.Net.HttpStatusCode.BadRequest
            };

            if (request == null)
                return response;

            if (string.IsNullOrEmpty(request.UserName))
            {
                response.Message = AccountOptions.UsernameIsRequired;
                return response;
            }

            var user = await _signInManager.UserManager.FindByNameAsync(request.UserName).ConfigureAwait(false);
            if (user != null)
            {
                var claims = await _signInManager.UserManager.GetClaimsAsync(user).ConfigureAwait(false);

                if (!string.IsNullOrEmpty(request.ClaimName))
                {
                    claims = claims.Where(x => x.Value == request.ClaimName).ToList();
                }

                if (!string.IsNullOrEmpty(request.ClaimType))
                {
                    claims = claims.Where(x => x.Type == request.ClaimType).ToList();
                }

                response.ResponseList = claims;
                response.Succeeded = true;
                response.HttpStatusCode = System.Net.HttpStatusCode.OK;
            }
            else
                response.Message = AccountOptions.InvalidUsername;

            return response;
        }
        public async Task<ResponseModel<string>> GetUserRolesAsync(GetUserRolesRequest request)
        {
            var response = new ResponseModel<string>(request.PagingMode, request.PageIndex, request.PageSize)
            {
                Succeeded = false,
                HttpStatusCode = System.Net.HttpStatusCode.BadRequest
            };

            if (request == null)
                return response;

            if (string.IsNullOrEmpty(request.UserName))
            {
                response.Message = AccountOptions.UsernameIsRequired;
                return response;
            }

            var user = await _signInManager.UserManager.FindByNameAsync(request.UserName).ConfigureAwait(false);
            if (user != null)
            {
                var roles = await _signInManager.UserManager.GetRolesAsync(user).ConfigureAwait(false);
                if (!string.IsNullOrEmpty(request.RoleName))
                {
                    roles = roles.Where(x => x.Contains(request.RoleName)).ToList();
                }

                response.ResponseList = roles;
                response.Succeeded = true;
                response.HttpStatusCode = System.Net.HttpStatusCode.OK;
            }
            else
                response.Message = AccountOptions.InvalidUsername;

            return response;
        }


        public async Task<List<UserResponse>> GetRoleUsersAsync(GetRoleUsersRequest request)
        {
            var users = await _signInManager.UserManager.GetUsersInRoleAsync(request.RoleName).ConfigureAwait(false);
            var filteredUsers = users.ToList(); //users.Where(f => f.IsActive == true).ToList();
            //if (request.PagingMode)
            //    filteredUsers = filteredUsers.Skip(request.PageIndex * request.PageSize)
            //        .Take(request.PageSize).ToList();

            return filteredUsers.Select(f => new UserResponse
            {
                Email = f.Email,
                //FirstName = f.FirstName,
                //IsActive = true,
                //LastName = f.LastName,
                //MiddleName = f.MiddleName,
                PhoneNumber = f.PhoneNumber,
                UserId = f.Id,
                UserName = f.UserName
            }).ToList();
        }


        public async Task<ResponseModel<GetAllRolesResponse>> GetAllRolesAsync() => new ResponseModel<GetAllRolesResponse>
        {
            Succeeded = true,
            HttpStatusCode = System.Net.HttpStatusCode.OK,
            ResponseList = await context.Roles.Select(x => new GetAllRolesResponse { Id = x.Id, RoleName = x.Name }).ToListAsync()
        };

        public async Task<ResponseModel<GetAllRolesResponse>> GetAllRolesAsync(Models.Account.GetRoleRequest request) => new ResponseModel<GetAllRolesResponse>(true, request.PageIndex, request.PageSize)
        {
            Succeeded = true,
            HttpStatusCode = System.Net.HttpStatusCode.OK,
            ResponseList = await context.Roles
                .Where(a => string.IsNullOrEmpty(request.RoleName) || a.Name.Contains(request.RoleName))
                .Select(x => new GetAllRolesResponse { Id = x.Id, RoleName = x.Name, Normalized = x.NormalizedName }).ToListAsync()
        };

        public async Task<ResponseModel<GetAllUsersResponse>> GetAllUsersAsync() => new ResponseModel<GetAllUsersResponse>
        {
            Succeeded = true,
            HttpStatusCode = System.Net.HttpStatusCode.OK,
            ResponseList = await context.Users.Select(x => new GetAllUsersResponse { Id = x.Id, UserName = x.UserName, Email = x.Email, PhoneNumber = x.PhoneNumber, PhoneNumberConfirmed = x.PhoneNumberConfirmed }).ToListAsync()
        };

        ////public async Task<ResponseModel<GetAllUsersResponse>> GetAllUsersAsync(GetUserRequest request) => new ResponseModel<GetAllUsersResponse>(true, request.PageIndex, request.PageSize)
        ////{
        ////    Succeeded = true,
        ////    HttpStatusCode = System.Net.HttpStatusCode.OK,

        ////    ResponseList = await context.Users
        ////    .Where(a => string.IsNullOrEmpty(request.FirstName) || a.FirstName.Contains(request.FirstName))
        ////    .Where(a => string.IsNullOrEmpty(request.LastName) || a.FirstName.Contains(request.LastName))
        ////    .Where(a => string.IsNullOrEmpty(request.Email) || a.FirstName.Contains(request.Email))
        ////    .Where(a => string.IsNullOrEmpty(request.Filter) ||
        ////        a.UserName.Contains(request.Filter) ||
        ////        a.FirstName.Contains(request.Filter) ||
        ////        a.LastName.Contains(request.Filter) ||
        ////        a.LastName.Contains(request.Username) ||
        ////        a.Email.Contains(request.Filter)
        ////    )
        ////    .Select(x => new GetAllUsersResponse
        ////    {
        ////        Id = x.Id,
        ////        UserName = x.UserName,
        ////        FirstName = x.FirstName,
        ////        Email = x.Email,
        ////        LastName = x.LastName,
        ////        MiddleName = x.MiddleName,
        ////        PhoneNumber = x.PhoneNumber,
        ////        IsActive = x.IsActive
        ////    })
        ////    .OrderByDescending(a => a.Id)
        ////    .ToListAsync()
        ////};

        //public async Task<ResponseModel<GetClaimResponse>> GetClaimAsync(GetClaimRequest request)
        //{
        //    return new ResponseModel<GetClaimResponse>();
        //}

        public async Task<ResponseModel<GetAllRoleClaimsResponse>> GetAllRoleClaimsAsync()
        {
            var query = from rc in context.RoleClaims
                        join r in context.Roles on rc.RoleId equals r.Id
                        orderby r.Name
                        select new GetAllRoleClaimsResponse
                        {
                            Id = rc.Id,
                            RoleName = r.Name,
                            ClaimType = rc.ClaimType,
                            ClaimValue = rc.ClaimValue
                        };

            return new ResponseModel<GetAllRoleClaimsResponse>
            {
                Succeeded = true,
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                ResponseList = await query.ToListAsync()
            };
        }
        public async Task<ResponseModel<GetAllUserClaimsResponse>> GetAllUserClaimsAsync()
        {
            var query = from uc in context.UserClaims
                        join u in context.Users on uc.UserId equals u.Id
                        orderby u.UserName
                        select new GetAllUserClaimsResponse
                        {
                            Id = uc.Id,
                            UserName = u.UserName,
                            ClaimType = uc.ClaimType,
                            ClaimValue = uc.ClaimValue
                        };

            return new ResponseModel<GetAllUserClaimsResponse>
            {
                Succeeded = true,
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                ResponseList = await query.ToListAsync()
            };
        }

        public async Task<ResponseModel<string>> GetRolesByUserIdAsync(string userId)
        {
            var response = new ResponseModel<string>(false, 0, 1)
            {
                Succeeded = false,
                HttpStatusCode = System.Net.HttpStatusCode.BadRequest
            };
            if (string.IsNullOrEmpty(userId))
            {
                response.Message = AccountOptions.InvalidUserID;
                return response;
            }
            var user = await _signInManager.UserManager.FindByIdAsync(userId);
            if (user != null)
            {
                var roles = await _signInManager.UserManager.GetRolesAsync(user).ConfigureAwait(false);

                response.ResponseList = roles;
                response.Succeeded = true;
                response.HttpStatusCode = System.Net.HttpStatusCode.OK;
            }
            else
                response.Message = AccountOptions.InvalidUserID;

            return response;
        }
        ////public async Task<MB_GetUserInfoByIdResponse> GetUserInfoAsync(string UserId)
        ////{
        ////    var user = signInManager.UserManager.Users.FirstOrDefault(x => x.Id == UserId);


        ////    return new MB_GetUserInfoByIdResponse
        ////    {
        ////        FirstName = user.FirstName,
        ////        MiddleName = user.MiddleName,
        ////        LastName = user.LastName,
        ////        Email = user.Email,
        ////        PhoneNumber = user.PhoneNumber,
        ////        UserId = user.Id,
        ////        UserName = user.UserName,
        ////        IsActive = user.IsActive
        ////    };
        ////}

        #region Private methods

        //private async Task PublishPersonalInformationChanged(PersonalInformationChanged personalInformation)
        //{
        //    try
        //    {
        //        logger.Debug("Publishing the personal information change. The new information is {@personalInformation}", personalInformation);
        //        await endpoint.Publish<PersonalInformationChanged>(personalInformation);

        //        logger.Debug("The personal information change message published. The new information is {@personalInformation}", personalInformation);
        //    }
        //    catch (Exception ex)
        //    {
        //        //logger.Error(ex.Message,
        //            "An error occurred when Publishing the personal information change. The new information is {@personalInformation}",
        //            personalInformation);
        //    }
        //}


        private async Task<IEnumerable<Claim>> GetUserClaims(IdentityUser user) => await _signInManager.UserManager.GetClaimsAsync(user).ConfigureAwait(false);
        private async Task<ResponseModel> AddUserToRole(IdentityUser user, string roleName)
        {
            var response = new ResponseModel
            {
                Succeeded = true
            };

            var result = await _signInManager.UserManager.AddToRoleAsync(user, roleName).ConfigureAwait(false);
            if (!result.Succeeded)
            {
                var errors = new StringBuilder();
                foreach (var error in result.Errors)
                    errors.Append($"Code: {error.Code} --> Description:{error.Description}\n");

                //logger.Warning(string.Format(AccountOptions.ExecutionActionRaisedError, nameof(AddUserToRole), DateTime.Now, errors.ToString()));
                response.Succeeded = false;
            }
            return response;
        }

        private static LoginResponse LoginValidation(LoginRequest request)
        {
            var response = new LoginResponse()
            {
                Message = string.Empty,
                Succeeded = true
            };
            if (request == null ||
                request.Username == null ||
                request.Password == null)
            {
                response.Message = AccountOptions.InvalidCredentialsErrorMessage;
                response.Succeeded = false;
            }

            return response;
        }
        private static ResponseModel UpdateUserPrfileValidation(UpdateUserProfileRequest request)
        {
            var response = new ResponseModel()
            {
                Message = AccountOptions.BadRequest,
                Succeeded = false,
                HttpStatusCode = System.Net.HttpStatusCode.BadRequest
            };
            if (request == null)
                return response;

            if (string.IsNullOrEmpty(request.FirstName))
            {
                response.Message = AccountOptions.FirstNameIsRequired;
                return response;
            }

            if (string.IsNullOrEmpty(request.LastName))
            {
                response.Message = AccountOptions.LastNameIsRequired;
                return response;
            }

            response.Succeeded = true;
            response.Message = string.Empty;

            return response;
        }

        //private ResponseModel UserRegisterValidation(RegisterUserRequest request)
        //{
        //    var response = new ResponseModel()
        //    {
        //        Succeeded = false,
        //        HttpStatusCode = System.Net.HttpStatusCode.BadRequest
        //    };

        //    if (request == null)
        //        response.Message = AccountOptions.BadRequest;

        //    if (string.IsNullOrEmpty(request.FirstName))
        //    {
        //        response.Message = AccountOptions.FirstNameIsRequired;
        //        return response;
        //    }

        //    if (string.IsNullOrEmpty(request.LastName))
        //    {
        //        response.Message = AccountOptions.LastNameIsRequired;
        //        return response;
        //    }

        //    if (string.IsNullOrEmpty(request.Username))
        //    {
        //        response.Message = AccountOptions.UsernameIsRequired;
        //        return response;
        //    }

        //    if (string.IsNullOrEmpty(request.Email))
        //    {
        //        response.Message = AccountOptions.EmailIsRequired;
        //        return response;
        //    }

        //    if (string.IsNullOrEmpty(request.PhoneNumber))
        //    {
        //        response.Message = AccountOptions.PhoneNumberIsRequired;
        //        return response;
        //    }

        //    /* var checkUser = signInManager.UserManager.FindByNameAsync(request.Username).Result;
        //    if (checkUser != null)
        //    {
        //        response.Message = AccountOptions.UsernameIsAlreadyExists;
        //        return response;
        //    } */

        //    var checkEmail = signInManager.UserManager.FindByEmailAsync(request.Email).Result;
        //    if (checkEmail != null)
        //    {
        //        response.Message = AccountOptions.EmailIsAlreadyExists;
        //        return response;
        //    }

        //    response.Succeeded = true;
        //    return response;
        //}
        private static string ExtractErrorMessage(IdentityResult identityResult)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var error in identityResult.Errors)
                sb.Append($"{error.Code} --> {error.Description}");
            return sb.ToString();
        }

        ////[Obsolete("IdentityServer's OAuth2 is used for issuing JWT")]
        ////private async Task<string> GetJWTToken(User user, bool rememberMe)
        ////{
        ////    var roles = await _userManager.GetRolesAsync(user);
        ////    var claims = new List<Claim>
        ////    {
        ////        new Claim(ClaimTypes.NameIdentifier, user.Id),
        ////        new Claim(ClaimTypes.Name, user.UserName??""),
        ////        new Claim(ClaimTypes.Surname, user.LastName??""),
        ////        new Claim(ClaimTypes.Role, roles.FirstOrDefault()??""),
        ////        new Claim(ClaimTypes.Email, user.Email??"")
        ////    };

        ////    var userClaims = await _userManager.GetClaimsAsync(user);
        ////    var userRoles = await _userManager.GetRolesAsync(user);
        ////    claims.AddRange(userClaims);
        ////    foreach (var userRole in userRoles)
        ////    {
        ////        claims.Add(new Claim(ClaimTypes.Role, userRole));
        ////        var role = await roleManager.FindByNameAsync(userRole);
        ////        if (role != null)
        ////        {
        ////            var roleClaims = await roleManager.GetClaimsAsync(role);
        ////            foreach (Claim roleClaim in roleClaims)
        ////            {
        ////                claims.Add(roleClaim);
        ////            }
        ////        }
        ////    }

        ////    var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(encryptionDecryptionHelper.DecryptString(appSettings.Security.TokenSecret)));
        ////    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        ////    var tokenDescriptor = new SecurityTokenDescriptor()
        ////    {
        ////        Subject = new ClaimsIdentity(claims),
        ////        Expires = rememberMe ? DateTime.Now.AddYears(1) : DateTime.Now.AddDays(1),
        ////        SigningCredentials = creds
        ////    };

        ////    var tokenHandler = new JwtSecurityTokenHandler();
        ////    var token = tokenHandler.CreateToken(tokenDescriptor);
        ////    return tokenHandler.WriteToken(token);
        ////}

        //public async Task<LoginResponse> GetAccessToken(User user, string secretCode)
        //{
        //    /*var handler = new HttpClientHandler() { UseDefaultCredentials = false };

        //    var client = new HttpClient(handler);
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
        //    return await client.RequestPasswordTokenAsync(new PasswordTokenRequest
        //    {
        //        Address = baseUrl,
        //        UserName = request.Username,
        //        Password = request.Password,
        //        ClientId = request.ClientId,
        //        GrantType = GrantType.ResourceOwnerPassword
        //    }).ConfigureAwait(false);*/

        //    if (user == null)
        //        throw new ArgumentNullException(nameof(user));

        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var key = Encoding.ASCII.GetBytes(secretCode);

        //    var roles = await _userManager.GetRolesAsync(user);
        //    var role = roles.Any() ? roles.First() : Roles.User;
        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(new Claim[]
        //        {
        //                new Claim(ClaimTypes.Name, user.FirstName),
        //                new Claim(ClaimTypes.Role, role),
        //                new Claim(JwtRegisteredClaimNames.Jti, user.Id)
        //        }),
        //        Expires = DateTime.UtcNow.AddHours(2),
        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        //    };

        //    var token = tokenHandler.CreateToken(tokenDescriptor);

        //    return new LoginResponse
        //    {
        //        AccessToken = tokenHandler.WriteToken(token),
        //        DisplayName = $"{user.FirstName} {user.LastName}",
        //        ExpireDate = DateTime.UtcNow.AddHours(2)
        //    };
        //}






        #endregion
    }
}
