namespace DocumentManagementSystem.Messaging
{
    public interface INewDocumentReceiver
    {
        Task<String> ReceiveDocumentText();
    }
}
