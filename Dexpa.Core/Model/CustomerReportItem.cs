using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dexpa.Core.Model
{
    public class CustomerReportItem
    {
        public long Id { get; set; }

        public Customer Customer { get; set; }

        public int Orders { get; set; }
    }
}
