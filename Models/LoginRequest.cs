using System.ComponentModel.DataAnnotations;

namespace GravyFoodsApi.Models
{
   

        public class LoginRequest
        {
            [MaxLength(50)]
            public string Username { get; set; }
            public string Password { get; set; }
        }
    
}
