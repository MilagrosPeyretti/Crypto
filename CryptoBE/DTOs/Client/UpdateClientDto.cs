using System.ComponentModel.DataAnnotations;

namespace APICrypto.DTOs.Client
{
    public class UpdateClientDto
    {
        [StringLength(250, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 250 caracteres.")]
        public string? Name { get; set; }

        [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
        public string? Email { get; set; }
    }
}
