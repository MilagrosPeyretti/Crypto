using System.ComponentModel.DataAnnotations;

namespace APICrypto.Models
{
    public class Client
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(250, MinimumLength = 3)]
        public string Name { get; set; } 

        [Required]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        public string Email { get; set; }
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
