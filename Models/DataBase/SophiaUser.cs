using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sophia.Models.Rogatio;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sophia.Models.DataBase
{
    public class SophiaUser : IdentityUser
    {
        [Key]
        public override string Id { get; set; }
        //[InverseProperty("SophiaUser")]
        public List<Answer> Answers { get; set; }
        public List<Merch> Merch { get; set; }
    }
}
