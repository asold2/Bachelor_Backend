public interface IScrapper
{
    public Task<string> GetHtml(string url);
}