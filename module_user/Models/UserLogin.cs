using System;
using System.Collections.Generic;

namespace module_user.Models;

public partial class UserLogin
{
    public int TenantId { get; set; }

    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Displayname { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? LastUpdate { get; set; }
}
