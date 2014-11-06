﻿// -----------------------------------------------------------------------
// <copyright file="CustomDataSourceExecuteSelectEventArgs.cs" company="-">
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
    using System.Web.UI;

    /// <summary>
    /// CustomDataSourceExecuteSelectEventArgs class.
    /// </summary>
    public class CustomDataSourceExecuteSelectEventArgs : EventArgs
    {
        // fields

        /// <summary>
        /// The arguments
        /// </summary>
        private readonly DataSourceSelectArguments arguments;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomDataSourceExecuteSelectEventArgs"/> class.
        /// </summary>
        /// <param name="arguments">The arguments</param>
        public CustomDataSourceExecuteSelectEventArgs(DataSourceSelectArguments arguments)
            : base()
        {
            this.arguments = arguments;
        }

        // properties

        /// <summary>
        /// Gets the arguments.
        /// </summary>
        public DataSourceSelectArguments Arguments
        {
            get
            {
                return this.arguments;
            }
        }
    }
}