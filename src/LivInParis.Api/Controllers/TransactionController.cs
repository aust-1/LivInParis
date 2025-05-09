using LivInParisRoussilleTeynier.Services;
using Microsoft.AspNetCore.Mvc;

namespace LivInParisRoussilleTeynier.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionController(ITransactionService transactionService) : ControllerBase
{
    private readonly ITransactionService _transactionService = transactionService;

    /// <summary>
    /// Retrieves all transactions for a customer.
    /// </summary>
    [HttpGet("customers/{customerId}")]
    public async Task<ActionResult<IEnumerable<TransactionDto>>> GetByCustomer(int customerId)
    {
        var txs = await _transactionService.GetTransactionsByCustomerAsync(customerId);
        return Ok(txs);
    }

    /// <summary>
    /// Retrieves a transaction by id.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<TransactionDto>> GetById(int id)
    {
        var tx = await _transactionService.GetTransactionByIdAsync(id);
        return Ok(tx);
    }
}
