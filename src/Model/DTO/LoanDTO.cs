namespace HomeBankingMindHub.Model.DTO;

public class LoanDTO
{
    public required string ID { get; set; }
    public required string Name { get; set; }
    public required double MaxAmount { get; set; }
    public required string Payments { get; set; }

}