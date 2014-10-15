// -----------------------------------------------------------------------
// <copyright file="ControlRegistry.cs" company="-">
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

    /// <summary>
    /// ControlRegistry class.
    /// </summary>
    [ComVisible(false)]
    public class ControlRegistry : DictionaryBase<string, Type>
    {
        // fields

        /// <summary>
        /// Singleton instance.
        /// </summary>
        private static ControlRegistry instance = null;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlRegistry"/> class.
        /// </summary>
        public ControlRegistry()
        {
            if (ControlRegistry.instance == null)
            {
                ControlRegistry.instance = this;
            }
        }

        // attributes

        /// <summary>
        /// Gets or Sets singleton instance.
        /// </summary>
        public static ControlRegistry Instance
        {
            get
            {
                return ControlRegistry.instance;
            }
            set
            {
                ControlRegistry.instance = value;
            }
        }

        // methods

        /// <summary>
        /// Registers a data entity class.
        /// </summary>
        /// <typeparam name="T">IDataEntity implementation.</typeparam>
        public void Register<T>() where T : IControl, new()
        {
            Type type = typeof(T);
            this.Add(type.Name, type);
        }

        /// <summary>
        /// Creates a new instance of related type.
        /// </summary>
        /// <param name="key">Key of the element</param>
        /// <returns>Created instance</returns>
        public object Create(string key)
        {
            Type type = this[key];

            return Activator.CreateInstance(type);
        }
    }
}
