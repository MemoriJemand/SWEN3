using DocumentManagementSystem.Models;

namespace DocumentManagementSystem.Business
{
    public interface IBusinessLayer
    {
        DocumentData getDocumentById(string id);
        bool newDocument(string name, string file, string? tags);
        IEnumerable<DocumentData> getAllDocuments();
        Task<string> getText(string data);
    }
}
