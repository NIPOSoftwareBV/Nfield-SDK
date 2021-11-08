//    This file is part of Nfield.SDK.
//
//    Nfield.SDK is free software: you can redistribute it and/or modify
//    it under the terms of the GNU Lesser General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    Nfield.SDK is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU Lesser General Public License for more details.
//
//    You should have received a copy of the GNU Lesser General Public License
//    along with Nfield.SDK.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nfield.Extensions;
using Nfield.Infrastructure;
using Nfield.Models;

namespace Nfield.Services.Implementation
{
    /// <summary>
    /// Implementation of <see cref="INfieldLanguagesService"/>
    /// </summary>
    internal class NfieldDomainLanguagesService : INfieldDomainLanguagesService, INfieldConnectionClientObject
    {
        #region Implementation of INfieldLanguagesService

        /// <summary>
        /// See <see cref="INfieldLanguagesService.QueryAsync"/>
        /// </summary>
        public Task<IQueryable<Language>> QueryAsync()
        {
            return Client.GetAsync(LanguagesApi())
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             stringTask =>
                             JsonConvert.DeserializeObject<List<Language>>(stringTask.Result).AsQueryable())
                         .FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldLanguagesService.AddAsync"/>
        /// </summary>
        public Task<Language> AddAsync(Language language)
        {
            if (language == null)
            {
                throw new ArgumentNullException("language");
            }

            return Client.PostAsJsonAsync(LanguagesApi(), language)
                         .ContinueWith(task => task.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(task => JsonConvert.DeserializeObject<Language>(task.Result))
                         .FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldLanguagesService.RemoveAsync"/>
        /// </summary>
        public Task RemoveAsync(Language language)
        {
            if (language == null)
            {
                throw new ArgumentNullException("language");
            }

            return
                Client.DeleteAsync(LanguagesApi(language.Id))
                      .FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldLanguagesService.UpdateAsync"/>
        /// </summary>
        public Task UpdateAsync(Language language)
        {
            if (language == null)
            {
                throw new ArgumentNullException("language");
            }

            return Client.PutAsJsonAsync(LanguagesApi(),
                language).FlattenExceptions();
        }

        #endregion


        #region Implementation of INfieldConnectionClientObject

        public INfieldConnectionClient ConnectionClient { get; internal set; }

        public void InitializeNfieldConnection(INfieldConnectionClient connection)
        {
            ConnectionClient = connection;
        }

        #endregion

        private INfieldHttpClient Client
        {
            get { return ConnectionClient.Client; }
        }

        private Uri LanguagesApi(int? id = null)
        {   
            var path = new StringBuilder();
            path.AppendFormat("Languages");
            if (id != null)
            {
                path.AppendFormat("/{0}", id);
            }
            return new Uri(ConnectionClient.NfieldServerUri, path.ToString());
        }

    }
}
