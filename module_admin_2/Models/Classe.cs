using System;
using System.Collections.Generic;

namespace module_admin_2.Models;

public partial class Classe
{
    public int IdClasse { get; set; }

    public int IdFiliere { get; set; }

    public int IdSpecialite { get; set; }

    public string CodeGrade { get; set; } = null!;

    public string CodeNiveau { get; set; } = null!;

    public int IdTenant { get; set; }
}
