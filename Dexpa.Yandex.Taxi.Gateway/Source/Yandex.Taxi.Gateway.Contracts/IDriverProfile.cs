using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yandex.Taxi.Gateway.Contracts
{
    public interface IDriverProfile
    {
        string Uuid { get; }

        /// <summary>
        /// Поддерживаемые водителем тарифы
        /// </summary>
        IEnumerable<string> Tarrifs { get; }

        ICar Car { get; }

        IDriver Driver { get; }

    }
}
