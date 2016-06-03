using System.Net.Http;
using System.Threading.Tasks;
using Nfield.Models;

namespace Nfield.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface INfieldRespondentDataEncryptService
    {
        /// <summary>
        /// Encrypts the data.
        /// </summary>
        /// <param name="surveyId">The survey identifier.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<string> EncryptData(string surveyId, DataCryptographyModel model);
    }
}
