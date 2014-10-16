// -----------------------------------------------------------------------
// <copyright file="CustomDataSourceView.cs" company="-">
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
    using System.Data;
    using System.Runtime.InteropServices;
    using System.Web.UI;

    /// <summary>
    /// CustomDataSourceView class.
    /// </summary>
    [ComVisible(false)]
    public class CustomDataSourceView : DataSourceView
    {
        // fields

        /// <summary>
        /// Owner of this data source view object.
        /// </summary>
        private CustomDataSource owner;

        // constructors

        /// <summary>
        /// Initializes a new instance of the CustomDataSourceView class.
        /// </summary>
        /// <param name="owner">The data source control that the System.Web.UI.DataSourceView is associated with.</param>
        /// <param name="viewName">The name of the System.Web.UI.DataSourceView object.</param>
        public CustomDataSourceView(IDataSource owner, string viewName) : base(owner, viewName)
        {
            this.owner = owner as CustomDataSource;
        }

        // attributes

        /// <summary>
        /// Gets a value indicating whether the System.Web.UI.DataSourceView object associated with the current System.Web.UI.DataSourceControl object
        /// supports a sorted view on the underlying data source.
        /// </summary>
        public override bool CanSort
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the System.Web.UI.DataSourceView object associated with the current System.Web.UI.DataSourceControl object
        /// supports paging through the data retrieved by the System.Web.UI.DataSourceView.ExecuteSelect(System.Web.UI.DataSourceSelectArguments) method.
        /// </summary>
        public override bool CanPage
        {
            get
            {
                return (this.owner.SelectPagedQuery != null);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the System.Web.UI.DataSourceView object associated with the current System.Web.UI.DataSourceControl object
        /// supports retrieving the total number of data rows, instead of the data.
        /// </summary>
        public override bool CanRetrieveTotalRowCount
        {
            get
            {
                return (this.owner.SelectCountQuery != null);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the System.Web.UI.DataSourceView object associated with the current System.Web.UI.DataSourceControl object
        /// supports the current System.Web.UI.DataSourceControl object supports the System.Web.UI.DataSourceView.ExecuteInsert(System.Collections.IDictionary) operation.
        /// </summary>
        public override bool CanInsert
        {
            get
            {
                return (this.owner.InsertQuery != null);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the System.Web.UI.DataSourceView object associated with the current System.Web.UI.DataSourceControl object
        /// supports the System.Web.UI.DataSourceView.ExecuteUpdate(System.Collections.IDictionary,System.Collections.IDictionary,System.Collections.IDictionary) operation.
        /// </summary>
        public override bool CanUpdate
        {
            get
            {
                return (this.owner.UpdateQuery != null);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the System.Web.UI.DataSourceView object associated with the current System.Web.UI.DataSourceControl object
        /// supports the System.Web.UI.DataSourceView.ExecuteDelete(System.Collections.IDictionary,System.Collections.IDictionary) operation.
        /// </summary>
        public override bool CanDelete
        {
            get
            {
                return (this.owner.DeleteQuery != null);
            }
        }

        // methods

        /// <summary>
        /// Gets a list of data from the underlying data storage.
        /// </summary>
        /// <param name="arguments">A System.Web.UI.DataSourceSelectArguments that is used to request operations on the data beyond basic data retrieval.</param>
        /// <returns>An System.Collections.IEnumerable list of data from the underlying data storage.</returns>
        protected override IEnumerable ExecuteSelect(DataSourceSelectArguments arguments)
        {
            DataQuery targetSelectQuery;
            DataQuery targetSelectCountQuery;

            this.owner.InvokeBeforeSelectQuery(EventArgs.Empty);

            if (this.CanPage && this.owner.UsePagedQuery)
            {
                targetSelectQuery = this.owner.SelectPagedQuery;
                targetSelectCountQuery = this.owner.SelectCountQuery;

                targetSelectQuery.AddPlaceholders("RECORD_FROM", (arguments.StartRowIndex + 1).ToString());
                targetSelectQuery.AddPlaceholders("RECORD_TO", (arguments.StartRowIndex + arguments.MaximumRows).ToString());
            } else {
                targetSelectQuery = this.owner.SelectQuery;
                targetSelectCountQuery = this.owner.SelectCountQuery;
            }

            if (this.CanSort && !string.IsNullOrEmpty(arguments.SortExpression))
            {
                targetSelectQuery.AddPlaceholders("SORT_FIELDS", arguments.SortExpression);
            }
            else
            {
                targetSelectQuery.AddPlaceholders("SORT_FIELDS", this.owner.PrimaryKeyField);
            }

            if (!string.IsNullOrEmpty(this.owner.Where))
            {
                targetSelectQuery.AddPlaceholders("WHERE_STATEMENT", this.owner.Where);
                targetSelectCountQuery.AddPlaceholders("WHERE_STATEMENT", this.owner.Where);
            }
            else
            {
                targetSelectQuery.AddPlaceholders("WHERE_STATEMENT", "1=1");
                targetSelectCountQuery.AddPlaceholders("WHERE_STATEMENT", "1=1");
            }

            DataTable dataTable = targetSelectQuery.ExecuteDataTable(CommandBehavior.SingleResult);
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns[this.owner.PrimaryKeyField] };

            IEnumerable result = new DataView(dataTable);

            if (this.CanRetrieveTotalRowCount && arguments.RetrieveTotalRowCount)
            {
                arguments.TotalRowCount = (int)targetSelectCountQuery.ExecuteScalar();
            }

            return result;
        }

        /// <summary>
        /// Performs an insert operation on the list of data that the System.Web.UI.DataSourceView object represents.
        /// </summary>
        /// <param name="values">An System.Collections.IDictionary of name/value pairs used during an insert operation.</param>
        /// <returns>The number of items that were inserted into the underlying data storage.</returns>
        protected override int ExecuteInsert(IDictionary values)
        {
            if (!this.CanInsert)
            {
                return base.ExecuteInsert(values);
            }

            DataQuery targetQuery = this.owner.InsertQuery;

            List<string> queryPart1 = new List<string>();
            List<string> queryPart2 = new List<string>();
            foreach (DictionaryEntry entry in values)
            {
                string key = entry.Key as string;

                if (key == this.owner.PrimaryKeyField)
                {
                    continue;
                }

                queryPart1.Add(key);
                queryPart2.Add("@" + key);

                targetQuery.AddParameters(key, entry.Value);
            }

            targetQuery.AddPlaceholders("FIELDNAMES", string.Join(", ", queryPart1));
            targetQuery.AddPlaceholders("FIELDVALUES", string.Join(", ", queryPart2));

            return (int)targetQuery.ExecuteScalar();
        }

        /// <summary>
        /// Performs an update operation on the list of data that the System.Web.UI.DataSourceView object represents.
        /// </summary>
        /// <param name="keys">An System.Collections.IDictionary of object or row keys to be updated by the update operation.</param>
        /// <param name="values">An System.Collections.IDictionary of name/value pairs that represent data elements and their new values.</param>
        /// <param name="oldValues">An System.Collections.IDictionary of name/value pairs that represent data elements and their original values.</param>
        /// <returns>The number of items that were updated in the underlying data storage.</returns>
        protected override int ExecuteUpdate(IDictionary keys, IDictionary values, IDictionary oldValues)
        {
            if (!this.CanUpdate)
            {
                return base.ExecuteUpdate(keys, values, oldValues);
            }

            DataQuery targetQuery = this.owner.UpdateQuery
                .AddParameters("ID", keys[this.owner.PrimaryKeyField]);

            List<string> queryPart = new List<string>();
            foreach (DictionaryEntry entry in values)
            {
                string key = entry.Key as string;

                if (key == this.owner.PrimaryKeyField)
                {
                    continue;
                }

                queryPart.Add(string.Format("{0}=@{0}", key));

                targetQuery.AddParameters(key, entry.Value);
            }

            targetQuery.AddPlaceholders("FIELDS", string.Join(", ", queryPart));

            return targetQuery.ExecuteNonQuery();
        }

        /// <summary>
        /// Performs a delete operation on the list of data that the System.Web.UI.DataSourceView object represents.
        /// </summary>
        /// <param name="keys">An System.Collections.IDictionary of object or row keys to be deleted by the System.Web.UI.DataSourceView.ExecuteDelete(System.Collections.IDictionary,System.Collections.IDictionary) operation.</param>
        /// <param name="oldValues">An System.Collections.IDictionary of name/value pairs that represent data elements and their original values.</param>
        /// <returns>The number of items that were deleted from the underlying data storage.</returns>
        protected override int ExecuteDelete(IDictionary keys, IDictionary oldValues)
        {
            if (!this.CanDelete)
            {
                return base.ExecuteDelete(keys, oldValues);
            }

            DataQuery targetQuery = this.owner.DeleteQuery
                .AddParameters("ID", keys[this.owner.PrimaryKeyField]);

            return targetQuery.ExecuteNonQuery();
        }

        #region CRUD
        /*
        /// <summary>
        /// Gets a list of data asynchronously from the underlying data storage.
        /// </summary>
        /// <param name="arguments">A System.Web.UI.DataSourceSelectArguments that is used to request operations on the data beyond basic data retrieval.</param>
        /// <param name="callback">A System.Web.UI.DataSourceViewSelectCallback delegate that is used to notify a data-bound control when the asynchronous operation is complete.</param>
        public override void Select(DataSourceSelectArguments arguments, DataSourceViewSelectCallback callback)
        {
            base.Select(arguments, callback);
        }

        /// <summary>
        /// Performs an asynchronous insert operation on the list of data that the System.Web.UI.DataSourceView object represents.
        /// </summary>
        /// <param name="values">An System.Collections.IDictionary of name/value pairs used during an insert operation.</param>
        /// <param name="callback">A System.Web.UI.DataSourceViewOperationCallback delegate that is used to notify a data-bound control when the asynchronous operation is complete.</param>
        public override void Insert(IDictionary values, DataSourceViewOperationCallback callback)
        {
            base.Insert(values, callback);
        }

        /// <summary>
        /// Performs an asynchronous update operation on the list of data that the System.Web.UI.DataSourceView object represents.
        /// </summary>
        /// <param name="keys">An System.Collections.IDictionary of object or row keys to be updated by the update operation.</param>
        /// <param name="values">An System.Collections.IDictionary of name/value pairs that represent data elements and their new values.</param>
        /// <param name="oldValues">An System.Collections.IDictionary of name/value pairs that represent data elements and their original values.</param>
        /// <param name="callback">A System.Web.UI.DataSourceViewOperationCallback delegate that is used to notify a data-bound control when the asynchronous operation is complete.</param>
        public override void Update(IDictionary keys, IDictionary values, IDictionary oldValues, DataSourceViewOperationCallback callback)
        {
            base.Update(keys, values, oldValues, callback);
        }

        /// <summary>
        /// Performs an asynchronous delete operation on the list of data that the System.Web.UI.DataSourceView object represents.
        /// </summary>
        /// <param name="keys">An System.Collections.IDictionary of object or row keys to be deleted by the System.Web.UI.DataSourceView.ExecuteDelete(System.Collections.IDictionary,System.Collections.IDictionary) operation.</param>
        /// <param name="oldValues">An System.Collections.IDictionary of name/value pairs that represent data elements and their original values.</param>
        /// <param name="callback">A System.Web.UI.DataSourceViewOperationCallback delegate that is used to notify a data-bound control when the asynchronous operation is complete.</param>
        public override void Delete(IDictionary keys, IDictionary oldValues, DataSourceViewOperationCallback callback)
        {
            base.Delete(keys, oldValues, callback);
        }
        */
        #endregion

        #region Unnecessary
        /*
        /// <summary>
        /// Determines whether the specified command can be executed.
        /// </summary>
        /// <param name="commandName">The name of the command.</param>
        /// <returns>true if the command can be executed; otherwise, false.</returns>
        public override bool CanExecute(string commandName)
        {
            return base.CanExecute(commandName);
        }

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="commandName">The name of the command.</param>
        /// <param name="keys">A dictionary of object keys or row keys to act on.</param>
        /// <param name="values">A dictionary of name/value pairs that represent data elements and their values.</param>
        /// <returns></returns>
        protected override int ExecuteCommand(string commandName, IDictionary keys, IDictionary values)
        {
            return base.ExecuteCommand(commandName, keys, values);
        }

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="commandName">The name of the command.</param>
        /// <param name="keys">A dictionary of object keys or row keys to act on.</param>
        /// <param name="values">A dictionary of name/value pairs that represent data elements and their values.</param>
        /// <param name="callback">A System.Web.UI.DataSourceViewOperationCallback object.</param>
        /// <returns></returns>
        public override void ExecuteCommand(string commandName, IDictionary keys, IDictionary values, DataSourceViewOperationCallback callback)
        {
            base.ExecuteCommand(commandName, keys, values, callback);
        }

        /// <summary>
        /// Raises the System.Web.UI.DataSourceView.DataSourceViewChanged event.
        /// </summary>
        /// <param name="e">An System.EventArgs that contains event data.</param>
        protected override void OnDataSourceViewChanged(EventArgs e)
        {
            base.OnDataSourceViewChanged(e);
        }

        /// <summary>
        /// Called by the System.Web.UI.DataSourceSelectArguments.RaiseUnsupportedCapabilitiesError(System.Web.UI.DataSourceView) method to compare the capabilities
        /// requested for an System.Web.UI.DataSourceView.ExecuteSelect(System.Web.UI.DataSourceSelectArguments) operation against those that the view supports.
        /// </summary>
        /// <param name="capability">One of the System.Web.UI.DataSourceCapabilities values that is compared against the capabilities that the view supports.</param>
        protected override void RaiseUnsupportedCapabilityError(DataSourceCapabilities capability)
        {
            base.RaiseUnsupportedCapabilityError(capability);
        }
        */
        #endregion
    }
}
