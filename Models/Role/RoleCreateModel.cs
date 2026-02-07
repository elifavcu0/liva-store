using System.ComponentModel.DataAnnotations;

namespace liva_store.models;

public class RoleCreateModel
{
    [Display(Name = "Role Name")]
    [Required]
    public string Name { get; set; } = null!;
}