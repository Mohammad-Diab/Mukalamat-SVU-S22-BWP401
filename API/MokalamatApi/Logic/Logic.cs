using System.Text.Json;

namespace MokalamatApi
{
    public static class Logic
    {
        public static bool InitializeData()
        {
            DirectoryInfo info = new DirectoryInfo("Database");
            if (!info.Exists)
            {
                try
                {
                    Directory.CreateDirectory("Database");
                }
                catch (Exception)
                {
                    return false;
                }
            }
            FileInfo database = new FileInfo(Path.Combine(info.FullName, "data.json"));
            if (!database.Exists)
            {
                try
                {
                    database.Create().Close();
                    // add admin user if not exists
                    User admin = new User("admin", "Mr.", "Admin", "User", "0935798343", "admin@mokalamat.com", "Male", "19930101000101", "Damascus", "Damascus City", "123", "Other", true);
                    List<User> users = new List<User>
                    {
                        admin
                    };

                    User.CommitData(users);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool CreateUser(string username,  string title,  string firstName,
             string lastName,  string phoneNumber,  string email,  string gender,  string birthday,
             string city,  string address,  string password,  string occupation)
        {
            var users = User.GetUsersList();
            var isExists = users.Find(x => x.Username == username || x.PhoneNumber == phoneNumber) is not null;
            if (!isExists)
            {
                birthday = Helper.DateTimeToChar14(DateTime.Parse(birthday));
                User newUser = new User(username, title, firstName, lastName, phoneNumber, email, gender, birthday, city, address, password, occupation);
                users.Add(newUser);
                User.CommitData(users);
                return true;
            }
            else
            {
                throw new Exception("User is already exists");
            }
        }

        internal static double GetMyBill(string username, string phoneNumber)
        {
            var users = User.GetUsersList();
            var user = users.Find(x => x.Username == username && x.PhoneNumber == phoneNumber);
            if(user != null)
            {
                return user.Bill;
            }
            else
            {
                throw new Exception("This phone number is not registered yet.");
            }
        }

        internal static bool SetUserBill(string phoneNumber, double bill)
        {
            var users = User.GetUsersList();
            var user = users.Find(x => x.PhoneNumber == phoneNumber);
            if (user != null)
            {
                user.Bill = bill;
                User.CommitData(users);
                return true;
            }
            else
            {
                throw new Exception("This phone number is not registered yet.");
            }
        }

        internal static List<UserTable> GetAllUsers()
        {
            List<User> users = User.GetUsersList();
            return users.Select(x => new UserTable(x)).ToList();
        }
    }
}
