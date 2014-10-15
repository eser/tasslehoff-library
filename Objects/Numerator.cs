// -----------------------------------------------------------------------
// <copyright file="Numerator.cs" company="-">
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

namespace Tasslehoff.Library.Objects
{
    using System;
    using System.Runtime.Serialization;
    using System.Security.Permissions;

    /// <summary>
    /// Numerator class.
    /// </summary>
    [DataContract]
    public class Numerator : ICloneable, ISerializable
    {
        // fields

        /// <summary>
        /// The sync lock
        /// </summary>
        [IgnoreDataMember]
        private readonly object syncLock = new object();

        /// <summary>
        /// The next number
        /// </summary>
        [DataMember]
        private int nextNumber;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Numerator"/> class.
        /// </summary>
        /// <param name="startNumber">Starting number</param>
        public Numerator(int startNumber = int.MinValue)
        {
            this.nextNumber = startNumber;
        }

        /// <summary>
        /// Constructor for serialization interface
        /// </summary>
        /// <param name="info">info</param>
        /// <param name="context">context</param>
        protected Numerator(SerializationInfo info, StreamingContext context)
        {
            this.nextNumber = info.GetInt32("nextNumber");
        }

        // methods

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns>The next number</returns>
        public int Get()
        {
            lock (this.syncLock)
            {
                unchecked
                {
                    ////if(this.nextNumber + 1 >= int.MaxValue) {
                    ////    this.nextNumber = int.MinValue;
                    ////}

                    return this.nextNumber++;
                }
            }
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        object ICloneable.Clone()
        {
            return new Numerator(this.nextNumber);
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        protected void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("nextNumber", this.nextNumber);
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            this.GetObjectData(info, context);
        }
     }
}