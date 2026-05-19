using DocumentManagementSystem.DataAccess;
using DocumentManagementSystem.Models;
using DocumentManagementSystem.Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using DocumentManagementSystem.Business;

namespace DocumentManagementSystem.Controllers
{
    [Route("api/documents")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        IBusinessLayer _bridge;
        
        [HttpGet]
        public ActionResult<IEnumerable<DocumentData>> GetDocuments()
        {
            var shownDocuments = _bridge.getAllDocuments();
            return Ok(shownDocuments ?? new List<DocumentData>());
        }


        [HttpPost]
        public async Task<IActionResult> NewDocument([FromForm] IFormFile file, [FromForm] string name, [FromForm] string tags)
        {
            using var reader = new StreamReader(file.OpenReadStream());
            var content = await reader.ReadToEndAsync();
            //figure out if that's correct for reading the file
            if (_bridge.newDocument(name, content, tags))
            {
                return Ok();
            }
            else
            {
                return StatusCode(500);
            }
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteDocument([FromRoute(Name = "id")]string id) 
        {
            //find document with this id and pass it on to delete it
            if (_bridge.deleteDocument(id))
            {
                return Ok();
            }
            else
            {
                return StatusCode(500);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<DocumentData> GetDocument([FromRoute(Name = "id")] string id)
        {
            //find specific document and return it
            var res = _bridge.getDocumentById(id);
            if (res != null)
            {
                return Ok(res);
            }
            return StatusCode(404);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateDocument([FromRoute(Name = "id")] string id, [FromBody] string body) 
        {
            //find specific document and change to body input
            //DocumentData data = new DocumentData(); //parse once again
            //_repository.Update(data);
            //return Ok(); 
            return StatusCode(501);
        }

        [HttpGet("{id}/data")]
        public IActionResult GetMetadata([FromRoute(Name = "id")] string id) 
        {
            //find specific document and return its metadata
            return StatusCode(501);
        }

    }
}

//for complicated body reading
/*using (var reader = new StreamReader(Request.Body))
            {
                var body = reader.ReadToEnd();

                // Do something
            }*/
