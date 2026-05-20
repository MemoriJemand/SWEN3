using DocumentManagementSystem.DataAccess;
using DocumentManagementSystem.Messaging;
using DocumentManagementSystem.Models;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Minio;
using System.Diagnostics.Eventing.Reader;

namespace DocumentManagementSystem.Business
{
    public class BusinessLayer : IBusinessLayer
    {
        IDocumentRepository _repository;
        INewDocumentPublisher _publisher;
        IMinioClient _minio;
        private Summarizer _summarizer = new();

        private string getSummary(string text) 
        {
            //send to ai
            var result = _summarizer.returnSummary(text);
            return result;
        }
        private bool uploadDocument(DocumentData data) 
        {
            //save to repository
            _repository.Insert(data);
            var check = _repository.GetById(data.Id);
            return (check != null);
        }
        private string uploadOriginal(byte[] data)
        {
            //save to minio
            var path = Path.Combine("/tempFile", Path.GetRandomFileName());
            File.WriteAllBytes(path, data);
            var result = FileUpload.Run(_minio, path).Result;
            if (result != null)
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                return result;
            }
            return "";
        }
        public DocumentData getDocumentById(string id) 
        {
            //get data from repository
            var result = _repository.GetById(Guid.Parse(id));
            return result;
        }
        public bool newDocument(string name, byte[] file, string? tags) 
        {
            //transform input into repository data
            DocumentData Document = new DocumentData();
            Document.Title = name;
            Document.Tags = tags;
            Document.Original = uploadOriginal(file);
            Document.Text = getText(file.ToString()).Result;
            Document.Summary = getSummary(Document.Text);
            Document.DateUploaded = DateTime.Now;
            bool res = uploadDocument(Document);
            return res;
        }
        public async Task<string> getText(string data)
        {
            //get ocr text
            await _publisher.PublishNewDocumentAsync(data);
            return "";
        }

        public IEnumerable<DocumentData> getAllDocuments()
        {
            return _repository.GetAll();
        }

        public bool deleteDocument(string id)
        {
            _repository.Delete(Guid.Parse(id));
            var res = _repository.GetById(Guid.Parse(id));
            if (res == null)
            {
                return true;
            }
            return false;
        }

    }
}
