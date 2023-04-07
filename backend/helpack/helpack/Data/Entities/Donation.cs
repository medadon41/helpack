using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace helpack.Data.Entities;

public class Donation
{
    [Key]
    public int Id { get; set; }
    
    public virtual Profile Receiver { get; set; }
    
    public double Amount { get; set; }
    
    public DateTime Date { get; set; }
    
    public string Name { get; set; }
    
    public string? Message { get; set; }
}