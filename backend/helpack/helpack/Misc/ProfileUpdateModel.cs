

namespace helpack.Misc;

public class ProfileUpdateModel
{
    public int Id { get; set; }
    
    public int AuthorId { get; set; }
    
    public string Title { get; set; }
    
    public string Description { get; set; }
    
    public IFormFile? Image { get; set; }
    
    public ProfileCategory? Category { get; set; }
    
    public double Goal { get; set; }
    
    public string? GoalDescription { get; set; }

    public string DonationTitle { get; set; }
    
    public string DonationDescription { get; set; }
}