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
        string apiUrl = "https://run.mocky.io/v3/84fc75f8-7f16-4eff-98e0-2be9d48fea3c";

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
