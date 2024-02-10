using HomeBankingMindHub.Database.Repository;
using HomeBankingMindHub.Model.Entity;

namespace HomeBankingMindHub.Database.Repository;

    public class CardRepository(HomeBankingContext context) : Repository<Card>(context), ICardRepository
    {
        public bool ExistsCardByNumber(string number) => FindByCondition(x => x.Number == number).Any();
        public bool CanPostNewCard(string clientID, CardType? Type) => FindByCondition(x => x.ClientID == clientID)
                                                                    .Where(x => x.Type == (CardType)Type)
                                                                    .Count() < 3;
        
    }

