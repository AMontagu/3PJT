using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrainCommander.Models
{
    public class MailModels
    {
        [Required]
        [Display(Name = "firstName", ResourceType = typeof(Resources.langage))]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "lastName", ResourceType = typeof(Resources.langage))]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "eMail", ResourceType = typeof(Resources.langage))]
        public string Email { get; set; }
        [Display(Name = "telephone", ResourceType = typeof(Resources.langage))]
        public string Telephone { get; set; }
        [Required]
        [Display(Name = "message", ResourceType = typeof(Resources.langage))]
        public string Message { get; set; }
    }
}