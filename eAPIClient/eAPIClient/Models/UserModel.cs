using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eAPIClient.Models
{
   
    public class UserModel 
    {        
        public int id { get; set; }
        public string username { get; set; }
        public string full_name { get; set; }
        public string password { get; set; } = "";  
    }
  
}