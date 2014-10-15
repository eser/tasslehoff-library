// -----------------------------------------------------------------------
// <copyright file="WebServiceEndpoint.cs" company="-">
// Copyright (c) 2013 larukedi (eser@sent.com). All rights reserved.
// </copyright>
// <author>larukedi (http://github.com/larukedi/)</author>
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
    using System.Runtime.Serialization;

    /// <summary>
    /// WebServiceEndpoint class.
    /// </summary>
    [DataContract]
    public class WebServiceEndpoint : ISerializable
    {
        // fields

        /// <summary>
        /// The name
        /// </summary>
        [DataMember]
        private string name;

        /// <summary>
        /// The type
        /// </summary>
        [DataMember]
        private Type type;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WebServiceEndpoint" /> class.
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="type">The type</param>
        public WebServiceEndpoint(string name, Type type)
        {
            this.name = name;
            this.type = type;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebServiceEndpoint"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination</param>
        protected WebServiceEndpoint(SerializationInfo info, StreamingContext context)
        {
            this.name = info.GetString("name");
            this.type = (Type)info.GetValue("type", typeof(Type));
        }

        // properties

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [IgnoreDataMember]
        public string Name
        {
            get
            {
                return this.name;
            }
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        [IgnoreDataMember]
        public Type Type
        {
            get
            {
                return this.type;
            }
        }

        // methods

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("name", this.name);
            info.AddValue("type", this.type);
        }

    }
}
