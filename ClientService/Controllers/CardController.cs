using HomeBankingMindHub.Model.DTO;
using HomeBankingMindHub.Model.Model.Client;
using HomeBankingMindHub.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace ClientService.Controllers;

    [ApiController]
    [Route("api/[controller]")]
    public class CardController(ICardService cardService) : ControllerBase
    {
        [HttpPost("current/cards")]
        public ActionResult PostCard(PostCardModel model)
        {
            string? UserEmail = User.FindFirstValue("Client");

            if (!UserEmail.IsNullOrEmpty())
            {
                CardDTO? card = cardService.PostCard(model: model, Email: UserEmail, out int statusCode, out string? message);

                if (card is not null)
                {
                    return StatusCode(statusCode, card);
                }
                return StatusCode(statusCode, message);
            }
            return StatusCode(401, "Usted no tiene los permisos necesarios.");
        }

        [HttpGet("current/cards")]
        public ActionResult GetCards()
        {

            string? UserEmail = User.FindFirstValue("Client");

            if (!UserEmail.IsNullOrEmpty())
            {
                CardDTO[]? cardDTOs = cardService.GetDTOCards(UserEmail, out int statusCode, out string? message);
                if (cardDTOs is not null)
                {
                    return StatusCode(statusCode, cardDTOs);
                }
                return StatusCode(statusCode, message);
            }

            return StatusCode(401, "Usted no tiene los permisos necesarios.");
        }
    }
