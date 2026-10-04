using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace MokalamatApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class Mokalamat : ControllerBase
    {
        [HttpGet]
        public string IsLogin()
        {
            string token = getTokenValue(); ;
            return Authentication.GetUserFullName(token);
        }

        [HttpGet]
        public bool IsAdmin()
        {
            string token = getTokenValue();
            return Authentication.IsAdmin(token);
        }

        [HttpGet]
        public string Login(string username, string password)
        {
            try
            {
                return Authentication.Login(username, password);

            }
            catch (Exception) { return ""; }
        }

        [HttpPost]
        public string CreateUser([FromBody] Register user)
        {
            try
            {
                bool result = Logic.CreateUser(user.username, user.title, user.firstName, user.lastName, user.phoneNumber, user.email, user.gender, user.birthday, user.city, user.address, user.password, user.occupation);
                if (result)
                {
                    return "ok";
                }
                else
                {
                    return "failed";
                }
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
        }

        [HttpGet]
        public void Logout()
        {
            string token = getTokenValue();
            Authentication.Logout(token);
        }

        [HttpGet]
        public string GetMyBill(string phoneNumber)
        {
            try
            {
                string token = getTokenValue();
                string username = Authentication.GetUsername(token);
                double bill = Logic.GetMyBill(username, phoneNumber);

                return "Your bill is: " + bill + " SP";

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        [HttpGet]
        public List<UserTable>? GetAllUsers()
        {
            try
            {
                return Logic.GetAllUsers();
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpGet]
        public string SetUserBill(string phoneNumber, double bill)
        {
            try
            {
                bool result = Logic.SetUserBill(phoneNumber, bill);
                if (result)
                {
                    return "ok";
                }
                else
                {
                    return "failed";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private String getTokenValue()
        {
            return Request.Headers.Where(x => x.Key.ToLower() == "mytoken").FirstOrDefault().Value.ToString() ?? ""; ;
        }

    }
}
