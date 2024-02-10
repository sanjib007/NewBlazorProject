using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.OAuth2DotNet7.DataAccess.Model.Parmission.RequestModel
{
    public class MenuSetupRequestModel
    {
        public long Id { get; set; }
        public string? MenuName { get; set; }
        public string? MenuIcon { get; set; }
        public string? FeatureName { get; set; }
        public int ParentId { get; set; }
        public int MenuSequence { get; set; }
        public bool IsVisible { get; set; }
        public bool ShowInMenuItem { get; set; }
        public bool AllowAnonymous { get; set; }
    }
}
