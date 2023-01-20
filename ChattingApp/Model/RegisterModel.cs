using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChattingApp.Model
{
    public class RegisterModel : LoginModel
    {
        [Required(ErrorMessage = "Please provide user name.")]
        public string UserName { get; set; }
    }
}
