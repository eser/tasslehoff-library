// -----------------------------------------------------------------------
// <copyright file="IControl.cs" company="-">
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

namespace Tasslehoff.Library
{
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;
    using Tasslehoff.Library.Text;

    /// <summary>
    /// IControl interface.
    /// </summary>
    public interface IControl : IDisposable
    {
        // properties

        /// <summary>
        /// Gets or sets child objects
        /// </summary>
        /// <value>
        /// Child objects
        /// </value>
        List<IControl> Children { get; }

        /// <summary>
        /// Gets or sets id
        /// </summary>
        /// <value>
        /// Id
        /// </value>
        string Id { get; }

        /// <summary>
        /// Gets or sets class
        /// </summary>
        /// <value>
        /// CssClass
        /// </value>
        string CssClass { get; }

        /// <summary>
        /// Gets or sets span
        /// </summary>
        /// <value>
        /// Span
        /// </value>
        int Span { get; }

        /// <summary>
        /// Gets or sets offset
        /// </summary>
        /// <value>
        /// Offset
        /// </value>
        int Offset { get; }

        // methods

        /// <summary>
        /// Creates web control
        /// </summary>
        /// <returns>Web control</returns>
        WebControl CreateWebControl();

        /// <summary>
        /// Serializes control into json
        /// </summary>
        /// <param name="jsonOutputWriter">Json Output Writer</param>
        void Export(JsonOutputWriter jsonOutputWriter);
    }
}
