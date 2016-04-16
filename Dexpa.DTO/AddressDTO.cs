namespace Dexpa.DTO
{
    public class AddressDTO
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

        public string FullName
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(City))
                {
                    var fullName = !string.IsNullOrWhiteSpace(City) ? City : "";
                    fullName += !string.IsNullOrWhiteSpace(Street) ? (", " + Street) : "";
                    fullName += !string.IsNullOrWhiteSpace(House) ? (", д." + House) : "";
                    fullName += !string.IsNullOrWhiteSpace(Housing) ? (", к." + Housing) : "";
                    fullName += !string.IsNullOrWhiteSpace(Building) ? (", строение" + Building) : "";
                    fullName += !string.IsNullOrWhiteSpace(Staircase) ? (", подъезд" + Staircase) : "";

                    return fullName;
                }

                return Comment;
            }
        }
    }
}