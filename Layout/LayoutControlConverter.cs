// -----------------------------------------------------------------------
// <copyright file="LayoutControlConverter.cs" company="-">
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
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// LayoutControlConverter class.
    /// </summary>
    public class LayoutControlConverter : CustomCreationConverter<ILayoutControl>
    {
        // fields

        private readonly LayoutControlRegistry registry;

        // constructors

        public LayoutControlConverter(LayoutControlRegistry registry)
            : base()
        {
            this.registry = registry;
        }

        // methods

        public override ILayoutControl Create(Type objectType)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Load JObject from stream
            JObject jObject = JObject.Load(reader);

            // Get type from JObject
            string type = (string)jObject.Property("Type");

            // Create target object based on JObject
            ILayoutControl target = this.registry.Create(type);

            // Populate the object properties
            serializer.Populate(jObject.CreateReader(), target);

            return target;
        }
    }
}
