// -----------------------------------------------------------------------
// <copyright file="DataQueryManagerItem.cs" company="-">
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

    /// <summary>
    /// DataQueryManagerItem class.
    /// </summary>
    public class DataQueryManagerItem
    {
        // fields

        /// <summary>
        /// Key.
        /// </summary>
        private string key;

        /// <summary>
        /// Sql Command.
        /// </summary>
        private string sqlCommand;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataQueryManagerItem"/> class.
        /// </summary>
        public DataQueryManagerItem()
        {
        }

        // properties

        /// <summary>
        /// Gets or Sets the key.
        /// </summary>
        public string Key
        {
            get
            {
                return this.key;
            }

            set
            {
                this.key = value;
            }
        }

        /// <summary>
        /// Gets or Sets the sql command.
        /// </summary>
        public string SqlCommand
        {
            get
            {
                return this.sqlCommand;
            }

            set
            {
                this.sqlCommand = value;
            }
        }
    }
}
