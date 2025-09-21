using DocumentManagementSystem.Models;

namespace DocumentManagementSystem.DataAccess
{
    public interface IDocumentRepository
    {
        IEnumerable<DocumentData> GetAll();
        DocumentData GetById(Guid id);
        void Insert(DocumentData data);
        void Update(DocumentData data);
        void Delete(Guid id);
    }
}
