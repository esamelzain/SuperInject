namespace SuperInject.Api.Services;

public interface IWeatherService
{
    IEnumerable<WeatherForecast> Get();
}
