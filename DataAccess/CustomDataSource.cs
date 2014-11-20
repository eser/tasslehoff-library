// -----------------------------------------------------------------------
// <copyright file="CustomDataSource.cs" company="-">
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
    using System.Collections.Generic;
    using System.Web.DynamicData;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    /// <summary>
    /// CustomDataSource class.
    /// 
    /// TODO filter implementation
    /// </summary>
    public class CustomDataSource : IDynamicDataSource
    {
        // constants

        private const string DefaultViewName = "";

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
        /// Field list for select queries.
        /// </summary>
        // private string[] selectFields = null;

        /// <summary>
        /// Data query object for paged select queries.
        /// </summary>
        private DataQuery selectPagedQuery = null;

        /// <summary>
        /// Field list for paged select queries.
        /// </summary>
        // private string[] selectPagedFields = null;

        /// <summary>
        /// Data query object for select count queries.
        /// </summary>
        private DataQuery selectCountQuery = null;

        /// <summary>
        /// Field list for select count queries.
        /// </summary>
        // private string[] selectCountFields = null;

        /// <summary>
        /// Data query object for insert queries.
        /// </summary>
        private DataQuery insertQuery = null;

        /// <summary>
        /// Field list for insert queries.
        /// </summary>
        private string[] insertFields = null;

        /// <summary>
        /// Data query object for update queries.
        /// </summary>
        private DataQuery updateQuery = null;

        /// <summary>
        /// Field list for update queries.
        /// </summary>
        private string[] updateFields = null;

        /// <summary>
        /// Data query object for delete queries.
        /// </summary>
        private DataQuery deleteQuery = null;

        /// <summary>
        /// Field list for delete queries.
        /// </summary>
        // private string[] deleteFields = null;

        /// <summary>
        /// Primary key field for the data source.
        /// </summary>
        private string primaryKeyField = null;

        /// <summary>
        /// Whether where clause is created dynamically or not.
        /// </summary>
        private bool autoGenerateWhereClause = false;

        /// <summary>
        /// Context type.
        /// </summary>
        private Type contextType = null;

        /// <summary>
        /// Enable insert.
        /// </summary>
        private bool enableInsert;

        /// <summary>
        /// Enable update.
        /// </summary>
        private bool enableUpdate;

        /// <summary>
        /// Enable delete.
        /// </summary>
        private bool enableDelete;

        /// <summary>
        /// Entity set name.
        /// </summary>
        private string entitySetName;

        /// <summary>
        /// Where statement.
        /// </summary>
        private string where;

        /// <summary>
        /// Where parameters.
        /// </summary>
        private ParameterCollection whereParameters;

        /// <summary>
        /// Determines whether paged query is used or not.
        /// </summary>
        private bool usePagedQuery;

        // constructors

        /// <summary>
        /// Initializes a new instance of the CustomDataSource class.
        /// </summary>
        public CustomDataSource()
        {
            this.dataSourceViewNames = new ArrayList() {
                CustomDataSource.DefaultViewName
            };

            this.dataSourceViews = new Dictionary<string, DataSourceView>();
        }

        // events

        /// <summary>
        ///  Occurs when a data source control has changed in some way that affects data-bound controls.
        /// </summary>
#pragma warning disable 0067
        public event EventHandler DataSourceChanged;
#pragma warning restore 0067

        /// <summary>
        /// Occurs when a data field fails to validate.
        /// </summary>
#pragma warning disable 0067
        public event EventHandler<DynamicValidatorEventArgs> Exception;
#pragma warning restore 0067

        /// <summary>
        /// Before select query.
        /// </summary>
        public event EventHandler<CustomDataSourceExecuteSelectEventArgs> OnExecuteSelect;
        
        /// <summary>
        /// Occurs when [on execute insert].
        /// </summary>
        public event EventHandler<CustomDataSourceExecuteInsertEventArgs> OnExecuteInsert;

        /// <summary>
        /// Occurs when [on execute update].
        /// </summary>
        public event EventHandler<CustomDataSourceExecuteUpdateEventArgs> OnExecuteUpdate;

        /// <summary>
        /// Occurs when [on execute delete].
        /// </summary>
        public event EventHandler<CustomDataSourceExecuteDeleteEventArgs> OnExecuteDelete;

        // properties

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
        /// Gets or Sets the field list for insert queries.
        /// </summary>
        public string[] InsertFields
        {
            get
            {
                return this.insertFields;
            }
            set
            {
                this.insertFields = value;
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
        /// Gets or Sets the field list for update queries.
        /// </summary>
        public string[] UpdateFields
        {
            get
            {
                return this.updateFields;
            }
            set
            {
                this.updateFields = value;
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

        /// <summary>
        /// Gets or Sets whether where clause is created dynamically or not.
        /// </summary>
        public bool AutoGenerateWhereClause
        {
            get
            {
                return this.autoGenerateWhereClause;
            }
            set
            {
                this.autoGenerateWhereClause = value;
            }
        }

        /// <summary>
        /// Gets or Sets the context type.
        /// </summary>
        public Type ContextType
        {
            get
            {
                return this.contextType;
            }
            set
            {
                this.contextType = value;
            }
        }

        /// <summary>
        /// Gets or Sets the enable insert.
        /// </summary>
        public bool EnableInsert
        {
            get
            {
                return this.enableInsert;
            }
            set
            {
                this.enableInsert = value;
            }
        }

        /// <summary>
        /// Gets or Sets the enable update.
        /// </summary>
        public bool EnableUpdate
        {
            get
            {
                return this.enableUpdate;
            }
            set
            {
                this.enableUpdate = value;
            }
        }

        /// <summary>
        /// Gets or Sets the enable delete.
        /// </summary>
        public bool EnableDelete
        {
            get
            {
                return this.enableDelete;
            }
            set
            {
                this.enableDelete = value;
            }
        }

        /// <summary>
        /// Gets or Sets the entity set name.
        /// </summary>
        public string EntitySetName
        {
            get
            {
                return this.entitySetName;
            }
            set
            {
                this.entitySetName = value;
            }
        }

        /// <summary>
        /// Gets or Sets the where statement.
        /// </summary>
        public string Where
        {
            get
            {
                return this.where;
            }
            set
            {
                this.where = value;
            }
        }

        /// <summary>
        /// Gets or Sets the where parameters.
        /// </summary>
        public ParameterCollection WhereParameters
        {
            get
            {
                return this.whereParameters;
            }
            set
            {
                this.whereParameters = value;
            }
        }

        /// <summary>
        /// Gets or Sets whether paged query is used or not.
        /// </summary>
        public bool UsePagedQuery
        {
            get
            {
                return this.usePagedQuery;
            }
            set
            {
                this.usePagedQuery = value;
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

        /// <summary>
        /// Invokes ExecuteSelect event.
        /// </summary>
        /// <param name="e">The event arguments</param>
        internal void InvokeExecuteSelect(CustomDataSourceExecuteSelectEventArgs e)
        {
            if (this.OnExecuteSelect != null)
            {
                this.OnExecuteSelect(this, e);
            }
        }

        /// <summary>
        /// Invokes ExecuteInsert event.
        /// </summary>
        /// <param name="e">The event arguments</param>
        internal void InvokeExecuteInsert(CustomDataSourceExecuteInsertEventArgs e)
        {
            if (this.OnExecuteInsert != null)
            {
                this.OnExecuteInsert(this, e);
            }
        }

        /// <summary>
        /// Invokes ExecuteUpdate event.
        /// </summary>
        /// <param name="e">The event arguments</param>
        internal void InvokeExecuteUpdate(CustomDataSourceExecuteUpdateEventArgs e)
        {
            if (this.OnExecuteUpdate != null)
            {
                this.OnExecuteUpdate(this, e);
            }
        }

        /// <summary>
        /// Invokes ExecuteDelete event.
        /// </summary>
        /// <param name="e">The event arguments</param>
        internal void InvokeExecuteDelete(CustomDataSourceExecuteDeleteEventArgs e)
        {
            if (this.OnExecuteDelete != null)
            {
                this.OnExecuteDelete(this, e);
            }
        }
    }
}
