using DocumentManagementSystem.Models;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DocumentManagementSystem.Controllers
{
    public class Summarizer
    {
        private string Url = "http://model-runner.docker.internal/engines/llama.cpp/v1/";

        public string returnSummary(string doc) 
        {
            string summary = "";
            using (var http = new HttpClient { BaseAddress = new Uri(Url) })
            {
                http.DefaultRequestHeaders.Clear();
                http.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
                
                using (var content = new StringContent($"\"model\": \"hf.co/tonyc666/text_summarization-q4_k_m-gguf:q4_k_m\", \"messages\": [ {{ \"role\": \"user\",  \"content\": \"Summarise the following text: {doc}\" }} ], }}'"))
                {
                    using (var response =  http.PostAsync($"chat/completions", content))
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
