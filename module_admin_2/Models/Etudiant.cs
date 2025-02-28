using System;
using System.Collections.Generic;

namespace module_admin_2.Models;

public partial class Etudiant
{
    public int IdEtudiant { get; set; }

    public string Matricule { get; set; } = null!;

    public string Nom { get; set; } = null!;

    public string Prenom { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateOnly DateNaissance { get; set; }

    public string VilleNaissance { get; set; } = null!;

    public string? Telephone { get; set; }

    public string? Adresse { get; set; }

    public int IdClasse { get; set; }

    public int IdTenant { get; set; }
}
