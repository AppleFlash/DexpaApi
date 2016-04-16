using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexpa.Core
{
    public enum ErrorCode
    {
        Custom = 0,
        /// <summary>
        /// Customer with same phone already exists
        /// </summary>
        CustomerCreateExistsPhone,
        ItemNotExists
    }
}
