
namespace DocumentManagementSystem.Messaging
{
    public interface INewDocumentPublisher
    {
        Task PublishNewDocumentAsync(string documentString);
    }
}
