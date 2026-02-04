using System.ComponentModel.DataAnnotations;

namespace dotnet_store.models;

public class RoleEditModel
{
    public int Id { get; set; }
    [Display(Name = "Role Name")]
    [Required]
    public string Name { get; set; } = null!;
}