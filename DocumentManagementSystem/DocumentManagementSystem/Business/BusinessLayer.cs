using DocumentManagementSystem.DataAccess;
using DocumentManagementSystem.Models;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DocumentManagementSystem.Business
{
    public class BusinessLayer
    {
        IDocumentRepository _repository;

        private string getSummary(string text) 
        {
            //send to ai
            return "";
        }
        private bool uploadDocument(DocumentData data) 
        {
            //save to repository
            return false;
        }
        private string uploadOriginal(string data)
        {
            //save to minio
            return "";
        }
        public DocumentData getDocument(string id) 
        {
            //get data from repository and minio
        }
        public bool newDocument(string name, string file, string? tags) 
        {
            //transform input into repository data
            DocumentData Document = new DocumentData();
            Document.Title = name;
            Document.Tags = tags;
            Document.Original = uploadOriginal(file);
            Document.Text = getText(file);
            Document.Summary = getSummary(Document.Text);
            Document.DateUploaded = DateTime.Now;
            bool res = uploadDocument(Document);
            return res;
        }
        public string getText(string data)
        {
            //get ocr text
            return "";
        }

        public IEnumerable<DocumentData> getAllDocuments()
        {
            return _repository.GetAll();
        }

    }
}
