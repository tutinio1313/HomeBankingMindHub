#pragma warning disable 
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.DataAnnotations;

namespace HomeBankingMindHub.Model.Model.Transaction;
public class PostTransactionModel
{
    [Required(ErrorMessage = "El número de cuenta emisor es requerido.")]
    [DataType(DataType.Text)]
    [Length(
    minimumLength: 12, 
    maximumLength: 12
    ,ErrorMessage = "La cuenta emisor no es correcto.")
    ]
    public string FromAccountNumber { get; set; }
    
    [Required(ErrorMessage = "El número de cuenta receptor es requerido.")]
    [DataType(DataType.Text)]
    [Length(
    minimumLength: 12, 
    maximumLength: 12
    ,ErrorMessage = "La cuenta emisor no es correcto.")
    ]
    public string ToAccountNumber { get; set; }
    
    [Required(ErrorMessage = "El importe es requerido.")]
    [DataType(DataType.Currency)]
    [Range(
        minimum: 0
        ,maximum: double.MaxValue
        ,ErrorMessage = "El numero esta fuera de rango.")
    ]
    public double Amount { get; set; }

    [Required(ErrorMessage = "La description es requerida.")]
    [MinLength(
        length: 1
        ,ErrorMessage = "La descripción es requerida")
    ]
    public string Description { get; set; }
}