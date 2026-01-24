using SafeCasino.Data.Entities;
using SafeCasino.Data.Identity;

namespace SafeCasino.Web.ViewModels.Admin
{
    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalGames { get; set; }
        public int PendingReviews { get; set; }
        public int ActiveUsers { get; set; }
    }

    public class UserManagementViewModel
    {
        public ApplicationUser User { get; set; }
        public List<string> Roles { get; set; }
        public bool IsLocked { get; set; }
    }

    public class UsersIndexViewModel
    {
        public List<UserManagementViewModel> Users { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string SearchQuery { get; set; }
    }

    public class UserDetailsViewModel
    {
        public ApplicationUser User { get; set; }
        public List<string> CurrentRoles { get; set; }
        public List<string> AvailableRoles { get; set; }
        public bool IsLocked { get; set; }
    }

    public class ReviewModerationViewModel
    {
        public List<Review> Reviews { get; set; }
        public bool ShowApproved { get; set; }
    }
}