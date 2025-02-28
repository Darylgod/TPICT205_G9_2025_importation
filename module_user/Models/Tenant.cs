using System;
using System.Collections.Generic;

namespace module_user.Models;

public partial class Tenant
{
    public int Id { get; set; }

    public DateTime? Created { get; set; }

    public string? CreateBy { get; set; }

    public string? Description { get; set; }
}
