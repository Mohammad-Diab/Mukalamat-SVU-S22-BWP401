namespace MokalamatApi
{
    public class UserTable
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Birthday { get; set; }
        public string Bill { get; set; }
        public bool IsAdmin { get; set; }

        public UserTable(User user)
        {
            Id= user.Id;
            Username = user.Username;
            FullName = user.FirstName + " " + user.LastName;
            Phone = user.PhoneNumber;
            Birthday = Helper.ToStandardDate(user.Birthday);
            Bill = user.Bill + " SP";
            IsAdmin = user.IsAdmin;
        }

        public UserTable()
        {
            Username = string.Empty;
            FullName = string.Empty;
            Phone = string.Empty;
            Birthday = string.Empty;
            Bill = string.Empty;
        }

        public UserTable(int id, string username, string fullName, string phone, string birthday, string bill, bool isAdmin)
        {
            Id = id;
            Username = username;
            FullName = fullName;
            Phone = phone;
            Birthday = birthday;
            Bill = bill;
            IsAdmin = isAdmin;
        }
    }
}
