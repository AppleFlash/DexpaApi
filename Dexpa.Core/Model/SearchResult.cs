using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dexpa.Core.Model
{
    public class SearchResult
    {
        public Driver Driver { get; set; }

        public Car Car { get; set; }

        public Order Order { get; set; }

        public object MapObject { get; set; }
    }
}
