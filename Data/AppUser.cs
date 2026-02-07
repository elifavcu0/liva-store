using Microsoft.AspNetCore.Identity;

namespace liva_store.Data;

public class AppUser : IdentityUser<int>
{
    public string NameSurname { get; set; } = null!;
    public List<Address>? Addresses { get; set; }
}