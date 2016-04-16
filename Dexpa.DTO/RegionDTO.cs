using System.Collections.Generic;
using Dexpa.Core.Model;

namespace Dexpa.DTO
{
    public class RegionDTO
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public List<RegionPointDTO> Points { get; set; }

        public RegionDTO()
        {
            Points = new List<RegionPointDTO>();
        }
    }
}
