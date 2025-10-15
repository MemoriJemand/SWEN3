using DocumentManagementSystem.DataAccess;
using DocumentManagementSystem.Models;
using DocumentManagementSystem.Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

namespace DocumentManagementSystem.Controllers
{
    [Route("api/documents")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        IDocumentRepository _repository;
        Messenger _messenger = new();
        [HttpGet]
        public ActionResult<IEnumerable<DocumentData>> GetDocuments([FromBody] string parameters)
        {
            //if parameters not empty, pass it on to the search, otherwise list all documents
            if (parameters == "")
            {
                var shownDocuments = _repository.GetAll();
                return Ok(shownDocuments);
            }
            else
            {
                throw new NotImplementedException();
            }
            
        }

        [HttpPost]
        public IActionResult NewDocument([FromBody] string document)
        {
            //send the document on to the ocr worker and receive the result
            _messenger.Sender.SendDocument(document);
            var info = _messenger.Receiver.ReceiveInfo().Result;
            //still have to add return parse into doc data
            DocumentData newDoc = new();
            _repository.Insert(newDoc);
            return StatusCode(StatusCodes.Status200OK);
            //throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteDocument([FromRoute(Name = "id")]string id) 
        { 
            //find document with this id and pass it on to delete it
            _repository.Delete(Guid.Parse(id));
            return StatusCode(StatusCodes.Status200OK);
            //throw new NotImplementedException(); 
        }

        [HttpGet("{id}")]
        public ActionResult<DocumentData> GetDocument([FromRoute(Name = "id")] string id)
        {
            //find specific document and return it
            var document = _repository.GetById(Guid.Parse(id));
            return Ok(document);
            //throw new NotImplementedException();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateDocument([FromRoute(Name = "id")] string id, [FromBody] string body) 
        { 
            //find specific document and change to body input
            DocumentData data = new DocumentData(); //parse once again
            _repository.Update(data);
            return Ok();
            //throw new NotImplementedException(); 
        }

        [HttpGet("{id}/data")]
        public IActionResult GetMetadata([FromRoute(Name = "id")] string id) 
        { 
            //find specific document and return its metadata
            throw new NotImplementedException(); 
        }

    }
}

//for complicated body reading
/*using (var reader = new StreamReader(Request.Body))
            {
                var body = reader.ReadToEnd();

                // Do something
            }*/
