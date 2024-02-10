using System.Security.Claims;
using HomeBankingMindHub.Database.Repository;
using HomeBankingMindHub.Model.DTO;
using HomeBankingMindHub.Model.Entity;
using HomeBankingMindHub.Model.Model.Client;
using HomeBankingMindHub.Service.Interface;
using HomeBankingMindHub.Utils;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace HomeBankingMindHub.Service.Instance;

public class CardService(ICardRepository _cardRepository, IClientRepository _clientRepository) : ICardService
{
    public CardDTO? PostCard(PostCardModel model, ClaimsPrincipal claims, out int statusCode, out string? message)
    {
#pragma warning disable
        string Id = claims.FindFirstValue("Client");
#pragma warning restore
        if (Id is not null)
        {
            try
            {
                Client? client = _clientRepository.FindByEmail(Id);
                Random random = new();
                if (client is not null)
                {
                    if (Utils.Utils.ValidateCardModel(model, out CardColor? CardColor, out CardType? CardType))
                    {
                        if (_cardRepository.CanPostNewCard(client.Id, CardType))
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
#pragma warning disable
                            client.Cards.Add(card);
#pragma warning restore
                            _clientRepository.Put(client);


                            statusCode = 201;
                            message = null;
                            return new CardDTO
                            {
                                Id = card.Id,
                                CardHolder = card.CardHolder,
                                CVV = card.CVV,

                                Type = card.Type.ToString(),
                                Color = card.Color.ToString(),
                                Number = card.Number,

                                FromDate = card.FromDate,
                                ThruDate = card.ThruDate
                            };
                        }
                        else
                        {
                            statusCode = 403;
                            message = $"Usted ha alcanzado el maximo de 3 tarjetas de tipo {model.Type.ToLower() + "s"}.";
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
        }

        else
        {
            statusCode = 401;
            message = "No tienes permisos para hacer esta acción.";
        }

        return null;
    }
}