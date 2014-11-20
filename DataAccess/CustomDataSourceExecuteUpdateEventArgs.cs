// -----------------------------------------------------------------------
// <copyright file="CustomDataSourceExecuteUpdateEventArgs.cs" company="-">
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

namespace Tasslehoff.Library.DataAccess
{
    using System;
    using System.Collections;

    /// <summary>
    /// CustomDataSourceExecuteUpdateEventArgs class.
    /// </summary>
    public class CustomDataSourceExecuteUpdateEventArgs : EventArgs
    {
        // fields

        /// <summary>
        /// The keys
        /// </summary>
        private readonly IDictionary keys;

        /// <summary>
        /// The values
        /// </summary>
        private readonly IDictionary values;

        /// <summary>
        /// The old values
        /// </summary>
        private readonly IDictionary oldValues;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomDataSourceExecuteUpdateEventArgs"/> class.
        /// </summary>
        /// <param name="keys">The keys</param>
        /// <param name="values">The values</param>
        /// <param name="oldValues">The old values</param>
        public CustomDataSourceExecuteUpdateEventArgs(IDictionary keys, IDictionary values, IDictionary oldValues)
            : base()
        {
            this.keys = keys;
            this.values = values;
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
        /// Gets the values.
        /// </summary>
        public IDictionary Values
        {
            get
            {
                return this.values;
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