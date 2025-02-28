using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace module_user.Models;

public partial class Role
{
    public int TenantId { get; set; }

    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Displayname { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? LastUpdate { get; set; }

    [JsonIgnore]

    public ICollection<UserMembership>? UserMemberships { get; set; }
}
