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
        INewDocumentReceiver _receiver;
        IMinioClient _minio;
        ILogger _logger;
        private Summarizer _summarizer = new();

        private string getSummary(string text) 
        {
            //send to ai
            _logger.LogInformation("Asking for summary from the AI.");
            var result = _summarizer.returnSummary(text);
            if (result != null)
            {
                _logger.LogDebug("Received a response, processing further.");
                return result;
            }
            else
            {
                _logger.LogError("Received no summary from the AI.");
                return "";
            }
            
        }
        private bool uploadDocument(DocumentData data) 
        {
            //save to repository
            _logger.LogInformation("Saving document to repository.");
            _repository.Insert(data);
            var check = _repository.GetById(data.Id);
            return (check != null);
        }
        private string uploadOriginal(byte[] data)
        {
            //save to minio
            _logger.LogDebug("Saving the file temporarily.");
            var path = Path.Combine("/tempFile", Path.GetRandomFileName());
            File.WriteAllBytes(path, data);
            _logger.LogInformation("Uploading file to MinIO.");
            var result = FileUpload.Run(_minio, path).Result;
            if (result != null)
            {
                if (File.Exists(path))
                {
                    _logger.LogDebug("Deleting temporary file.");
                    File.Delete(path);
                }
                return result;
            }
            return "";
        }
        public DocumentData? getDocumentById(string id) 
        {
            //get data from repository
            _logger.LogInformation("Looking for the document.");
            var result = _repository.GetById(Guid.Parse(id));
            if (result != null)
            {
                _logger.LogDebug("Document found.");
            }
            return result;
        }
        public bool newDocument(string name, byte[] file, string? tags) 
        {
            //transform input into repository data
            _logger.LogInformation("Creating new document.");
            DocumentData Document = new DocumentData();
            Document.Title = name;
            Document.Tags = tags?? "";
            Document.Original = uploadOriginal(file);
            Document.Text = getText(file.ToString()!).Result;
            Document.Summary = getSummary(Document.Text);
            Document.DateUploaded = DateTime.Now;
            bool res = uploadDocument(Document);
            _logger.LogInformation($"Document saved at: {Document.DateUploaded}.");
            return res;
        }
        public async Task<string> getText(string data)
        {
            //get ocr text
            _logger.LogInformation("Waiting for OCR text processing.");
            await _publisher.PublishNewDocumentAsync(data);
            return "";
        }

        public IEnumerable<DocumentData> getAllDocuments()
        {
            _logger.LogInformation("Accessing all available documents.");
            return _repository.GetAll();
        }

        public bool deleteDocument(string id)
        {
            _logger.LogDebug("Deleting the document.");
            _repository.Delete(Guid.Parse(id));
            var res = _repository.GetById(Guid.Parse(id));
            if (res == null)
            {
                _logger.LogInformation("Deletion successful.");
                return true;
            }
            _logger.LogError("Deletion unsuccessful.");
            return false;
        }

    }
}
