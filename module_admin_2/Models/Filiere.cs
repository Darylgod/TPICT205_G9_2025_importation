using System;
using System.Collections.Generic;

namespace module_admin_2.Models;

public partial class Filiere
{
    public int IdFiliere { get; set; }

    public string CodeFiliere { get; set; } = null!;

    public string Intitule { get; set; } = null!;

    public string? Tittle { get; set; }

    public int IdTenant { get; set; }
}
