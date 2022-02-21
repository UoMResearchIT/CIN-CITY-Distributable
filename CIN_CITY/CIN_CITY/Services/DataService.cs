using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CIN_CITY.Services
{
    // Implementation of the interface for data collection and storage
    public sealed class DataService : IDataService
    {
        private static readonly Lazy<IDataService> lazy = new Lazy<IDataService>(() => new DataService());

        public static IDataService Instance { get { return lazy.Value; } }

        private DataService()
        {
        }

        public bool IsConnected()
        {
            return Connectivity.NetworkAccess == NetworkAccess.Internet;
        }

        public Task<bool> CreateAndUploadRecord()
        {
            throw new NotImplementedException();
        }

        public bool IsAuthenticated()
        {
            throw new NotImplementedException();
        }

        public Task<bool> LoginAsync(string username, string password)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RegisterAsync(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}