using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.CommonModel
{
    public class GlobalApplicationException : Exception
    {

        public GlobalApplicationException()
        {
        }

        public GlobalApplicationException(string message)
             : base(message)
        {
        }

        public GlobalApplicationException(string message, Exception inner)
             : base(message, inner)
        {
        }

    }
}
