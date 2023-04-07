using helpack.Misc;

namespace helpack.DTO;

public class ProfileSettingsViewModel
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    
    public string Description { get; set; }
    
    public string? ImageUrl { get; set; }
    
    public ProfileCategory Category { get; set; }
    
    public double Goal { get; set; }
    
    public string? GoalDescription { get; set; }

    public string DonationTitle { get; set; }
    
    public string DonationDescription { get; set; }
    
}