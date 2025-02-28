using System;
using System.Collections.Generic;

namespace module_admin_2.Models;

public partial class Niveau
{
    public int IdNiveau { get; set; }

    public string CodeNiveau { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int IdTenant { get; set; }
}
