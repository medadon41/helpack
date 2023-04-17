using helpack.Data.Entities;
using helpack.Misc;

namespace helpack.DTO;

public class ProfileInsightsViewModel
{
    public int Id { get; set; }
    
    public string Author { get; set; }
    
    public string RegistrationDate { get; set; }

    public string? ImageUrl { get; set; }
    
    public virtual IEnumerable<DonationViewModel> Donations { get; set; }
}