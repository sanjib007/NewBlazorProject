using System;

namespace L3TIdentityServer.Models.Account
{
    public class AccountOptions
    {
        public static string AddRoleSuccessfully = "-10";
        public static bool AllowLocalLogin = true;
        public static bool AllowRememberLogin = true;
        public static TimeSpan RememberMeLoginDuration = TimeSpan.FromDays(30);
        public static bool ShowLogoutPrompt = true;
        public static bool AutomaticRedirectAfterSignOut = false;
        public static string BadRequest = "-100";
        public static string InvalidCredentialsErrorMessage = "-200";
        public static string InvalidUsername = "-300";
        public static string InvalidRoleName = "-400";
        public static string AccountLocked = "-500";
        public static string LoginSuccessfully = "-510,{0},{1}";
        public static string RaisedError = "-520,{0},{1},{2}";
        public static string ExecutionActionRaisedError = "-530,{0},{1},{2}";
        public static string PublicErrorMessage = "-600";
        public static string FirstNameIsRequired = "-700";
        public static string LastNameIsRequired = "-800";
        public static string UsernameIsRequired = "-900";
        public static string RolenameIsRequired = "-1000";
        public static string PasswordIsRequired = "-1100";
        public static string EmailIsRequired = "-1200";
        public static string PhoneNumberIsRequired = "-1300";
        public static string EmailIsAlreadyExists = "-1400";
        public static string UsernameIsAlreadyExists = "-1500";
        public static string UserRegisterSuccessfully = "-1600";
        public static string EmailAddressIncorrect = "-1700";
        public static string EmailConfirmation = "-1800";
        public static string UpdateProfileSuccessfully = "-1900";
        public static string DeactivationUserSuccessfully = "-2000";
        public static string AddUserClaimSuccessfully = "-2100";
        public static string RemoveUserClaimSuccessfully = "-2200";

        public static string AddUserToRoleSuccessfully = "-2300";
        public static string RemoveUserFromRoleSuccessfully = "-2400";

        public static string AddUserToRolesSuccessfully = "-2500";
        public static string RemoveUserFromRolesSuccessfully = "-2600";

        public static string ChangingPasswordSuccessfully = "-2700";
        public static string UsernameOrEmailIsIncorrect = "-2800";

        public static string ResetPasswordSubject = "-2900";
        public static string ConfirmationPasswordSubject = "-3000";
        public static string CheckYoutEmailForChangeEmail = "-3100";
        public static string ResetPasswordBody = "-3200 \n {0}";
        public static string ConfirmationEmailBody = "-3300 \n {0}";
        public static string EmailProviderServiceDown = "-3400,{0},{1}";
        public static string ResetPasswordLinkSent = "-3500";
        public static string ResetingPasswordSuccessfully = "-3600";
        public static string LoggedOutSuccessfully = "-3700";

        public static string BeginTransactionRegister = "-3800,{0},{1}";
        public static string RoleNameIsDuplicte = "-3900";
        public static string DeleteRoleSuccessfully = "-4000";

        public static string InvalidUserID = "-4100";
        public static string UserNotFound = "-4200";
        public static string PhoneNumberNotConfirm = "-4300";
        internal static string PasswordIsIncorrect = "-4400";
    }
}
