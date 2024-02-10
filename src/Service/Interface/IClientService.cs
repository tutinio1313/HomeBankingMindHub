using Microsoft.AspNetCore.Mvc;

using HomeBankingMindHub.Model.DTO;
using HomeBankingMindHub.Model.Model.Client;
using System.Security.Claims;

namespace HomeBankingMindHub.Service.Interface;

public interface IClientService {
    public ClientDTO? CreateUser(PostModel model, out int StatusCode, out string? message);
    public IEnumerable<ClientDTO>? GetAll(out int StatusCode, out string? message);
    public ClientDTO? GetByID(string id, out int StatusCode, out string? message);
    public ClientDTO? GetCurrent(ClaimsPrincipal claims, out int StatusCode, out string? message);

}