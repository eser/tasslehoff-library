﻿// -----------------------------------------------------------------------
// <copyright file="TreeNode{T}.cs" company="-">
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
    using System;
    using System.Runtime.Serialization;
    using System.Security.Permissions;
    using Tasslehoff.Library.Collections;

    /// <summary>
    /// A node in tree data structure
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    [DataContract, Serializable]
    public class TreeNode<T> : IComparable, ISerializable
    {
        // fields

        /// <summary>
        /// Value
        /// </summary>
        [DataMember]
        private T value;

        /// <summary>
        /// SortIndex
        /// </summary>
        [DataMember]
        private int sortIndex;

        /// <summary>
        /// Children
        /// </summary>
        [DataMember]
        private SortedList<TreeNode<T>> children;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeNode{T}"/> class.
        /// </summary>
        public TreeNode()
        {
            this.children = new SortedList<TreeNode<T>>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeNode{T}"/> class.
        /// </summary>
        /// <param name="value">The value</param>
        public TreeNode(T value)
        {
            this.value = value;
            this.children = new SortedList<TreeNode<T>>();
        }

        /// <summary>
        /// Constructor for serialization interface
        /// </summary>
        /// <param name="info">info</param>
        /// <param name="context">context</param>
        protected TreeNode(SerializationInfo info, StreamingContext context)
        {
            this.value = (T)info.GetValue("value", typeof(T));
            this.sortIndex = info.GetInt32("sortIndex");
            this.children = (SortedList<TreeNode<T>>)info.GetValue("children", typeof(SortedList<TreeNode<T>>));
        }

        // attributes

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        [IgnoreDataMember]
        public T Value {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }

        /// <summary>
        /// Gets or sets the sort index
        /// </summary>
        [IgnoreDataMember]
        public int SortIndex
        {
            get
            {
                return this.sortIndex;
            }
            set
            {
                this.sortIndex = value;
            }
        }

        /// <summary>
        /// Gets or sets the children
        /// </summary>
        [IgnoreDataMember]
        public SortedList<TreeNode<T>> Children
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

        // methods

        /// <summary>
        /// Adds a child to node.
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="sortIndex">The sort index</param>
        /// <returns>Child object created</returns>
        public TreeNode<T> AddChild(T value, int? sortIndex = null)
        {
            TreeNode<T> node = new TreeNode<T>(value);
            if (sortIndex.HasValue)
            {
                node.sortIndex = sortIndex.Value;
            }

            this.children.Add(node);

            return node;
        }

        /// <summary>
        /// Does a comparision between two TreeNode instances
        /// </summary>
        /// <param name="obj">Other object to be compaired with</param>
        /// <returns>The result</returns>
        public int CompareTo(object obj)
        {
            TreeNode<T> other = obj as TreeNode<T>;
            if (other == null)
            {
                return 1;
            }

            return this.sortIndex.CompareTo(other.sortIndex);
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        protected void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("value", this.value);
            info.AddValue("sortIndex", this.sortIndex);
            info.AddValue("children", this.children);
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            this.GetObjectData(info, context);
        }
    }
}
