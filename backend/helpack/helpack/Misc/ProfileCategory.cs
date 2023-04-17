using System.ComponentModel.DataAnnotations;

namespace helpack.Misc;

public enum ProfileCategory
{
    [Display(Name = "Unset")]
    Unset,
    [Display(Name = "Start-ups")]
    StartUps,
    [Display(Name = "Content makers")]
    ContentMakers,
    [Display(Name = "Charity")]
    Charity,
    [Display(Name = "Streaming")]
    Streaming,
    [Display(Name = "Development")]
    Development
}