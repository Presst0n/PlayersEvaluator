using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PE.DataManager.Dto
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string AuthUserId { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastLoginDate { get; set; }
        public string RefreshToken { get; set; }
        public string Token { get; set; }
    }
}
