// -----------------------------------------------------------------------
// <copyright file="ILayoutControl.cs" company="-">
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

namespace Tasslehoff.Library.Layout
{
    using System;
    using System.Collections.Generic;
    using Tasslehoff.Library.Text;
    using WebUI = System.Web.UI;

    /// <summary>
    /// ILayoutControl interface.
    /// </summary>
    public interface ILayoutControl : IDisposable, ICloneable
    {
        // properties

        /// <summary>
        /// Gets or sets tree id
        /// </summary>
        /// <value>
        /// Tree Id
        /// </value>
        Guid TreeId { get; set; }

        /// <summary>
        /// Gets or sets parent tree id
        /// </summary>
        /// <value>
        /// Parent Tree Id
        /// </value>
        Guid ParentTreeId { get; set;  }

        /// <summary>
        /// Gets type
        /// </summary>
        /// <value>
        /// Type
        /// </value>
        string Type { get; }

        /// <summary>
        /// Gets description
        /// </summary>
        /// <value>
        /// Description
        /// </value>
        string Description { get; }

        /// <summary>
        /// Gets or sets child objects
        /// </summary>
        /// <value>
        /// Child objects
        /// </value>
        List<ILayoutControl> Children { get; }

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

        /// <summary>
        /// Gets or sets webcontrol
        /// </summary>
        /// <value>
        /// Webcontrol
        /// </value>
        WebUI.Control WebControl { get; }

        // methods

        /// <summary>
        /// Creates web control
        /// </summary>
        void CreateWebControl();

        /// <summary>
        /// Set parameters of the control and its children
        /// </summary>
        /// <param name="parameters">Parameters</param>
        void SetParameters(Dictionary<string, object> parameters);

        /// <summary>
        /// Sets some ids to produce a tree
        /// </summary>
        /// <param name="isRoot">Whether this node is root or not</param>
        void MakeTree(bool isRoot = false);

        /// <summary>
        /// Flattens tree into one list
        /// </summary>
        /// <returns>Generated list</returns>
        List<ILayoutControl> FlattenTree();

        /// <summary>
        /// Serializes control into json
        /// </summary>
        /// <param name="jsonOutputWriter">Json Output Writer</param>
        void Export(MultiFormatOutputWriter jsonOutputWriter);

        /// <summary>
        /// Gets editable properties
        /// </summary>
        /// <returns>List of properties</returns>
        List<string> GetEditProperties();
    }
}
