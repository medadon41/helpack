﻿using System.ComponentModel.DataAnnotations;

namespace helpack.Data.Entities;

public class HelpackUser
{
    [Key]
    public int Id { get; set; }
    
    public string Email { get; set; }
    
    public string UserName { get; set; }
    
    public string Password { get; set; }
    
    public DateTime RegistrationDate { get; set; } = DateTime.Now;
}