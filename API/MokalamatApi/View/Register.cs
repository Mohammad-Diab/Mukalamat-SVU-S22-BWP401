namespace MokalamatApi
{
    public class Register
    {
        public string username { get; set; }
        public string title { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string phoneNumber { get; set; }
        public string email { get; set; }
        public string gender { get; set; }
        public string birthday { get; set; }
        public string city { get; set; }
        public string address { get; set; }
        public string password { get; set; }
        public string occupation { get; set; }

        public Register(string username, string title, string firstName, string lastName, string phoneNumber, string email, string gender, string birthday, string city, string address, string password, string occupation)
        {
            this.username = username;
            this.title = title;
            this.firstName = firstName;
            this.lastName = lastName;
            this.phoneNumber = phoneNumber;
            this.email = email;
            this.gender = gender;
            this.birthday = birthday;
            this.city = city;
            this.address = address;
            this.password = password;
            this.occupation = occupation;
        }

        public Register()
        {
            this.username = "";
            this.title = "";
            this.firstName = "";
            this.lastName = "";
            this.phoneNumber = "";
            this.email = "";
            this.gender = "";
            this.birthday = "";
            this.city = "";
            this.address = "";
            this.password = "";
            this.occupation = "";
        }
    }
}
