// -----------------------------------------------------------------------
// <copyright file="DataEntityRegistry.cs" company="-">
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

namespace Tasslehoff.Library.DataEntities
{
    using System;
    using System.Runtime.InteropServices;
    using Tasslehoff.Library.Collections;
    using Tasslehoff.Library.DataAccess;

    /// <summary>
    /// DataEntityRegistry class.
    /// </summary>
    [ComVisible(false)]
    public class DataEntityRegistry : DictionaryBase<Type, IDataEntityMap>
    {
        // fields

        /// <summary>
        /// Singleton instance.
        /// </summary>
        private static DataEntityRegistry instance = null;

        /// <summary>
        /// Database instance.
        /// </summary>
        private Database database;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataEntityRegistry"/> class.
        /// </summary>
        public DataEntityRegistry()
        {
            if (DataEntityRegistry.instance == null)
            {
                DataEntityRegistry.instance = this;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataEntityRegistry"/> class.
        /// </summary>
        /// <param name="database"></param>
        public DataEntityRegistry(Database database) : this()
        {
            this.database = database;
        }

        // properties

        /// <summary>
        /// Gets or Sets singleton instance.
        /// </summary>
        public static DataEntityRegistry Instance {
            get
            {
                return DataEntityRegistry.instance;
            }
            set
            {
                DataEntityRegistry.instance = value;
            }
        }

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

        // methods

        /// <summary>
        /// Registers a data entity class.
        /// </summary>
        /// <typeparam name="T">IDataEntity implementation.</typeparam>
        public void Register<T>() where T : IDataEntity, new()
        {
            this.Add(typeof(T), new DataEntityMap<T>());
        }

        /// <summary>
        /// Gets the data entity class.
        /// </summary>
        /// <typeparam name="T">IDataEntity implementation.</typeparam>
        /// <returns>Data entity class.</returns>
        public DataEntityMap<T> Get<T>() where T : IDataEntity, new()
        {
            return this[typeof(T)] as DataEntityMap<T>;
        }

        /// <summary>
        /// Gets the data entity class.
        /// </summary>
        /// <typeparam name="T">IDataEntity implementation.</typeparam>
        /// <param name="dataQuery">DataQuery instance.</param>
        /// <returns>Data entity class.</returns>
        public T GetObjectFromDb<T>(DataQuery dataQuery) where T : IDataEntity, new()
        {
            return dataQuery.ExecuteDataEntity<T>();
        }

        /// <summary>
        /// Gets the data entity class.
        /// </summary>
        /// <typeparam name="T">IDataEntity implementation.</typeparam>
        /// <param name="database">Database instance.</param>
        /// <param name="sqlString">Sql string.</param>
        /// <returns>Data entity class.</returns>
        public T GetObjectFromDb<T>(Database database, string sqlString) where T : IDataEntity, new()
        {
            DataQuery dataQuery = database.NewQuery()
                .SetSqlString(sqlString);

            return this.GetObjectFromDb<T>(dataQuery);
        }

        /// <summary>
        /// Gets the data entity class.
        /// </summary>
        /// <typeparam name="T">IDataEntity implementation.</typeparam>
        /// <param name="sqlString">Sql string.</param>
        /// <returns>Data entity class.</returns>
        public T GetObjectFromDb<T>(string sqlString) where T : IDataEntity, new()
        {
            return this.GetObjectFromDb<T>(this.database, sqlString);
        }
    }
}
