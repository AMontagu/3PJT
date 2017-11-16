using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TrainCommander.Models
{

    public class ExternalInternalLogin
    {
        public ExternalLoginConfirmationViewModel External { get; set; }
        public LoginViewModel Internal { get; set; }
    }

    public class ExternalLoginConfirmationViewModel
    {
        /*[Display(Name = "FirstName", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources),
              ErrorMessageResourceName = "FirstNameRequired")]
        [StringLength(50, ErrorMessageResourceType = typeof(Resources.Resources),
                      ErrorMessageResourceName = "FirstNameLong")]*/

        [Required]
        [Display(Name = "eMail", ResourceType = typeof(Resources.langage))]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "code", ResourceType = typeof(Resources.langage))]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "rememberBrowser", ResourceType = typeof(Resources.langage))]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "eMail", ResourceType = typeof(Resources.langage))]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "eMail", ResourceType = typeof(Resources.langage))]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "password", ResourceType = typeof(Resources.langage))]
        public string Password { get; set; }

        [Display(Name = "rememberPassword", ResourceType = typeof(Resources.langage))]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "eMail", ResourceType = typeof(Resources.langage))]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "La chaîne {0} doit comporter au moins {2} caractères.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "password", ResourceType = typeof(Resources.langage))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "passwordConfirmation", ResourceType = typeof(Resources.langage))]
        [Compare("Password", ErrorMessageResourceType = typeof(Resources.langage), ErrorMessageResourceName = "comparePasswordInvalid")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "eMail", ResourceType = typeof(Resources.langage))]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "La chaîne {0} doit comporter au moins {2} caractères.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "password", ResourceType = typeof(Resources.langage))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "passwordConfirmation", ResourceType = typeof(Resources.langage))]
        [Compare("Password", ErrorMessageResourceType = typeof(Resources.langage), ErrorMessageResourceName ="comparePasswordInvalid")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "eMail", ResourceType = typeof(Resources.langage))]
        public string Email { get; set; }
    }
}
