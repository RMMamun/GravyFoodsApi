using System.ComponentModel.DataAnnotations;

namespace GravyFoodsApi.Models
{
   

        public class LoginRequest
        {
            [MaxLength(50)]
            public string Username { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            public string CompanyCode { get; set; } = string.Empty;

        }
    
}
