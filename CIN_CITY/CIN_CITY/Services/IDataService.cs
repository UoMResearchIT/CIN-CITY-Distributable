using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIN_CITY.Services
{
    public interface IDataService
    {
        /// <summary>
        /// Check for internet connectivity
        /// </summary>
        /// <returns>Whether internet is available</returns>
        bool IsConnected();

        /// <summary>
        /// Create a record and send to backend
        /// </summary>
        /// <returns>Whether the operation was successful</returns>
        Task<bool> CreateAndUploadRecord();

        /// <summary>
        /// Check whether user has stored credentials for a connection to the backend
        /// </summary>
        /// <returns>Whether credentials exist</returns>
        bool IsAuthenticated();

        /// <summary>
        /// Login to backend
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<bool> LoginAsync(string username, string password);

        /// <summary>
        /// Register with backend
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<bool> RegisterAsync(string username, string password);
    }
}
