using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

namespace DocumentManagementSystem.Controllers
{
    [Route("api/documents")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        [HttpGet]
        public IValueHttpResult GetDocuments([FromBody] string parameters)
        {
            //if parameters not empty, pass it on to the search, otherwise list all documents
            throw new NotImplementedException();
        }

        [HttpPost]
        public IValueHttpResult NewDocument(/*figure out new document input format*/)
        {
            //parse the input and send it on to the new document processing
            
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public IValueHttpResult DeleteDocument([FromRoute(Name = "id")]string id) 
        { 
            //find document with this id and pass it on to delete it
            throw new NotImplementedException(); 
        }

        [HttpGet("{id}")]
        public IValueHttpResult GetDocument([FromRoute(Name = "id")] string id)
        {
            //find specific document and return it
            throw new NotImplementedException();
        }

        [HttpPut("{id}")]
        public IValueHttpResult UpdateDocument([FromRoute(Name = "id")] string id, [FromBody] string body) 
        { 
            //find specific document and change to body input
            throw new NotImplementedException(); 
        }

        [HttpGet("{id}/data")]
        public IValueHttpResult GetMetadata([FromRoute(Name = "id")] string id) 
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
