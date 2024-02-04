using HomeBankingMindHub.Model.Entity;

namespace HomeBankingMindHub.Database.Repository;

public interface IClientRepository {
    IEnumerable<Client> GetAllUsers();
    Client FindByID(string ID);
    void Save(Client client);
}