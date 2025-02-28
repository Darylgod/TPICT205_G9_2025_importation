using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace module_user.Models;

public partial class UserMembership
{
    public int TenantId { get; set; }

    public int Id { get; set; }

    public int Userid { get; set; }

    public int Roleid { get; set; }

    public string AssignBy { get; set; } = null!;

    public DateTime AssignDate { get; set; }
    [JsonIgnore] // Empêche Swagger d'afficher User
    public virtual User? User { get; set; }

    [JsonIgnore] // Empêche Swagger d'afficher Role
    public virtual Role? Role { get; set; }
    // [JsonIgnore] // Empêche Swagger d'afficher Role
    // public Tenant Tenant { get; set; }

}
