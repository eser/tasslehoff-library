// -----------------------------------------------------------------------
// <copyright file="IDataEntityMap.cs" company="-">
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

namespace Tasslehoff.Library.DataEntities
{
    using System.Collections.Generic;
    using System.Data;
    using Tasslehoff.Library.DataStructures.Collections;

    /// <summary>
    /// IDataEntityMap interface
    /// </summary>
    public interface IDataEntityMap
    {
        /// <summary>
        /// Serializes the item.
        /// </summary>
        /// <param name="instance">The instance</param>
        /// <param name="convertNullsToDBNull">if set to <c>true</c> [convert nulls to DB null]</param>
        /// <returns>
        /// Serialized data.
        /// </returns>
        IDictionary<string, object> Serialize(IDataEntity instance, bool convertNullsToDBNull = false);

        /// <summary>
        /// Deserializes the item.
        /// </summary>
        /// <param name="dictionary">The dictionary</param>
        /// <typeparam name="T2">IDataEntity implementation</typeparam>
        /// <returns>Deserialized class</returns>
        T2 Deserialize<T2>(IDictionary<string, object> dictionary) where T2 : IDataEntity, new();

        /// <summary>
        /// Deserializes the item.
        /// </summary>
        /// <param name="record">The record</param>
        /// <typeparam name="T2">IDataEntity implementation</typeparam>
        /// <returns>
        /// Deserialized class
        /// </returns>
        T2 Deserialize<T2>(IDataRecord record) where T2 : IDataEntity, new();

        /// <summary>
        /// Deserializes the specified reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <typeparam name="T2">IDataEntity implementation</typeparam>
        /// <returns>Deserialized class</returns>
        T2 Deserialize<T2>(IDataReader reader) where T2 : IDataEntity, new();

        /// <summary>
        /// Deserializes to enumerable.
        /// </summary>
        /// <param name="reader">The reader</param>
        /// <typeparam name="T2">IDataEntity implementation</typeparam>
        /// <returns>Set of deserialized classes</returns>
        IEnumerable<T2> DeserializeToEnumerable<T2>(IDataReader reader) where T2 : IDataEntity, new();

        /// <summary>
        /// Deserializes to collection.
        /// </summary>
        /// <param name="reader">The reader</param>
        /// <typeparam name="T2">IDataEntity implementation</typeparam>
        /// <returns>Set of deserialized classes</returns>
        IEnumerable<T2> DeserializeToCollection<T2>(IDataReader reader) where T2 : IDataEntity, new();

        /// <summary>
        /// Deserializes to dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the key</typeparam>
        /// <param name="key">The key</param>
        /// <param name="reader">The reader</param>
        /// <typeparam name="T2">IDataEntity implementation</typeparam>
        /// <returns>Set of deserialized classes</returns>
        IDictionary<TKey, T2> DeserializeToDictionary<TKey, T2>(string key, IDataReader reader) where T2 : IDataEntity, new();

        /// <summary>
        /// Deserializes to base dictionary.
        /// </summary>
        /// <typeparam name="TKey1">The type of the key1.</typeparam>
        /// <typeparam name="TKey2">The type of the key2.</typeparam>
        /// <param name="key1">The key1.</param>
        /// <param name="key2">The key2.</param>
        /// <param name="reader">The reader.</param>
        /// <typeparam name="T2">IDataEntity implementation</typeparam>
        /// <returns>Dictionary object</returns>
        DictionaryBase<TKey1, TKey2, T2> DeserializeToBaseDictionary<TKey1, TKey2, T2>(string key1, string key2, IDataReader reader) where T2 : IDataEntity, new();
    }
}
