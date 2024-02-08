using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#pragma warning disable
namespace HomeBankingMindHub.Model.Model.Auth;

public class LoginModel {
    [Required(ErrorMessage = "El email es requerido.")]
    [DataType(DataType.EmailAddress)]
    [RegularExpressionAttribute(@"^((?!\.)[\w\-_.]*[^.])(@\w+)(\.\w+(\.\w+)?[^.\W])$", ErrorMessage = "El email ingresado no es valido.")]
    public string Email {get;set;}
    [Required(ErrorMessage = "la contraseña es requerida.")]
    public string Password {get;set;}
}