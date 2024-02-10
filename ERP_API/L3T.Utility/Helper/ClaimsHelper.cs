using System.Security.Claims;

namespace L3T.Utility.Helper
{
    public static class ClaimsHelper
    {
        public static string GetClaimUserId(this ClaimsPrincipal claims)
        {
            try
            {
                var id = claims.Claims.Where(c => c.Type == "sub")
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
                var id = claims.Claims.Where(c => c.Type == "UserL3Id")
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