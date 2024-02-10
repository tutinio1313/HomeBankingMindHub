using System.Security.Cryptography;
using System.Text;
using HomeBankingMindHub.Database.Repository;
using HomeBankingMindHub.Model.Entity;
using HomeBankingMindHub.Model.Model.Client;

namespace HomeBankingMindHub.Utils;
public static class Utils
{
    public static string GenerateAccountNumber(Random random, ref IAccountRepository _accountRepository)
    {
        string accountNumber;
        do
        {
            accountNumber = "VIN-" + random.Next(1, 100000000).ToString();
        } while (_accountRepository.ExistsAccountByNumber(accountNumber));
        return accountNumber;
    }

    public static bool ValidateCardModel(PostCardModel model, out CardColor? cardColor, out CardType? cardType)
    {

        bool boolColor = ValidateCardColor(model.Color, out cardColor);
        bool boolType = ValidateCardType(model.Type, out cardType);

        return boolColor && boolType;
    }

    public static string GenerateCardNumber(Random random, ICardRepository _cardRepository)
    {
        string cardNumber = string.Empty;
        do
        {
            cardNumber = random.Next(1000, 10000).ToString() + "-" + random.Next(1000, 10000).ToString() + "-" + random.Next(1000, 10000).ToString() + "-" + random.Next(1000, 10000).ToString();
        } while (_cardRepository.ExistsCardByNumber(cardNumber));
        return cardNumber;
    }
           
    public static string HashPassword(string password) => Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(password)));

    public static bool AreEqual(string password, string passwordHash) => HashPassword(password).Equals(passwordHash);
    private static bool ValidateCardColor(string color, out CardColor? cardColor)
    {
        switch (color.ToUpper())
        {
            case nameof(CardColor.GOLD):
                cardColor = CardColor.GOLD;
                return true;

            case nameof(CardColor.TITANIUM):
                cardColor = CardColor.TITANIUM;
                return true;

            case nameof(CardColor.SILVER):
                cardColor = CardColor.SILVER;
                return true;
            default:
                cardColor = null;
                return false;
        }
    }
    private static bool ValidateCardType(string type, out CardType? cardType)
    {
        if (type == CardType.CREDIT.ToString())
        {
            cardType = CardType.CREDIT;
            return true;
        }
        if (type == CardType.DEBIT.ToString())
        {
            cardType = CardType.DEBIT;
            return true;
        }

        cardType = null;
        return false;

    }
}