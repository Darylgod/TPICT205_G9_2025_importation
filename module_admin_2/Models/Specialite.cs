using System;
using System.Collections.Generic;

namespace module_admin_2.Models;

public partial class Specialite
{
    public int IdSpecialite { get; set; }

    public string CodeSpecialite { get; set; } = null!;

    public string Intitule { get; set; } = null!;

    public string? Tittle { get; set; }

    public int IdTenant { get; set; }
}
