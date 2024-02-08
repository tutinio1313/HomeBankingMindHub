#pragma warning disable 
using System.ComponentModel.DataAnnotations;

namespace HomeBankingMindHub.Model.Model.Client;
public class PostModel
{
    [Required(ErrorMessage = "El nombre es requerido.")]
    [DataType(DataType.Text)]
    public string FirstName { get; set; }
    [Required(ErrorMessage = "El apellido es requerido.")]
    [DataType(DataType.Text)]
    public string LastName { get; set; }
    [Required(ErrorMessage = "El email es requerido.")]
    [RegularExpressionAttribute(@"^((?!\.)[\w\-_.]*[^.])(@\w+)(\.\w+(\.\w+)?[^.\W])$", ErrorMessage = "El email ingresado no es valido.")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    [Required(ErrorMessage = "La contraseña es requerida.")]
    public string Password { get; set; }
}