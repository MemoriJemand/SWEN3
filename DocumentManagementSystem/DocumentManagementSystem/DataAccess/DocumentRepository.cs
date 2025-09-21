using DocumentManagementSystem.Models;

namespace DocumentManagementSystem.DataAccess
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly DocumentContext _context;

        public DocumentRepository()
        {
            _context = new DocumentContext();
        }
        public DocumentRepository(DocumentContext context)
        {
            _context = context;
        }
        public IEnumerable<DocumentData> GetAll()
        {
            return _context.Documents.ToList();
        }
        public DocumentData GetById(Guid id)
        {
            return _context.Documents.Find(id);
        }
        public void Delete(Guid id)
        {
            var document = GetById(id);
            if (document != null)
            {
                _context.Documents.Remove(document);
            }
            _context.SaveChanges();
        }
        public void Insert(DocumentData document)
        {
            _context.Documents.Add(document);
            _context.SaveChanges();
        }
        public void Update(DocumentData document)
        {
            _context.Documents.Update(document);
            _context.SaveChanges();
        }
    }
}
