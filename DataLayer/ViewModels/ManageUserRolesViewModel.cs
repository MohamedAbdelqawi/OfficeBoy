namespace DataLayer.ViewModels
{
    public class ManageUserRolesViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public IList<string> UserRoles { get; set; }
        public IList<string> AllRoles { get; set; }
        public IList<string> SelectedRoles { get; set; }
    }

}
