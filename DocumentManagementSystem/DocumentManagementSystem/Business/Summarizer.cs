using DocumentManagementSystem.Models;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DocumentManagementSystem.Business
{
    public class Summarizer
    {
        private string Url = "http://model-runner.docker.internal";

        public string returnSummary(string doc) 
        {
            string summary = "";
            using (var http = new HttpClient { BaseAddress = new Uri(Url) })
            {
                http.DefaultRequestHeaders.Clear();
                http.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
                
                using (var content = new StringContent($"{{\"model\": \"ai/smollm2\",\"messages\": [ {{ \"role\": \"user\",  \"content\": \"Summarise the following text: {doc}\" }} ] }}"))
                    
                {
                    using (var response =  http.PostAsync($"/engines/v1/completions", content))
                    {
                        response.Wait();
                        var result = response.Result;
                        summary = result.Content.ReadAsStringAsync().Result;
                    }
                }
            }
            return summary;
        }
    }
}
