using PlayFabBuddy.Lib.Aggregate;
using PlayFabBuddy.Lib.Interfaces.Adapter;
using PlayFabBuddy.Lib.UseCases.Player;
using System.Net;

namespace PlayFabBuddy.UI.Data
{
    public class PlayFabService
    {
        private readonly IDataExplorerAdapter _dataAdapter;
        private readonly IPlayerAccountAdapter _playerAdapter;

        public PlayFabService(IDataExplorerAdapter dataAdapter, IPlayerAccountAdapter playerAccountAdapter)
        {
            _dataAdapter = dataAdapter;
            _playerAdapter = playerAccountAdapter;
        }

        public Task<WeatherForecast[]> GetForecastAsync()
        {
            string ipAddress = "123.123.123.0";

            var getPlayerUsecase = new GetPlayersByIpUseCAse(_dataAdapter, _playerAdapter, IPAddress.Parse(ipAddress));

            return Task.FromResult(Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
            }).ToArray());
        }
    }
}
