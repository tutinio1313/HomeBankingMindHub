using HomeBankingMindHub.Model.DTO;
using HomeBankingMindHub.Model.Model.Client;

namespace HomeBankingMindHub.Service.Interface;

public interface ICardService
{
    public CardDTO? PostCard(PostCardModel model,string Email, out int statusCode, out string? message);    
    public CardDTO[]? GetDTOCards(string Email,out int statusCode, out string? message);
}