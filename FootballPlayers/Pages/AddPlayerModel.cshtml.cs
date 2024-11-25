using FootballPlayers.Data;
using FootballPlayers.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FootballPlayers.Pages
{
    public class AddPlayerModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AddPlayerModel> _logger;

        public AddPlayerModel(ApplicationDbContext context, ILogger<AddPlayerModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        [BindProperty]
        public Player Player { get; set; }

        [BindProperty]
        public string? NewTeam { get; set; }

        public List<Team> Teams { get; set; }

        public async Task OnGetAsync()
        {
            Teams = await _context.Teams.ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            _logger.LogInformation("OnPostAsync started");

            if (string.IsNullOrEmpty(NewTeam) && !Player.TeamId.HasValue)
            {
                ModelState.AddModelError("Player.TeamId", "You must select a team or add a new one.");
                Teams = await _context.Teams.ToListAsync();
                _logger.LogWarning("Validation failed: No team selected or added");
                return Page();
            }

            try
            {
                if (!string.IsNullOrEmpty(NewTeam))
                {
                    _logger.LogInformation("Adding new team: {NewTeam}", NewTeam);
                    var existingTeam = await _context.Teams.FirstOrDefaultAsync(t => t.Name == NewTeam);

                    if (existingTeam == null)
                    {
                        var team = new Team { Name = NewTeam };
                        _context.Teams.Add(team);
                        await _context.SaveChangesAsync();
                        Player.TeamId = team.Id;
                        _logger.LogInformation("New team {NewTeam} added with ID {TeamId}", NewTeam, team.Id);
                    }
                    else
                    {
                        Player.TeamId = existingTeam.Id;
                        _logger.LogInformation("Existing team {NewTeam} found with ID {TeamId}", NewTeam, existingTeam.Id);
                    }
                }

                _context.Players.Add(Player);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Player {FirstName} {LastName} added successfully", Player.FirstName, Player.LastName);

                return RedirectToPage("/PlayerList");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding player");
                Teams = await _context.Teams.ToListAsync();
                return Page();
            }
        }
    }
}