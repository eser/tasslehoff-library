// -----------------------------------------------------------------------
// <copyright file="CustomDataSourceExecuteDeleteEventArgs.cs" company="-">
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

namespace Tasslehoff.Library.DataAccess
{
    using System;
    using System.Collections;

    /// <summary>
    /// CustomDataSourceExecuteDeleteEventArgs class.
    /// </summary>
    public class CustomDataSourceExecuteDeleteEventArgs : EventArgs
    {
        // fields

        /// <summary>
        /// The keys
        /// </summary>
        private readonly IDictionary keys;

        /// <summary>
        /// The old values
        /// </summary>
        private readonly IDictionary oldValues;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomDataSourceExecuteDeleteEventArgs"/> class.
        /// </summary>
        /// <param name="keys">The keys</param>
        /// <param name="oldValues">The old values</param>
        public CustomDataSourceExecuteDeleteEventArgs(IDictionary keys, IDictionary oldValues)
            : base()
        {
            this.keys = keys;
            this.oldValues = oldValues;
        }

        // properties

        /// <summary>
        /// Gets the keys.
        /// </summary>
        public IDictionary Keys
        {
            get
            {
                return this.keys;
            }
        }

        /// <summary>
        /// Gets the old values.
        /// </summary>
        public IDictionary OldValues
        {
            get
            {
                return this.oldValues;
            }
        }
    }
}