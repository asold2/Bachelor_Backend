
using HtmlAgilityPack;

public class EboksServiceImpl : IEboksService
{
    private readonly IScrapper _scrapper;
    private readonly ILogger<EboksServiceImpl> _logger;

    public EboksServiceImpl(IScrapper scrapper, ILogger<EboksServiceImpl> logger)
    {
        _scrapper = scrapper;
        _logger = logger;
    }


    public async Task<int> GetNumberOfServicesDown()
    {
        var htmlResult = await _scrapper.GetHtml("https://status.e-boks.com/");

        HtmlDocument htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(htmlResult);

        int servicesDown = 0;

        var divServices = htmlDoc.DocumentNode.Descendants("div")
            .Where(node => node.GetAttributeValue("class", "")
            .Contains("component-inner-container"))
            .ToList();

        List<string> services = divServices.Select(service => service.InnerText.Trim()).ToList();


        foreach (var service in services)
        {

            if (!service.Contains("Operational"))
            {
                servicesDown++;
                _logger.LogInformation("Service down: " + service);
            }
        }

        return servicesDown;
    }
}