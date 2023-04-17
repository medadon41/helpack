using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace helpack.Data.Entities;

public class Donation
{
    [Key]
    public int Id { get; set; }
    
    public int ReceiverId { get; set; }
    
    [ForeignKey("ReceiverId")]
    public virtual Profile? Receiver { get; set; }
    
    public double Amount { get; set; }
    
    public DateTime Date { get; set; } = DateTime.Now;
    
    public string Name { get; set; }
    
    public string? Message { get; set; }
}