using System;
using System.Collections.Generic;

namespace module_admin_2.Models;

public partial class Grade
{
    public int IdGrade { get; set; }

    public string CodeGrade { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int IdTenant { get; set; }
}
