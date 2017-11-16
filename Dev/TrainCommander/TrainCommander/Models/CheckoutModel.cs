using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrainCommander.Models
{
    public class ListCarViewModel
    {
        public List<SuperTripViewModel> superTripViewModel { get; set; }
    }

    public class PersonneViewModel
    {
        [Required]
        [Display(Name = "firstName", ResourceType = typeof(Resources.langage))]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "lastName", ResourceType = typeof(Resources.langage))]
        public string LastName { get; set; }
    }

    public class CoordonneesViewModel
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
    }

    public class ConditionViewModel
    {
        [Required]
        [Display(Name = "generalCondition", ResourceType = typeof(Resources.langage))]
        [MustBeTrue(ErrorMessage = "acceptCondition", ResourceType = typeof(Resources.langage))]
        public bool ConditionAccepted { get; set; }
    }

    public class ConfirmationViewModel
    {
        public List<SuperTripViewModel> superTripViewModel { get; set; }
        public ConditionViewModel conditionViewModel { get; set; }
        public CoordonneesViewModel coordonneesViewModel { get; set; }
        public List<PersonneViewModel> personneViewModel { get; set; }

        public double TotalPrice { get; set; }
    }

    public class MustBeTrueAttribute : ValidationAttribute
    {
        public Type ResourceType { get; set; }

        public override bool IsValid(object value)
        {
            return value is bool && (bool)value;
        }
    }
}