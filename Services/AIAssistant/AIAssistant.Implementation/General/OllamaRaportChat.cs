using Microsoft.Extensions.Configuration;
using OllamaSharp;

namespace AIAssistant.Implementation.General;

public class OllamaRaportChat
{
    private readonly IConfiguration _configuration;
    private readonly OllamaApiClient _client;

    public OllamaRaportChat(IConfiguration configuration)
    {
        _configuration = configuration;

        string endpoint = _configuration.GetValue<string>("Ollama:Endpoint");
        string model = _configuration.GetValue<string>("Ollama:Model");

        _client = new OllamaApiClient(endpoint, model);
    }

    public async Task<string> HelloWorld()
    {
        string result = string.Empty;

        var res = _client.GenerateAsync("Hello, who are you?");
        await foreach (var item in res)
        {
            result += item.Response;
        }

        return result;
    }

    /// <summary>
    /// Returns description for given measurement input.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<string> GenerateDescription(string input)
    {
        string result = string.Empty;

        string prompt = $"I will provide data for collected measurements for you. " +
            $"Your job is to analyze this data and create description of this measurements. " +
            $"I only want conclusions in one paragraph, plain text, witout obsolete symbols. Make sure answer is at least 300 words long. Reduce number of digits after decimal point to two. Dont put any notes to reponse." +
            $"Examplary answear:" +
            $"```" +
            $"Temperature measurements were concluded in two rooms ..." +
            $"```" +
            $"Data: {input}";

        var res = _client.GenerateAsync(prompt);
        await foreach (var item in res)
        {
            result += item.Response;
        }

        return result;
    }

}
