using System;
using Dexpa.DTO.HelpDictionaries;

namespace Dexpa.DTO
{
    public class ContentDTO
    {
        public string Id { get; set; }

        public DexpaContentTypeDTO Type { get; set; }

        public string WebUrl { get; set; }

        public string WebUrlSmall { get; set; }

        public DateTime TimeStamp { get; set; }

    }
}