using System.Collections.Generic;

namespace Dexpa.DTO
{
    public class ContentsObjDTO
    {
        public ContentDTO DriverPhoto { get; set; }
        public IList<ContentDTO> CarFronts { get; set; }
        public IList<ContentDTO> CarBacks { get; set; }
        public IList<ContentDTO> CarSides { get; set; }
        public IList<ContentDTO> CarInteriors { get; set; }
        public IList<ContentDTO> CarRegNumbers { get; set; }
    }
}