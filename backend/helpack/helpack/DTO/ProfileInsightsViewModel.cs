using helpack.Data.Entities;
using helpack.Misc;

namespace helpack.DTO;

public class ProfileInsightsViewModel
{
    public int Id { get; set; }
    
    public HelpackUser Author { get; set; }

    public string? ImageUrl { get; set; }
    
    public virtual ICollection<Donation> Donations { get; set; }
}