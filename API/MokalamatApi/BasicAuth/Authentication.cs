using System.Security.Cryptography;
using System.Text;

namespace MokalamatApi
{
    public static class Authentication
    {
        public static string Login(string username, string password)
        {
            var users = User.GetUsersList();
            var user = users.Find(x => x.Username == username);
            if (user == null)
            {
                throw new Exception("(-1) Bad Username or Password");
            }
            else
            {
                if (user.IsPasswordMatch(password))
                {
                    string token = encrypt(username);
                    user.Login(token);
                    return token;
                }
                else
                {
                    throw new Exception("(-1) Bad Username or Password");
                }
            }
        }

        public static string GetUserFullName(string token)
        {

            var users = User.GetUsersList();
            var Username = GetUsername(token);
            var user = users.Find(x => x.Username == Username);
            if (user != null)
            {
                return user.FirstName + " " + user.LastName;
            }

            return "";
        }

        public static bool IsLogin(string token)
        {
            var Username = Decrypt(token);
            var users = User.GetUsersList();
            return users.Find(x => x.Username == Username)?.IsLogin() ?? false;
        }

        public static bool IsAdmin(string token)
        {

            var Username = GetUsername(token);
            var users = User.GetUsersList();
            return users.Find(x => x.Username == Username)?.IsAdmin ?? false;

        }

        public static void Logout(string token)
        {
            var Username = GetUsername(token);
            var users = User.GetUsersList();
            users.Find(x => x.Username == Username)?.Logout();
        }

        public static string GetUsername(string token)
        {
            if (IsLogin(token))
            {
                var Username = Decrypt(token);
                return Username;
            }
            return "";
        }

        private static string EncryptionKey = "0d895a26-aa56-4ee4-a3c7-77801c5b2e48";

        private static string encrypt(string encryptString)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(encryptString);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    encryptString = Convert.ToBase64String(ms.ToArray());
                }
            }
            return encryptString;
        }

        private static string Decrypt(string cipherText)
        {
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

    }
}
