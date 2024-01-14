using SuperInject.Api.Repositories;

namespace SuperInject.Api.Services;

[Service(ServiceLifetime.Scoped)]
internal class WeatherService : IWeatherService
{
    private readonly IWeatherRepository _weatherRepository;   
    public WeatherService(IWeatherRepository weatherRepository)
    {
        _weatherRepository = weatherRepository;
    }
    public IEnumerable<WeatherForecast> Get()
    {
        return _weatherRepository.Get();
    }
}
