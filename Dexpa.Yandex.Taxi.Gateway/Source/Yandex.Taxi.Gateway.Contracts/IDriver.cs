using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yandex.Taxi.Gateway.Contracts
{
    public interface IDriver
    {
        string Name { get; }

        string Phone { get; }

        /// <summary>
        /// Год рождения водителя
        /// </summary>
        int BirthYear { get; }

        /// <summary>
        /// Лицензия
        /// </summary>
        string Permit { get; }
    }
}
