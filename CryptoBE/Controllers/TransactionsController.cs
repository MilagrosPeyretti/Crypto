using APICrypto.Data;
using APICrypto.DTOs.Trasaction;
using APICrypto.DTOs.Client;
using APICrypto.Models;
using APICrypto.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICrypto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ICryptoYaService _cryptoService;

        public TransactionsController(AppDbContext context, ICryptoYaService cryptoService)
        {
            _context = context;
            _cryptoService = cryptoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionResponseDto>>> GetTransactions([FromQuery] int? clientId)
        {
            var query = _context.Transactions.Include(t => t.Client).AsQueryable(); 

            if (clientId.HasValue)
            {
                query = query.Where(t => t.ClientId == clientId.Value);
            }

            var transactions = await query.OrderByDescending(t => t.DateTime).ToListAsync();

            var response = transactions.Select(t => new TransactionResponseDto
            {
                Id = t.Id,
                CryptoCode = t.CryptoCode,
                Action = t.Action,
                ClientId = t.ClientId,
                ClientName = t.Client.Name,
                CryptoAmount = t.CryptoAmount,
                Money = t.Money,
                DateTime = t.DateTime
            }).ToList();

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionResponseDto>> GetTransaction(int id)
        {
            var transaction = await _context.Transactions
                .Include(t => t.Client) 
                .FirstOrDefaultAsync(t => t.Id == id);

            if (transaction == null)
            {
                return NotFound();
            }

            var transactionDto = new TransactionResponseDto
            {
                Id = transaction.Id,
                CryptoCode = transaction.CryptoCode,
                Action = transaction.Action,
                ClientId = transaction.ClientId,
                ClientName = transaction.Client.Name, 
                CryptoAmount = transaction.CryptoAmount,
                Money = transaction.Money,
                DateTime = transaction.DateTime,
            };

            return Ok(transactionDto);
        }

        [HttpPost]
        public async Task<ActionResult<TransactionResponseDto>> CreateTransaction(CreateTransactionDto createDto)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var allowedCodes = new[] { "BTC", "ETH", "USDT", "USDC", "DAI" };
            if (!allowedCodes.Contains(createDto.CryptoCode?.ToUpper()))
                return BadRequest("CryptoCode inválido. Debe ser uno de: BTC, ETH, USDT, USDC, DAI");

            var allowedExchanges = new[] { "buenbit", "binance", "satoshitango" };
            if (!allowedExchanges.Contains(createDto.Exchange?.ToLower()))
                return BadRequest("Exchange inválido. Debe ser uno de: buenbit, binance, satoshitango");


            var client = await _context.Clients
                .Include(c => c.Transactions)
                .FirstOrDefaultAsync(c => c.Id == createDto.ClientId);

            if (client == null)
                return NotFound($"Cliente con ID {createDto.ClientId} no encontrado.");

            if (createDto.Action == "sale")
            {
                var totalComprado = client.Transactions
                    .Where(t => t.CryptoCode == createDto.CryptoCode && t.Action == "purchase")
                    .Sum(t => t.CryptoAmount);

                var totalVendido = client.Transactions
                    .Where(t => t.CryptoCode == createDto.CryptoCode && t.Action == "sale")
                    .Sum(t => t.CryptoAmount);

                var saldoDisponible = totalComprado - totalVendido;

                if (createDto.CryptoAmount > saldoDisponible)
                {
                    return BadRequest($"Saldo insuficiente de {createDto.CryptoCode}. Disponible: {saldoDisponible}");
                }
            }

            var precioActual = await _cryptoService.GetPriceAsync(createDto.Exchange, createDto.CryptoCode);
            if (precioActual == null)
                return StatusCode(500, "Error al obtener el precio de CriptoYa.");

            var montoTotal = Math.Round(createDto.CryptoAmount * precioActual.Value, 2);

            var transaction = new Transaction
            {
                CryptoCode = createDto.CryptoCode.ToUpper(),
                Action = createDto.Action,
                ClientId = createDto.ClientId,
                CryptoAmount = createDto.CryptoAmount,
                DateTime = createDto.DateTime,
                Money = montoTotal
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            var result = new TransactionResponseDto
            {
                Id = transaction.Id,
                CryptoCode = transaction.CryptoCode.ToUpper(),
                ClientId = transaction.ClientId,
                Action = transaction.Action,
                CryptoAmount = transaction.CryptoAmount,
                Money = transaction.Money,
                DateTime = transaction.DateTime
            };

            return CreatedAtAction(nameof(GetTransaction), new { id = result.Id }, result);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTransactionDto dto)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
                return NotFound($"Transacción con ID {id} no encontrada.");

            if (dto.CryptoAmount.HasValue)
                transaction.CryptoAmount = dto.CryptoAmount.Value;

            if (dto.Money.HasValue)
                transaction.Money = dto.Money.Value;

            if (dto.DateTime.HasValue)
                transaction.DateTime = dto.DateTime.Value;

            if (!string.IsNullOrEmpty(dto.Action)) transaction.Action = dto.Action;

            if (!string.IsNullOrEmpty(dto.CryptoCode)) transaction.CryptoCode = dto.CryptoCode.ToUpper();

            await _context.SaveChangesAsync();

            var result = new TransactionResponseDto
            {
                Id = transaction.Id,
                CryptoCode = transaction.CryptoCode,
                Action = transaction.Action,
                ClientId = transaction.ClientId,
                CryptoAmount = transaction.CryptoAmount,
                Money = transaction.Money,
                DateTime = transaction.DateTime
            };

            return Ok(result);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
                return NotFound($"Transacción con ID {id} no encontrada.");

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();

            var result = new TransactionResponseDto
            {
                Id = transaction.Id,
                CryptoCode = transaction.CryptoCode,
                Action = transaction.Action,
                CryptoAmount = transaction.CryptoAmount,
                Money = transaction.Money,
                DateTime = transaction.DateTime
            };

            return Ok(result);
        }

        [HttpGet("summary/{clientId}")]
        public async Task<ActionResult<ClientSummaryDto>> GetSummary(int clientId)
        {
            var client = await _context.Clients
                .Include(c => c.Transactions)
                .FirstOrDefaultAsync(c => c.Id == clientId);

            if (client == null)
                return NotFound($"Cliente con ID {clientId} no encontrado.");

            var grouped = client.Transactions
                .GroupBy(t => t.CryptoCode)
                .Select(g => new
                {
                    CryptoCode = g.Key,
                    NetAmount = g.Where(t => t.Action == "purchase").Sum(t => t.CryptoAmount)
                      - g.Where(t => t.Action == "sale").Sum(t => t.CryptoAmount)
                })
                .Where(x => x.NetAmount > 0)
                .ToList();

            var summary = new ClientSummaryDto();

            foreach (var cripto in grouped)
            {
                var price = await _cryptoService.GetPriceSumary(cripto.CryptoCode);
                if (price == null) continue;

                var valueARS = Math.Round(cripto.NetAmount * price.Value, 2);

                summary.Cryptos.Add(new CryptoSummaryDto
                {
                    CryptoCode = cripto.CryptoCode,
                    Amount = cripto.NetAmount,
                    ValueARS = valueARS
                });

                summary.TotalARS += valueARS;
            }

            return Ok(summary);
        }
    }
}