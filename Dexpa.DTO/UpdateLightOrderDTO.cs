using System;
using Dexpa.Core.Model;
using Dexpa.Core.Model.Light;
using Dexpa.DTO.HelpDictionaries;

namespace Dexpa.DTO
{
    public class UpdateLightOrderDTO
    {
        public LightOrder Order { get; set; }

        public string UpdateCancelReason { get; set; }
    }
}
