using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using helpack.Misc;

namespace helpack.Data.Entities;

public class Profile
{
    [Key]
    public int Id { get; set; }
    
    public int AuthorId { get; set; }
    
    [ForeignKey("AuthorId")]
    public HelpackUser? Author { get; set; }
    
    public string? Title { get; set; }
    
    public string? Description { get; set; }
    
    public string? ImageUrl { get; set; }
    
    public ProfileCategory? Category { get; set; }
    
    public double? Goal { get; set; }
    
    public string? GoalDescription { get; set; }

    public string? DonationTitle { get; set; }
    
    public string? DonationDescription { get; set; }
    
    public double? DonationsRaised { get; set; }

    public virtual ICollection<Donation> Donations { get; set; }
}