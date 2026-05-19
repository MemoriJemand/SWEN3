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
        IDocumentRepository _repository;
        INewDocumentPublisher _publisher;
        Summarizer _summarizer;
        public DocumentController(IDocumentRepository documentRepository)
        {
            _repository = documentRepository;   
        }
        
        [HttpGet]
        public ActionResult<IEnumerable<DocumentData>> GetDocuments()
        {
            var shownDocuments = _repository.GetAll();
            return Ok(shownDocuments ?? new List<DocumentData>());
        }


        [HttpPost]
        public async Task<IActionResult> NewDocument([FromForm] IFormFile file, [FromForm] string name)
        {
            using var reader = new StreamReader(file.OpenReadStream());
            var content = await reader.ReadToEndAsync();
            string testContent = "A Christian holiday signifying the birth of Jesus, Christmas is widely celebrated and enjoyed across the United States and the world. The holiday always falls on 25 December (regardless of the day of the week), and is typically accompanied by decorations, presents, and special meals.\r\n\r\nSpecifically, the legend behind Christmas (and the one that most children are told) is that Santa Claus, a bearded, hefty, jolly, and red-jacket-wearing old man who lives in the North Pole, spends the year crafting presents with his elves, or small, festive, excited Santa-assistants. All the children who behave throughout the year are admitted to the Good List, and will presumably receive their desired gifts on Christmas, while those who don't behave are placed on the Naughty List, and will presumably (although the matter is determined by parents) receive a lump of coal.\r\n\r\nSanta Claus is said to fly around the Christmas sky in a sled powered by his magical reindeer, or cold-resistant, mythically powered, individually named animals, delivering presents to each child's house in the process. Santa is also expected to slide through chimneys to deliver these presents (homes not equipped with chimneys might \"leave the front door cracked open\"), and children sometimes arrange cookies or other treats on a plate for him to enjoy.\r\n\r\nGifts are placed underneath a Christmas tree, or a pine tree that's decorated with ornaments and/or lights and is symbolic of the holiday. Additionally, smaller gifts may be placed inside a stocking, or a sock-shaped, holiday-specific piece of fabric that's generally hung on the mantle of a fireplace (homes without fireplaces might use the wall). A Christmas tree's ornaments, or hanging, typically spherical decorations, in addition to the mentioned lights, may be accompanied by a star, or a representation of the Star of Jerusalem that the Three Apostles followed while bringing Baby Jesus gifts and honoring him, in the Bible.";
            var newDoc = new DocumentData
            {
                Title = name,
                Text = content,
                Summary = _summarizer.returnSummary(testContent)
            };

            await _publisher.PublishNewDocumentAsync(content);

            _repository.Insert(newDoc);
            return Ok();
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
