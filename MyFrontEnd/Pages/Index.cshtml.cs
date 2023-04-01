using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static Google.Rpc.Context.AttributeContext.Types;

namespace MyFrontEnd.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly DaprClient _daprClient;
    private readonly HttpClient _httpClient;
    private readonly IHttpClientFactory _httpClientFactory;


    public IndexModel(ILogger<IndexModel> logger, DaprClient daprClient, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _daprClient = daprClient;
        _httpClientFactory = httpClientFactory;
        _httpClient = _httpClientFactory.CreateClient("restClient");
    }

    public async Task OnGet()
    {
        //using dapr
        var forecasts = await _daprClient.InvokeMethodAsync<IEnumerable<WeatherForecast>>(
                        HttpMethod.Get,
                        "MyBackEnd",
                        "WeatherForecast");

        ViewData["WeatherForecastDataDapr"] = forecasts;


        //using rest client
        var forecasts1 = await _httpClient.GetAsync("WeatherForecast");
        var jsonResponse = await forecasts1.Content.ReadFromJsonAsync<IEnumerable<WeatherForecast>>();


        ViewData["WeatherForecastData"] = jsonResponse;
    }
}

