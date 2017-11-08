using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication.ViewModels
{
    public class UserInfo
    {
        public string EmailAddress { get; set; }
        public string Name { get; set; }
        public string ProfileImage { get; set; }
        
        private string idToken { get; set; }
        private string refreshToken { get; set; }
        private string accessToken { get; set; }

        public UserInfo() { }

        public UserInfo(string EmailAddress, string Name, string ProfileImage)
        {
            this.EmailAddress = EmailAddress;
            this.Name = Name;
            this.ProfileImage = ProfileImage;
        }

        public UserInfo(string EmailAddress, string Name, string ProfileImage, string idToken, string accessToken, string refreshToken)
        {
            this.EmailAddress = EmailAddress;
            this.Name = Name;
            this.ProfileImage = ProfileImage;
            this.idToken = idToken;
            this.accessToken = accessToken;
            this.refreshToken = refreshToken;
        }

        public string getIdToken()
        {
            return this.idToken;
        }
        public string getAccessToken()
        {
            return this.accessToken;
        }
        public string getRefreshToken()
        {
            return this.refreshToken;
        }
    }
}