using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.Mikrotik.SocketModel
{
    public class ChartDtoInfo
    {
        public string TimeLabel { get; set; }
        public decimal RxValue { get; set; }
        public decimal TxValue { get; set; }
    }
}
