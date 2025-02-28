using System;
using System.Collections.Generic;

namespace module_user.Models;

public partial class User
{
    public int TenantId { get; set; }

    public int Id { get; set; }

    public bool? Enable { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Title { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? LastUpdate { get; set; }

    public ICollection<UserMembership>? UserMemberships { get; set; }
    public virtual ICollection<UserContactinfo>? UserContactinfos { get; set; }
}
