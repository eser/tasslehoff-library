// -----------------------------------------------------------------------
// <copyright file="CustomDataSource.cs" company="-">
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
    using System.Collections.Generic;
    using System.Web.UI;

    /// <summary>
    /// CustomDataSource class.
    /// </summary>
    public class CustomDataSource : IDataSource
    {
        // events

        /// <summary>
        /// On data source has changed.
        /// </summary>
        public event EventHandler DataSourceChanged;

        // fields

        /// <summary>
        /// Names of data source views.
        /// </summary>
        private ArrayList dataSourceViewNames;

        /// <summary>
        /// Object instances for data source views.
        /// </summary>
        private IDictionary<string, DataSourceView> dataSourceViews;

        /// <summary>
        /// Data query object for select queries.
        /// </summary>
        private DataQuery selectQuery = null;

        /// <summary>
        /// Data query object for paged select queries.
        /// </summary>
        private DataQuery selectPagedQuery = null;

        /// <summary>
        /// Data query object for select count queries.
        /// </summary>
        private DataQuery selectCountQuery = null;

        /// <summary>
        /// Data query object for insert queries.
        /// </summary>
        private DataQuery insertQuery = null;

        /// <summary>
        /// Data query object for update queries.
        /// </summary>
        private DataQuery updateQuery = null;

        /// <summary>
        /// Data query object for delete queries.
        /// </summary>
        private DataQuery deleteQuery = null;

        /// <summary>
        /// Primary key field for the data source.
        /// </summary>
        private string primaryKeyField = null;

        // constructors

        /// <summary>
        /// Initializes a new instance of the CustomDataSource class.
        /// </summary>
        public CustomDataSource()
        {
            this.dataSourceViewNames = new ArrayList() {
                ""
            };

            this.dataSourceViews = new Dictionary<string, DataSourceView>();
        }

        // attributes

        /// <summary>
        /// Gets or Sets the data query object for select queries.
        /// </summary>
        public DataQuery SelectQuery
        {
            get
            {
                return this.selectQuery;
            }
            set
            {
                this.selectQuery = value;
            }
        }

        /// <summary>
        /// Gets or Sets the data query object for paged select queries.
        /// </summary>
        public DataQuery SelectPagedQuery
        {
            get
            {
                return this.selectPagedQuery;
            }
            set
            {
                this.selectPagedQuery = value;
            }
        }

        /// <summary>
        /// Gets or Sets the data query object for select count queries.
        /// </summary>
        public DataQuery SelectCountQuery
        {
            get
            {
                return this.selectCountQuery;
            }
            set
            {
                this.selectCountQuery = value;
            }
        }

        /// <summary>
        /// Gets or Sets the data query object for insert queries.
        /// </summary>
        public DataQuery InsertQuery
        {
            get
            {
                return this.insertQuery;
            }
            set
            {
                this.insertQuery = value;
            }
        }

        /// <summary>
        /// Gets or Sets the data query object for update queries.
        /// </summary>
        public DataQuery UpdateQuery
        {
            get
            {
                return this.updateQuery;
            }
            set
            {
                this.updateQuery = value;
            }

        }

        /// <summary>
        /// Gets or Sets the data query object for delete queries.
        /// </summary>
        public DataQuery DeleteQuery
        {
            get
            {
                return this.deleteQuery;
            }
            set
            {
                this.deleteQuery = value;
            }
        }

        /// <summary>
        /// Gets or Sets the primary key field for the data source.
        /// </summary>
        public string PrimaryKeyField
        {
            get
            {
                return this.primaryKeyField;
            }
            set
            {
                this.primaryKeyField = value;
            }
        }

        // methods

        /// <summary>
        /// Gets specified data source view.
        /// </summary>
        /// <param name="viewName">Name of the view</param>
        /// <returns>Data source view instance</returns>
        public DataSourceView GetView(string viewName)
        {
            if (!this.dataSourceViewNames.Contains(viewName))
            {
                throw new ArgumentException("An invalid view was requested", "viewName");
            }

            if (this.dataSourceViews.ContainsKey(viewName))
            {
                return this.dataSourceViews[viewName];
            }

            DataSourceView dataSourceView = new CustomDataSourceView(this, viewName);
            this.dataSourceViews.Add(viewName, dataSourceView);

            return dataSourceView;
        }

        /// <summary>
        /// Gets all data source view names available.
        /// </summary>
        /// <returns>List of data source view names</returns>
        public ICollection GetViewNames()
        {
            return this.dataSourceViewNames;
        }
    }
}
