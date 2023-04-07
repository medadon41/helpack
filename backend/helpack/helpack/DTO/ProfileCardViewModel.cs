using helpack.Data.Entities;
using helpack.Misc;

namespace helpack.DTO;

public class ProfileCardViewModel
{
    public int Id { get; set; }
    
    public string Author { get; set; }
    
    public string Title { get; set; }

    public string? ImageUrl { get; set; }
    
    public ProfileCategory Category { get; set; }
    
    public int ProgressReached { get; set; }
}