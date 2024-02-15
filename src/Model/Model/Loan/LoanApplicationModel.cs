using System.ComponentModel.DataAnnotations;

namespace HomeBankingMindHub.Model.Model.Loan;

public class LoanApplicationModel
{
    [Required(ErrorMessage = "El ID del prestamo es requerido.")]
    /*[Length(minimumLength: 36
            , maximumLength: 36
            , ErrorMessage = "El formato del ID no es valido."
    )]*/
    public required string LoanID { get; set; }
    [Required(ErrorMessage = "El monto del prestamo es requerido.")]
    [Range(
        minimum: 0.001
        , maximum: double.MaxValue
        , ErrorMessage = "El numero esta fuera de rango.")
    ]
    public required double Amount { get; set; }

    [Required(ErrorMessage = "El número de cuenta es requerido.")]
    [DataType(DataType.Text)]
    [Length(
    minimumLength: 12
    , maximumLength: 12
    , ErrorMessage = "La cuenta emisor no es correcto.")
    ]
    [RegularExpression(@"[V|v][I|i][n|N]-\d\d\d\d\d\d\d\d"
    , ErrorMessage = "El número de cuenta emisor no tiene el formato correcto.")]
    public required string NumberAccount { get; set; }
    [Required(ErrorMessage = "El plazo de pagos es requerido.")]
    public required string Payment { get; set; }
}