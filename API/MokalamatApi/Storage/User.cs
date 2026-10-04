using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace MokalamatApi
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string Birthday { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public string Occupation { get; set; }
        public string Token { get; set; }
        public string TokenTakenTime { get; set; }
        public string? LastLogin { get; set; }
        public double Bill { get; set; }
        public bool IsAdmin { get; set; }

        private static SHA256? hasher;

        private string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return string.Empty;
            if (hasher is null)
            {
                hasher = SHA256.Create();
            }
            byte[] textData = System.Text.Encoding.UTF8.GetBytes(password);
            var hash = hasher.ComputeHash(textData);
            return BitConverter.ToString(hash).Replace("-", String.Empty);
        }


        public User(string username, string title, string firstName, string lastName, string phoneNumber, string email, string gender, string birthday, string city, string address, string password, string occupation, bool isAdmin = false)
        {
            Id = GetNextId();
            Username = username;
            Title = title;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Email = email;
            Gender = gender;
            Birthday = birthday;
            City = city;
            Address = address;
            Password = HashPassword(password);
            Occupation = occupation;
            IsAdmin = isAdmin;
            Bill = 0;
            TokenTakenTime = "";
            LastLogin = "";
            Token = "";
        }

        public User(int id, string username, string title, string firstName, string lastName, string phoneNumber, string email, string gender, string birthday, string city, string address, string password, string occupation, string token, string tokenTakenTime, string lastLogin, double bill, bool isAdmin)
        {
            Id = id;
            Username = username;
            Title = title;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Email = email;
            Gender = gender;
            Birthday = birthday;
            City = city;
            Address = address;
            Password = password;
            Occupation = occupation;
            IsAdmin = isAdmin;
            Bill = bill;
            TokenTakenTime = tokenTakenTime;
            LastLogin = lastLogin;
            Token = token;
        }

        public User() { }

        public static List<User> GetUsersList()
        {
            try
            {
                var users = new List<User>();
                string path = Path.Combine("Database", "data.json");
                var stream = new StreamReader(File.OpenRead(path));
                var json = stream.ReadToEnd();
                stream.Close();
                var jUsers = JsonSerializer.Deserialize<List<User>>(json);
                if (jUsers != null)
                {
                    users.AddRange(jUsers);
                }
                return users;
            }
            catch(Exception) { throw; }
        }

        private int GetNextId()
        {
            try
            {
                var users = GetUsersList();
                var maxId = users.Max(u => u.Id);
                return maxId + 1;
            }
            catch (Exception) { return 1; }
        }

        internal static void CommitData(List<User> users)
        {
            try
            {
                string path = Path.Combine("Database", "data.json");
                var stream = new StreamWriter(File.Open(path,FileMode.Truncate));
                var json = JsonSerializer.Serialize(users);
                stream.Write(json);
                stream.Close();
            } catch { }
        }

        internal static void UpdateUser(User user)
        {
            var users = User.GetUsersList();
            var currerntUser = users.Find(x => x.Id == user.Id);
            if (currerntUser is not null)
            {
                currerntUser.Update(user);
            }
            else
            {
                users.Add(user);
            }

            CommitData(users);
        }

        private void Update(User user)
        {
            Id = user.Id;
            Title = user.Title;
            Username = user.Username;
            FirstName = user.FirstName;
            LastName = user.LastName;
            PhoneNumber = user.PhoneNumber;
            Email = user.Email;
            Gender = user.Gender;
            Birthday = user.Birthday;
            City = user.City;
            Address = user.Address;
            Password = user.Password;
            Occupation = user.Occupation;
            Token = user.Token;
            TokenTakenTime = user.TokenTakenTime;
            LastLogin = user.LastLogin;
            Bill = user.Bill;
            IsAdmin = user.IsAdmin;
        }

        internal bool IsPasswordMatch(string password)
        {
            return HashPassword(password) == Password;
        }

        internal void Login(string token)
        {
            LastLogin = Helper.DateTimeToChar14(DateTime.Now);
            TokenTakenTime = Helper.DateTimeToChar14(DateTime.Now);
            Token = token;
            UpdateUser(this);
        }

        internal void Logout()
        {
            TokenTakenTime = "";
            Token = "";
            UpdateUser(this);
        }

        internal bool IsLogin()
        {
            var tokenDate = Helper.Char14ToDateTime(TokenTakenTime, DateTime.MinValue);

            return tokenDate.AddHours(2) > DateTime.Now;
        }
    }
}
