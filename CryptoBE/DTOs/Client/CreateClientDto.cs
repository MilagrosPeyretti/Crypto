using System.ComponentModel.DataAnnotations;

namespace APICrypto.DTOs.Client
{
    public class CreateClientDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
        public string Email { get; set; }
    }
}
