using DocumentManagementSystem.Models;

namespace DocumentManagementSystem.Business
{
    public interface IBusinessLayer
    {
        string getSummary(string text);
        DocumentData uploadDocument(DocumentData data);
        DocumentData getDocument(string id);
        DocumentData newDocument(string name, string file, string? tags);
    }
}
