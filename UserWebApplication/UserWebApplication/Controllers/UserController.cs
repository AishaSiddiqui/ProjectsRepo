using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UserWebApplication.Models;
using MongoDB.Driver;
using System.Net.Mail;
using System.Text;

namespace UserWebApplication.Controllers
{
    public class UserController : ApiController
    {
        //Get All Users
        [HttpGet]
        public IHttpActionResult GetAllUsers()
        {
            if (SessionInfo.IsLoggedIn)
            {
                var myuserContext = new UserContext();
                MongoClient MongoClient = new MongoClient();
                var _database = MongoClient.GetDatabase("UserManagement");
                var collection = _database.GetCollection<UserModel>("Users");
                var results = collection.Find(x => x.FirstName == "Aisha").ToList();
                var filter = new BsonDocument();
                var result = collection.Find(filter).ToListAsync();
                result.Wait();
                var myUsers = myuserContext.Users.Find(filter).ToList().OrderBy(x => x.FirstName);
                if (myUsers != null && myUsers.Count() > 0)
                {
                    var AllUsers = myUsers.Select(x => new UserModel()
                    {
                        _id = x._id,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        User_Name = x.User_Name,
                        User_Password = x.User_Password,
                        User_Email = x.User_Email
                    });
                    return Ok( AllUsers.ToList());
                }
                else
                    return NotFound();
            }
            else
            {
                return InternalServerError(new Exception("User Session Unavailable"));
                
            }
        }
        //Get a single user based on user id
        [HttpGet]
        public IHttpActionResult GetUser(string Id)
        {
            if (SessionInfo.IsLoggedIn)
            {
                var myuserContext = new UserContext();
                var myUsers = myuserContext.Users.Find(x => x._id == Id).FirstOrDefault();
                if (myUsers != null)
                {
                    UserModel user = new UserModel();
                    user._id = myUsers._id;
                    user.FirstName = myUsers.FirstName;
                    user.LastName = myUsers.LastName;
                    user.User_Name = myUsers.User_Name;
                    user.User_Password = myUsers.User_Password;
                    user.User_Email = myUsers.User_Email;
                    return Ok(user);
                }
                else
                    return NotFound();
            }
            else
                
                return InternalServerError(new Exception("User Session Unavailable"));
        }

        //Add new User
        [HttpPost]
        public HttpResponseMessage AddUser(UserModel userModel)
        {
            try
            {
                if (SessionInfo.IsLoggedIn)
                {
                    if (ModelState.IsValid)
                    {
                        UserModel user = new UserModel();
                        user.FirstName = userModel.FirstName;
                        user.LastName = userModel.LastName;
                        user.User_Name = userModel.User_Name;
                        user.User_Password = userModel.User_Password;
                        user.User_Email = userModel.User_Email;
                        var myuserContext = new UserContext();
                        myuserContext.Users.InsertOne(user);

                        return Request.CreateResponse(HttpStatusCode.Created, "Submitted Successfully");
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Model State not Valid");
                    }
                }
                else
                {
                    throw new Exception("User Session Unavailable");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something wrong !", ex);
            }
        }

        //Update the user
        [HttpPut]
        public HttpResponseMessage UpdateUser(UserModel userModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UserModel user = new UserModel();
                    user._id = userModel._id;
                    user.FirstName = userModel.FirstName;
                    user.LastName = userModel.LastName;
                    user.User_Name = userModel.User_Name;
                    user.User_Password = userModel.User_Password;
                    user.User_Email = userModel.User_Email;
                    var myuserContext = new UserContext();
                    myuserContext.Users.ReplaceOne(new BsonDocument("_id", new ObjectId(userModel._id)), user);
                    return Request.CreateResponse(HttpStatusCode.OK, "Updated Successfully");
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Model State not Valid");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Something wrong !", ex);
            }
        }

        //Delete the user
        [HttpDelete]
        public HttpResponseMessage DeleteUser(string id)
        {
            UserModel user = new UserModel();
            var myuserContext = new UserContext();
            myuserContext.Users.FindOneAndDelete(x => x._id == id);
            //var filter = new BsonDocument("_id", new ObjectId(id));
            //myuserContext.Users.DeleteOne(filter);
            return Request.CreateResponse(HttpStatusCode.OK, user);
        }

        //Login User
        [HttpPost]
        public HttpResponseMessage Login(UserModel userModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UserModel user = new UserModel();
                    user.User_Name = userModel.User_Name;
                    user.User_Password = userModel.User_Password;
                    var myuserContext = new UserContext();
                    var results = myuserContext.Users.Find(x => x.User_Name == userModel.User_Name && x.User_Password == userModel.User_Password).FirstOrDefault();
                    if (results != null)
                    {
                        SessionInfo.CurrentUserSession = results;
                        return Request.CreateResponse(HttpStatusCode.OK, "Logged In Successfully");
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.Unauthorized, "user does not exists");
                    }
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Model State not Valid");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something wrong !", ex);
            }
        }
        [HttpPost]
        public HttpResponseMessage SendEmail(UserModel userModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var myuserContext = new UserContext();
                    var results = myuserContext.Users.Find(x => x.User_Email == userModel.User_Email).FirstOrDefault();
                    if (results != null)
                    {
                        SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);
                        MailMessage mail = new MailMessage();
                        StringBuilder sb = new StringBuilder();
                        sb.Append("Hi" + results.FirstName);
                        sb.Append("This email is sent in response to your request for password change, Please user the temporary password provided");
                        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                        var stringChars = new char[8];
                        var random = new Random();
                        for (int i = 0; i < stringChars.Length; i++)
                        {
                            stringChars[i] = chars[random.Next(chars.Length)];
                        }
                        var finalString = new String(stringChars);
                        sb.Append("User Name" + results.User_Name);
                        sb.Append("Temporary Password" + finalString);
                        sb.AppendLine();
                        sb.Append("Regards,");
                        sb.Append("XYZ Support Team");
                        mail.From = new MailAddress("aishasidz@gmail.com");
                        mail.To.Add(results.User_Email);
                        mail.Subject = "Password Recovery";
                        mail.Body = sb.ToString();
                        SmtpServer.Credentials = new System.Net.NetworkCredential("aishasidz@gmail.com", "uuaaj11331");
                        SmtpServer.EnableSsl = true;
                        results.User_Password = finalString;
                        changePassword(results);
                        SmtpServer.Send(mail);
                        return Request.CreateResponse(HttpStatusCode.OK, "Email send Successfully");
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.Unauthorized, "user does not exists");
                    }
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Model State not Valid");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something wrong !", ex);
            }
        }
        public void changePassword(UserModel userModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UserModel user = new UserModel();
                    user._id = userModel._id;
                    user.FirstName = userModel.FirstName;
                    user.LastName = userModel.LastName;
                    user.User_Name = userModel.User_Name;
                    user.User_Password = userModel.User_Password;
                    user.User_Email = userModel.User_Email;
                    var myuserContext = new UserContext();
                    myuserContext.Users.ReplaceOne(new BsonDocument("_id", new ObjectId(userModel._id)), user);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong while changing Password");
            }
        }
    }
}
