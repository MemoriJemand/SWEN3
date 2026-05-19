using DocumentManagementSystem.Models;

namespace DocumentManagementSystem.Business
{
    public interface IBusinessLayer
    {
        DocumentData getDocument(string id);
        DocumentData newDocument(string name, string file, string? tags);
        IEnumerable<DocumentData> getAllDocuments();
        string getText(string data);
    }
}
