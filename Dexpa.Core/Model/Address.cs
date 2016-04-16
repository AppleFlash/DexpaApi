using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json.Converters;

namespace Dexpa.Core.Model
{
    [ComplexType]
    public class Address
    {
        public string City { get; set; }
        public string Street { get; set; }
        public string House { get; set; } //дом
        public string Housing { get; set; } //корпус
        public string Building { get; set; } //строение
        public string Staircase { get; set; } //подъезд
        public string Comment { get; set; } //комментарий

        public double? Longitude { get; set; }

        public double? Latitude { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                var fullName = !string.IsNullOrWhiteSpace(City) ? City : "";
                fullName += !string.IsNullOrWhiteSpace(Street) ? (", " + Street) : "";
                fullName += !string.IsNullOrWhiteSpace(House) ? (", д." + House) : "";
                fullName += !string.IsNullOrWhiteSpace(Housing) ? (", к." + Housing) : "";
                fullName += !string.IsNullOrWhiteSpace(Building) ? (", строение" + Building) : "";
                fullName += !string.IsNullOrWhiteSpace(Staircase) ? (", подъезд" + Staircase) : "";

                return fullName;
            }
        }

        public bool IsAirport { get; set; }

        public override string ToString()
        {
            return FullName;
        }
    }
}