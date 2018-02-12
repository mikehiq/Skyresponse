namespace Skyresponse.Persistence
{
    public interface IPersistenceManager
    {
        void SaveSecure(string key, string value);
        string ReadSecure(string key);
        void Save(string key, string value);
        string Read(string key);
        bool HasValue(string key);
    }
}