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

using System.Linq;
using System.Threading.Tasks;
using Nfield.SDK.Models.Events;

namespace Nfield.Services
{
    /// <summary>
    /// Set of methods to manage the event subscriptions.
    /// </summary>
    public interface INfieldEventSubscriptionsService
    {
        /// <summary>
        /// Gets a list of event subscriptions for the domain.
        /// </summary>
        /// <returns>A list of subscriptions or an empty list if none exists.</returns>
        Task<IQueryable<EventSubscriptionModel>> QueryAsync();

        /// <summary>
        /// Gets the event subscription for the provided identifier.
        /// </summary>
        /// <param name="subscriptionName">The name of the subscription.</param>
        /// <returns>The event subscription associated to the identifier. If it doesn't exist, returns a NotFound response.</returns>
        Task<EventSubscriptionModel> GetAsync(string subscriptionName);

        /// <summary>
        /// Creates a new event subscription.
        /// </summary>
        /// <param name="model">The event subscription that will be created.</param>
        /// <returns>The subscription name, if succeeded. The appropriate exception in case of failure.</returns>
        Task<CreatedEventSubscriptionModel> CreateAsync(CreateEventSubscriptionModel model);

        /// <summary>
        /// Updates an event subscription.
        /// </summary>
        /// <param name="subscriptionName">The event subscription name.</param>
        /// <param name="model">The subscription info which will be used in the update process.</param>
        /// <returns><c>NoContentResult</c>, if succeeded. The appropriate exception in case of failure.</returns>
        /// <remarks>
        /// You can update the endpoint or the event types or both:
        /// - Endpoint: This is the web hook endpoint. This should be a valid Uri. It may require to validate the new endpoint before being able to use it.
        /// - EventTypes: The values supplied here will replace the existing event types. You should supply a full list of the event types you want to subscribe to.
        /// </remarks>
        Task UpdateAsync(string subscriptionName, UpdateEventSubscriptionModel model);

        /// <summary>
        /// Deletes an existing event subscription.
        /// </summary>
        /// <param name="subscriptionName">The subscription name.</param>
        ///<returns> Returns a <c>NoContent</c> response, when successful. The appropriate exception will be thrown in case of failure.</returns>
        Task DeleteAsync(string subscriptionName);
    }
}
