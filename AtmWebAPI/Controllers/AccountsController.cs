using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AtmWebAPI.Models;

namespace AtmWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly AtmWebAPIDbContext _context;

        public AccountsController(AtmWebAPIDbContext context)
        {
            _context = context;
        }

        // GET: api/Accounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
        {

            return await _context.Accounts.ToListAsync();
        }

        // GET: api/Accounts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccount(int id)
        {
            var account = await _context.Accounts.FindAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            return account;
        }

        // PUT: api/Accounts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccount(int id, Account account)
        {
            if (id != account.Id)
            {
                return BadRequest();
            }

            _context.Entry(account).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Accounts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Account>> PostAccount(Account account)
        {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAccount", new { id = account.Id }, account);
        }
        [HttpPost("deposit")]
        public async Task<ActionResult<Account>> DepositMoney(int accountId, int amount, string bankNoteType)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if (account == null)
            {
                return NotFound();
            }

            // Validate that only 10, 20, or 50 euro notes are accepted
            if (bankNoteType != "Ten" && bankNoteType != "Twenty" && bankNoteType != "Fifty")
            {
                return BadRequest("Invalid bank note type");
            }

            // Check if the amount is a multiple of 10, 20, or 50
            if (amount % 10 != 0 || (bankNoteType == "Ten" && amount != 10) || (bankNoteType == "Twenty" && amount != 20) || (bankNoteType == "Fifty" && amount != 50))
            {
                return BadRequest("Invalid amount");
            }

            // Update the balance of the account
            account.Balance += amount;

            // Create a new transaction record
            var transaction = new Transaction
            {
                AccountId = account.Id,
                Amount = amount,
                Type = "Deposit",
                Timestamp = DateTime.Now
            };
            _context.Transactions.Add(transaction);

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Return the updated account information
            return Ok(new
            {
                Timestamp = transaction.Timestamp,
                Available = account.Balance,
                Fifty = account.Balance / 50,
                Twenty = (account.Balance % 50) / 20,
                Ten = (account.Balance % 50 % 20) / 10
            });
        }
        [HttpPost("withdraw")]
        public async Task<ActionResult<Account>> WithdrawMoney(int accountId, int amount)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if (account == null)
            {
                return NotFound();
            }

            // Validate that the account has sufficient funds to make the withdrawal
            if (account.Balance < amount)
            {
                return BadRequest("Insufficient funds");
            }

            // Validate that only multiple of 10 euro notes can be withdrawn
            if (amount % 10 != 0)
            {
                return BadRequest("Invalid amount");
            }

            // Update the balance of the account
            account.Balance -= amount;

            // Create a new transaction record
            var transaction = new Transaction
            {
                AccountId = account.Id,
                Amount = -amount,
                Type = "Withdrawal",
                Timestamp = DateTime.Now
            };
            _context.Transactions.Add(transaction);

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Return the updated account information
            return Ok(new
            {
                Timestamp = transaction.Timestamp,
                Available = account.Balance,
                Fifty = account.Balance / 50,
                Twenty = (account.Balance % 50) / 20,
                Ten = (account.Balance % 50 % 20) / 10
            });
        }


        [HttpGet("/{id}")]
        public IActionResult GetTransactionHistory(int id)
        {
            var transactions = _context.Transactions
                                    .Where(t => t.AccountId == id)
                                    .OrderByDescending(t => t.Timestamp)
                                    .ToList();

            int balance = 0;
            foreach (var trx in transactions)
            {
                if (trx.Type == "Deposit")
                {
                    balance += trx.Amount;
                }
                else if (trx.Type == "Withdraw")
                {
                    balance -= trx.Amount;
                }
            }

            var result = new
            {
                TransactionsHistory = transactions,
                Available = balance
            };

            return Ok(result);
        }


        // DELETE: api/Accounts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AccountExists(int id)
        {
            return _context.Accounts.Any(e => e.Id == id);
        }
    }
}
