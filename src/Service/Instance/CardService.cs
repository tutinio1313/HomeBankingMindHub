using System.Security.Claims;
using HomeBankingMindHub.Database.Repository;
using HomeBankingMindHub.Model.DTO;
using HomeBankingMindHub.Model.Entity;
using HomeBankingMindHub.Model.Model.Client;
using HomeBankingMindHub.Service.Interface;
using Microsoft.IdentityModel.Tokens;

namespace HomeBankingMindHub.Service.Instance;

public class CardService(ICardRepository _cardRepository, IClientRepository _clientRepository) : ICardService
{
    public CardDTO[]? GetDTOCards(string Email, out int statusCode, out string? message)
    {
        string? ClientID = _clientRepository.FindByEmail(Email).Id;

        if(!ClientID.IsNullOrEmpty())
        {
            IEnumerable<Card> cards = _cardRepository.FindCardsByClientID(ClientID);
            if(cards.Any())
            {
                CardDTO[] DTOCards = new CardDTO[cards.Count()];
                int index = 0;

                foreach(Card card in cards)
                {
                    DTOCards[index] = LoadCardDTO(card);
                    index++;
                }
                
                if(DTOCards.Length != 0)
                {
                    statusCode = 200;
                    message = null;
                    return DTOCards;
                }
            }
            message = "No hay tarjetas cardas asociadas a ese cliente.";
            statusCode = 200;
        }
        else
        {
            statusCode = 403;
            message = "El usuario no se ha podido encontrar";
        }
        return null;
    }

    public CardDTO? PostCard(PostCardModel model, string Email, out int statusCode, out string? message)
    {
            try
            {
                Client? client = _clientRepository.FindByEmail(Email);
                Random random = new();
                if (client is not null)
                {
                    if (Utils.Utils.ValidateCardModel(model, out CardColor? CardColor, out CardType? CardType))
                    {
                        if (_cardRepository.CanPostNewCard(client.Id, CardType, CardColor))
                        {
                            Card card = new Card
                            {
                                Id = Guid.NewGuid().ToString(),
                                CardHolder = string.Concat(string.Concat(client.FirstName, " "), client.LastName),
                                client = client,
                                ClientID = client.Id,
#pragma warning disable
                                Color = (CardColor)CardColor,
                                Type = (CardType)CardType,
#pragma warning restore
                                CVV = random.Next(1, 1000),
                                Number = Utils.Utils.GenerateCardNumber(random, _cardRepository),

                                FromDate = DateTime.Now.AddMonths(-1),
                                ThruDate = DateTime.Now.AddYears(2)

                            };
                            _cardRepository.Save(card);


                            statusCode = 201;
                            message = null;
                            return LoadCardDTO(card);
                        }
                        else
                        {
                            statusCode = 403;
                            message = $"Usted ya posee una tarjeta del tipo {model.Type + " " + model.Color}.";
                        }
                    }
                    else
                    {
                        statusCode = 415;
                        message = "Los datos ingresados son erroneos.";
                    }
                }
                else
                {
                    statusCode = 403;
                    message = "No se ha podido identificar tu usuario.";
                }
            }
            catch (Exception ex)
            {
                statusCode = 500;
                message = ex.ToString();
            }
        return null;
    }

    private static CardDTO LoadCardDTO(Card card) => new() {Id = card.Id,
                                CardHolder = card.CardHolder,
                                CVV = card.CVV,

                                Type = card.Type.ToString(),
                                Color = card.Color.ToString(),
                                Number = card.Number,

                                FromDate = card.FromDate,
                                ThruDate = card.ThruDate};
}