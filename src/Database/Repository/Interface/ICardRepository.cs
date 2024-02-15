using HomeBankingMindHub.Model.Entity;

namespace HomeBankingMindHub.Database.Repository;
public interface ICardRepository
{
    bool ExistsCardByNumber(string number);
    bool CanPostNewCard(string clientID, CardType? Type, CardColor? Color);
    void Save(Card card);
}
