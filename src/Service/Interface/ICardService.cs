using HomeBankingMindHub.Model.DTO;
using HomeBankingMindHub.Model.Model.Client;
using System.Security.Claims;

namespace HomeBankingMindHub.Service.Interface;

public interface ICardService
{
    public CardDTO? PostCard(PostCardModel model,ClaimsPrincipal claims, out int statusCode, out string? message);    
}