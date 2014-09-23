// -----------------------------------------------------------------------
// <copyright file="TreeNode.cs" company="-">
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

namespace Tasslehoff.Library.Objects
{
    using System.Collections.Generic;

    /// <summary>
    /// A node in tree data structure
    /// </summary>
    public class TreeNode
    {
        // fields

        /// <summary>
        /// Name
        /// </summary>
        private string name;

        /// <summary>
        /// Children
        /// </summary>
        private List<TreeNode> children;

        // attributes

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        /// <summary>
        /// Gets or sets the children
        /// </summary>
        public List<TreeNode> Children
        {
            get
            {
                return this.children;
            }
            set
            {
                this.children = value;
            }
        }
    }
}
