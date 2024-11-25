using FootballPlayers.Data;
using FootballPlayers.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FootballPlayers.Pages
{
    public class EditPlayerModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditPlayerModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Player Player { get; set; }

        public List<Team> Teams { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Player = await _context.Players.FindAsync(id);
            if (Player == null)
            {
                return NotFound();
            }

            Teams = await _context.Teams.ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {


            _context.Attach(Player).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayerExists(Player.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("/PlayerList");
        }

        private bool PlayerExists(int id)
        {
            return _context.Players.Any(e => e.Id == id);
        }
    }
}