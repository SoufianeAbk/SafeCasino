using Microsoft.AspNetCore.Identity;

namespace SafeCasino.Data.Identity
{
    /// <summary>
    /// Custom rol klasse voor Identity
    /// </summary>
    public class ApplicationRole : IdentityRole
    {
        /// <summary>
        /// Beschrijving van de rol
        /// </summary>
        public string? Description { get; set; }
    }
}