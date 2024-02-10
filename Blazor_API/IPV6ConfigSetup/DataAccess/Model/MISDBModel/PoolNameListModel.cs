using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace IPV6ConfigSetup.DataAccess.Model.MISDBModel
{
    [Keyless]
    public class PoolNameListModel
    {
        public string? PoolName { get; set; }
        public string? VLAN { get; set; }
    }
}
