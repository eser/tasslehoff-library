// -----------------------------------------------------------------------
// <copyright file="DataQueryManager.cs" company="-">
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
    using System.Collections.Generic;

    /// <summary>
    /// DataQueryManager class.
    /// </summary>
    public class DataQueryManager
    {
        // fields

        /// <summary>
        /// Database instance.
        /// </summary>
        private Database database;

        /// <summary>
        /// Queries
        /// </summary>
        private Dictionary<string, DataQueryManagerItem> queries;

        /// <summary>
        /// Query placeholders.
        /// </summary>
        private Dictionary<string, string> queryPlaceholders;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataQueryManager"/> class.
        /// </summary>
        public DataQueryManager()
        {
            this.queries = new Dictionary<string, DataQueryManagerItem>();
            this.queryPlaceholders = new Dictionary<string, string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataQueryManager"/> class.
        /// </summary>
        /// <param name="database"></param>
        public DataQueryManager(Database database)
            : this()
        {
            this.database = database;
        }

        // events

        /// <summary>
        /// Occurs when [on getting a query].
        /// </summary>
        public event EventHandler OnQueryGet;

        // properties

        /// <summary>
        /// Gets or Sets database instance.
        /// </summary>
        public Database Database
        {
            get
            {
                return this.database;
            }

            set
            {
                this.database = value;
            }
        }

        /// <summary>
        /// Gets or Sets queries.
        /// </summary>
        public Dictionary<string, DataQueryManagerItem> Queries
        {
            get
            {
                return this.queries;
            }

            set
            {
                this.queries = value;
            }
        }

        /// <summary>
        /// Gets or Sets query placeholders.
        /// </summary>
        public Dictionary<string, string> QueryPlaceholders {
            get
            {
                return this.queryPlaceholders;
            }

            set
            {
                this.queryPlaceholders = value;
            }
        }

        // methods

        /// <summary>
        /// Returns a DataQuery instance which is requested by the query key
        /// </summary>
        /// <param name="queryKey">Query key</param>
        /// <param name="replacements">Replacements</param>
        /// <returns>DataQuery instance</returns>
        public virtual DataQuery GetQuery(string queryKey, Dictionary<string, string> replacements = null)
        {
            if (this.OnQueryGet != null)
            {
                this.OnQueryGet(this, EventArgs.Empty);
            }

            DataQueryManagerItem query = this.Queries[queryKey];

            DataQuery dataQuery = this.Database.NewQuery()
                                    .SetSqlString(query.SqlCommand)
                                    .AddPlaceholders(this.QueryPlaceholders);

            if (replacements != null)
            {
                dataQuery.AddPlaceholders(replacements);
            }

            return dataQuery;
        }
    }
}
