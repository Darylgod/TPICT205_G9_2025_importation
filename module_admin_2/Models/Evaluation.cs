using System;
using System.Collections.Generic;

namespace module_admin_2.Models;

public partial class Evaluation
{
    public int IdEvaluation { get; set; }

    public int IdEtudiant { get; set; }

    public int IdProgramme { get; set; }

    public decimal Note { get; set; }

    public int IdTenant { get; set; }
}
