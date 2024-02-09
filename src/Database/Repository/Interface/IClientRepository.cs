using HomeBankingMindHub.Model.Entity;

namespace HomeBankingMindHub.Database.Repository;

public interface IClientRepository {
    IEnumerable<Client> GetAllUsers();
    Client? FindByID(string ID);
    Client? FindByEmail(string email);
    int Save(Client client);
    void Put(Client client);
}