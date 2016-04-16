using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yandex.Taxi.Gateway.Contracts
{
    public interface ICar
    {
        string Model { get; }

        /// <summary>
        /// Номер автомобиля
        /// </summary>
        string Number { get; }

        string Color { get; }

        /// <summary>
        /// Год выпуска
        /// </summary>
        int Year { get; }

        /// <summary>
        /// Поддерживаемые требования
        /// </summary>
        IEnumerable<CarRequirement> Requirements { get; }
    }
}
