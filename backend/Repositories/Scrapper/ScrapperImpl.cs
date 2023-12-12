
public class ScrapperImpl : IScrapper
{

    private readonly HttpClient _client;

    public ScrapperImpl()
    {
        _client = new HttpClient();
    }

    public async Task<string> GetHtml(string url)
    {
        var response = await _client.GetStringAsync(url);
        return response;
    }
}