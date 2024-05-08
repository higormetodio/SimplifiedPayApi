using SimplifiedPayApi.Models;
using SimplifiedPayApi.Repositories;
using System.Text.Json;

namespace SimplifiedPayApi.Services;

public class TransactionService
{
    private readonly HttpClient _httpClient;

    public TransactionService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> TransactionValidation()
    {
        string apiUrl = "https://run.mocky.io/v3/1c0326e4-7880-4732-ac30-412da17ec774";

        var response = await _httpClient.GetAsync(apiUrl);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();

            var authorizeTransaction =  JsonSerializer.Deserialize<AuthorizeTransaction>(content)!;

            return authorizeTransaction.message;
        }

        throw new Exception($"Error when trying to authorize the transaction: {response.StatusCode}");
    }
}
