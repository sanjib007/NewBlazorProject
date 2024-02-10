using System.Security.Claims;

namespace L3T.Utility.Helper
{
    public static class ClaimsHelper
    {
        public static string GetClaimUserId(this ClaimsPrincipal claims)
        {
            try
            {
                var id = claims.Claims.Where(c => c.Type == "name")
                    .Select(c => c.Value).FirstOrDefault();
                return id == null ? "0" : id; 
            }
            catch (Exception e)
            {
                return  null;
            }
        }

        public static string GetClaimUserL3Id(this ClaimsPrincipal claims)
        {
            try
            {
                var id = claims.Claims.Where(c => c.Type == "Subject")
                    .Select(c => c.Value).FirstOrDefault();
                return id == null ? "0" : id;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static string GetClaimUserFullName(this ClaimsPrincipal claims)
        {
            try
            {
                var id = claims.Claims.Where(c => c.Type == "FullName")
                    .Select(c => c.Value).FirstOrDefault();
                return id == null ? "0" : id;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static string GetClaimUserEmail(this ClaimsPrincipal claims)
        {
            try
            {
                var id = claims.Claims.Where(c => c.Type == "email")
                    .Select(c => c.Value).FirstOrDefault();
                return id == null ? "0" : id;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static string GetClaimUserDesignation(this ClaimsPrincipal claims)
        {
            try
            {
                var id = claims.Claims.Where(c => c.Type == "Designation")
                    .Select(c => c.Value).FirstOrDefault();
                return id == null ? "0" : id;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static string GetClaimUserDepartment(this ClaimsPrincipal claims)
        {
            try
            {
                var id = claims.Claims.Where(c => c.Type == "Department")
                    .Select(c => c.Value).FirstOrDefault();
                return id == null ? "0" : id;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static string GetClaimUserPhoneNo(this ClaimsPrincipal claims)
        {
            try
            {
                var id = claims.Claims.Where(c => c.Type == "PhoneNo")
                    .Select(c => c.Value).FirstOrDefault();
                return id == null ? "0" : id;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static string GetClaimUserRoles(this ClaimsPrincipal claims)
        {
            try
            {
                var id = claims.Claims.Where(c => c.Type == "role")
                    .Select(c => c.Value).FirstOrDefault();
                return id == null ? "0" : id;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    } 
}