using System;
using System.Collections.Generic;

namespace module_admin_2.Models;

public partial class Programme
{
    public int IdProgramme { get; set; }

    public string Nom { get; set; } = null!;

    public int IdClasse { get; set; }

    public int IdTenant { get; set; }
}
