using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace module_user.Models;

public partial class UserContactinfo
{
    public int TenantId { get; set; }

    public int Id { get; set; }

    public int Userid { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? WhatsApp { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? Country { get; set; }

    public string CreateBy { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public DateTime? LastUpdate { get; set; }
    [JsonIgnore]
    public User? User { get; set; }  // Relation avec User
}
