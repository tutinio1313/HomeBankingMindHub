using HomeBankingMindHub.Database.Repository;
using HomeBankingMindHub.Model.Entity;

namespace HomeBankingMindHub.Database.Repository;

    public class CardRepository(HomeBankingContext context) : Repository<Card>(context), ICardRepository
    {
    public bool ExistsCardByNumber(string number) => FindByCondition(x => x.Number == number).Any();
    public bool CanPostNewCard(string clientID, CardType? Type, CardColor? Color) => !FindByCondition(x => x.ClientID == clientID)
                                                                    .Where(x => x.Type == (CardType)Type)
                                                                    .Where(x => x.Color == (CardColor)Color)
                                                                    .Any();
    public bool IsIDAvailable(string id) => !FindByCondition(card => card.Id == id).Any();

    public void Save(Card card)
    {
        if(!IsIDAvailable(card.Id))
        {
            do
            {
                card.Id = Guid.NewGuid().ToString();
            } while (!IsIDAvailable(card.Id));
        }
        Create(card);
        SaveChanges();
    }
}

