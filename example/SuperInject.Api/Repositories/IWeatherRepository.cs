namespace SuperInject.Api.Repositories;

public interface IWeatherRepository
{
    IEnumerable<WeatherForecast> Get();
}