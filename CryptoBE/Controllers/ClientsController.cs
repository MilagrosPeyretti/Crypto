using APICrypto.Data;
using APICrypto.DTOs;
using APICrypto.DTOs.Client;
using APICrypto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace APICrypto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClientsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateClient([FromBody] CreateClientDto createDto)
       
        {
            
            var cliente = new Client
            {
                Name = createDto.Name,
                Email = createDto.Email
            };

            _context.Clients.Add(cliente);
            await _context.SaveChangesAsync();

            var responseDto = new ClientResponseDto
            {
                Id = cliente.Id,
                Name = cliente.Name,
                Email = cliente.Email
            };

            return CreatedAtAction(nameof(GetClient), new { id = cliente.Id }, responseDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClientResponseDto>> GetClient(int id)
        {
            var cliente = await _context.Clients.FindAsync(id);

            if (cliente == null) return NotFound();

            return new ClientResponseDto
            {
                Id = cliente.Id,
                Name = cliente.Name,
                Email = cliente.Email
            };
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientResponseDto>>> GetClients()
        {
            var clients = await _context.Clients.ToListAsync();
            var clientDto = clients.Select(c => new ClientResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                Email = c.Email
            }).ToList();

            return Ok(clientDto);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateClient(int id, [FromBody] UpdateClientDto dto)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
                return NotFound($"Cliente con ID {id} no encontrado.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!string.IsNullOrEmpty(dto.Name))
                client.Name = dto.Name;

            if (!string.IsNullOrEmpty(dto.Email))
                client.Email = dto.Email;

            await _context.SaveChangesAsync();

            var result = new ClientResponseDto
            {
                Id = client.Id,
                Name = client.Name,
                Email = client.Email
            };

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var client = await _context.Clients
                .Include(c => c.Transactions)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (client == null)
                return NotFound($"Cliente con ID {id} no encontrado.");

            if (client.Transactions.Any())
                return BadRequest("No se puede eliminar un cliente con transacciones registradas.");

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}