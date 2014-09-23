// -----------------------------------------------------------------------
// <copyright file="Config.cs" company="-">
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

namespace Tasslehoff.Library.Config
{
    using System;
    using System.IO;
    using System.Reflection;
    using Newtonsoft.Json;

    /// <summary>
    /// Config class
    /// </summary>
    public abstract class Config
    {
        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Config"/> class.
        /// </summary>
        public Config()
        {
            this.Reset(true);
        }

        // methods

        /// <summary>
        /// Serializes a configuration object into a string
        /// </summary>
        /// <returns>Serialized config object</returns>
        public string Dump()
        {
            return JsonConvert.SerializeObject(this, ConfigSerializer.GetSerializerSettings());
        }

        /// <summary>
        /// Serializes a config object into a file
        /// </summary>
        /// <param name="path">The path serialized object will be written</param>
        public void SaveToFile(string path)
        {
            File.WriteAllText(path, this.Dump());
        }

        /// <summary>
        /// Resets the specified config object.
        /// </summary>
        /// <param name="isFirstInit">Whether it is called during initialization or not</param>
        public void Reset(bool isFirstInit = false)
        {
            Type type = this.GetType();

            foreach (PropertyInfo property in type.GetProperties())
            {
                object value = null;
                ConfigEntryAttribute[] attributes = (ConfigEntryAttribute[])property.GetCustomAttributes(typeof(ConfigEntryAttribute), true);

                if (attributes.Length > 0)
                {
                    if (!isFirstInit && attributes[0].SkipInReset)
                    {
                        continue;
                    }

                    value = attributes[0].DefaultValue;
                }

                property.SetValue(this, value, null);
            }
        }
    }
}
