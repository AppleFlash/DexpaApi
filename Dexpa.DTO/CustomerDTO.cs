using System.ComponentModel.DataAnnotations;

namespace Dexpa.DTO
{
    public class CustomerDTO
    {
        public long Id { get; set; }

        [RegularExpression(@"[@А-Яа-яA-Za-z\W]+", ErrorMessage = "Имя клиента может состоять только из букв русского или английского алфавита")]
        public string Name { get; set; }

        public string Phone { get; set; }

        public bool ToBlackList { get; set; }

        public virtual OrganizationDTO Organization { get; set; }

        public long? OrganizationId { get; set; }
    }
}
