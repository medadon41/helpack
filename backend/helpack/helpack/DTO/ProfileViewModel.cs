using helpack.Data.Entities;
using helpack.Misc;

namespace helpack.DTO;

public class ProfileViewModel
{
    public int Id { get; set; }
    
    public HelpackUser Author { get; set; }
    
    public string Title { get; set; }
    
    public string Description { get; set; }
    
    public string? ImageUrl { get; set; }
    
    public string Category { get; set; }
    
    public double Goal { get; set; }
    
    public string? GoalDescription { get; set; }

    public virtual IEnumerable<DonationScoreboardViewModel> Donations { get; set; }
}