// -----------------------------------------------------------------------
// <copyright file="WebServiceManager.cs" company="-">
// Copyright (c) 2014 Eser Ozvataf (eser@sent.com). All rights reserved.
// Web: http://eser.ozvataf.com/ GitHub: http://github.com/larukedi
// </copyright>
// <author>Eser Ozvataf (eser@sent.com)</author>
// -----------------------------------------------------------------------

//// This program is free software: you can redistribute it and/or modify
//// it under the terms of the GNU General Public License as published by
//// the Free Software Foundation, either version 3 of the License, or
//// (at your option) any later version.
//// 
//// This program is distributed in the hope that it will be useful,
//// but WITHOUT ANY WARRANTY; without even the implied warranty of
//// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//// GNU General Public License for more details.
////
//// You should have received a copy of the GNU General Public License
//// along with this program.  If not, see <http://www.gnu.org/licenses/>.

namespace Tasslehoff.Library.WebServices
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ServiceModel.Web;
    using Tasslehoff.Library.Helpers;
    using Tasslehoff.Library.Services;

    /// <summary>
    /// WebServiceManager class.
    /// </summary>
    public class WebServiceManager : ServiceControllable
    {
        // constants

        /// <summary>
        /// The default stop timeout seconds
        /// </summary>
        public const int DefaultStopTimeoutSeconds = 60;

        // fields

        /// <summary>
        /// The base addresses
        /// </summary>
        private readonly IEnumerable<string> baseAddresses;

        /// <summary>
        /// The service hosts
        /// </summary>
        private readonly IDictionary<string, WebServiceHost> serviceHosts;

        /// <summary>
        /// The endpoints
        /// </summary>
        private readonly ICollection<WebServiceEndpoint> endpoints;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WebServiceManager"/> class.
        /// </summary>
        /// <param name="baseAddress">The base address</param>
        public WebServiceManager(string baseAddress) : this(new string[] { baseAddress })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebServiceManager" /> class.
        /// </summary>
        /// <param name="baseAddresses">The base addresses</param>
        public WebServiceManager(IEnumerable<string> baseAddresses) : base()
        {
            this.baseAddresses = baseAddresses;
            this.serviceHosts = new Dictionary<string, WebServiceHost>();
            this.endpoints = new Collection<WebServiceEndpoint>();
        }

        // properties

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public override string Name
        {
            get
            {
                return "WebServiceManager";
            }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public override string Description
        {
            get
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the base addresses.
        /// </summary>
        /// <value>
        /// The base addresses.
        /// </value>
        public IEnumerable<string> BaseAddresses
        {
            get
            {
                return this.baseAddresses;
            }
        }

        /// <summary>
        /// Gets the service hosts.
        /// </summary>
        /// <value>
        /// The service hosts.
        /// </value>
        public IDictionary<string, WebServiceHost> ServiceHosts
        {
            get
            {
                return this.serviceHosts;
            }
        }

        /// <summary>
        /// Gets the endpoints.
        /// </summary>
        /// <value>
        /// The endpoints.
        /// </value>
        public ICollection<WebServiceEndpoint> Endpoints
        {
            get
            {
                return this.endpoints;
            }
        }

        // methods

        /// <summary>
        /// Adds the specified web service endpoint.
        /// </summary>
        /// <param name="webServiceEndpoint">The web service endpoint</param>
        public void Add(WebServiceEndpoint webServiceEndpoint)
        {
            this.endpoints.Add(webServiceEndpoint);
        }

        /// <summary>
        /// Services the start.
        /// </summary>
        protected override void ServiceStart()
        {
            foreach (WebServiceEndpoint endpoint in this.endpoints)
            {
                // prepare base addresses
                List<Uri> collection = new List<Uri>();
                foreach (string item in this.baseAddresses)
                {
                    collection.Add(new Uri(item.TrimEnd('/') + "/" + endpoint.Name));
                }

                // construct serviceHost
                WebServiceHost serviceHost = new WebServiceHost(endpoint.Type, collection.ToArray());

                this.serviceHosts.Add(endpoint.Name, serviceHost);
                serviceHost.Open();
            }
        }

        /// <summary>
        /// Services the stop.
        /// </summary>
        protected override void ServiceStop()
        {
            WebServiceHost[] serviceHosts = ArrayHelpers.GetArray<WebServiceHost>(this.serviceHosts.Values);
            Array.Reverse(serviceHosts);

            foreach (WebServiceHost serviceHost in serviceHosts)
            {
                serviceHost.Close(TimeSpan.FromSeconds(WebServiceManager.DefaultStopTimeoutSeconds));
            }

            this.serviceHosts.Clear();
        }
    }
}
