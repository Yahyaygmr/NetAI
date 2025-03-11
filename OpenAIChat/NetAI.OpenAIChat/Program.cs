
using System.Text;
using System.Text.Json;

var apiKey = "apikey";
Console.WriteLine("Lütfen sorunuzu girin :");

var prompt = Console.ReadLine();

using var httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

var requestBody = new
{
    model = "gpt-3.5-turbo",
    messages = new[]
    {
        new
        {
            role = "system",
            content = "You are a helpful asistant"
        },
        new
        {
            role = "user",
            content = prompt!.ToString()
        },
    },
    max_tokens = 100
};

var json = JsonSerializer.Serialize(requestBody);
var content = new StringContent(json, Encoding.UTF8, "application/json");

try
{
    var response = await httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
    if (response.IsSuccessStatusCode)
    {
        var responseString = await response.Content.ReadAsStringAsync();
        if (!string.IsNullOrEmpty(responseString))
        {
            var result = JsonSerializer.Deserialize<JsonElement>(responseString);
            var answer = result
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();
            Console.WriteLine($"Cevap :  {answer}");
        }
    }
    else
    {
        Console.WriteLine($"Bir hata oluştu. Hata : {response.StatusCode}");
    }

}
catch (Exception ex)
{
    Console.WriteLine($"Bir hata oluştu. Hata : {ex.Message}");
}

//Console.WriteLine($"Cevap : \n {prompt}");