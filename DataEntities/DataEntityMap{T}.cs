// -----------------------------------------------------------------------
// <copyright file="DataEntityMap{T}.cs" company="-">
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
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using Tasslehoff.Library.Collections;
    using Tasslehoff.Library.Utils;

    /// <summary>
    /// DataEntityMap class.
    /// </summary>
    /// <typeparam name="T">IDataEntity implementation</typeparam>
    [ComVisible(false)]
    public class DataEntityMap<T> : DictionaryBase<string, DataEntityFieldAttribute>, IDataEntityMap where T : IDataEntity, new()
    {
        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataEntityMap{T}"/> class.
        /// </summary>
        public DataEntityMap() : base()
        {
            Type type = typeof(T);

            MemberInfo[] members = type.GetMembers(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            foreach (MemberInfo member in members)
            {
                if (member.MemberType != MemberTypes.Field && member.MemberType != MemberTypes.Property)
                {
                    continue;
                }

                object[] attributes = member.GetCustomAttributes(typeof(DataEntityFieldAttribute), true);
                foreach (object attribute in attributes)
                {
                    DataEntityFieldAttribute fieldAttribute = (DataEntityFieldAttribute)attribute;

                    if (string.IsNullOrEmpty(fieldAttribute.FieldName))
                    {
                        fieldAttribute.FieldName = member.Name;
                    }

                    fieldAttribute.ClassMember = member;
                    fieldAttribute.Type = VariableUtils.GetMemberType(member);

                    this.Add(fieldAttribute.FieldName, fieldAttribute);
                }
            }
        }

        // methods

        /// <summary>
        /// Serializes the item.
        /// </summary>
        /// <param name="instance">The instance</param>
        /// <param name="convertNullsToDBNull">if set to <c>true</c> [convert nulls to DB null]</param>
        /// <returns>
        /// Serialized data.
        /// </returns>
        public IDictionary<string, object> Serialize(IDataEntity instance, bool convertNullsToDBNull = false)
        {
            IDictionary<string, object> dictionary = new Dictionary<string, object>();

            foreach (DataEntityFieldAttribute fieldAttribute in this.Values)
            {
                object value = VariableUtils.ReadMemberValue(fieldAttribute.ClassMember, instance);

                if (fieldAttribute.Serializer != null)
                {
                    value = FieldSerializers.Get(fieldAttribute.Serializer).Serializer(value);
                }

                if (convertNullsToDBNull && value == null)
                {
                    value = DBNull.Value;
                }

                dictionary.Add(fieldAttribute.FieldName, value);
            }

            instance.OnSerialize(ref dictionary);

            return dictionary;
        }

        /// <summary>
        /// Deserializes the item.
        /// </summary>
        /// <param name="dictionary">The dictionary</param>
        /// <typeparam name="T2">IDataEntity implementation</typeparam>
        /// <returns>Deserialized class</returns>
        public T2 Deserialize<T2>(IDictionary<string, object> dictionary) where T2 : IDataEntity, new()
        {
            T2 instance = new T2();

            instance.OnDeserialize(ref dictionary);

            foreach (KeyValuePair<string, object> pair in dictionary)
            {
                if (!this.ContainsKey(pair.Key))
                {
                    continue;
                }

                object fieldValue = pair.Value;
                if (Convert.IsDBNull(fieldValue))
                {
                    fieldValue = null;
                }

                DataEntityFieldAttribute attribute = this[pair.Key];

                if (attribute.Serializer != null)
                {
                    fieldValue = FieldSerializers.Get(attribute.Serializer).Deserializer(fieldValue);
                }

                VariableUtils.WriteMemberValue(attribute.ClassMember, instance, fieldValue);
            }

            return instance;
        }

        /// <summary>
        /// Deserializes the item.
        /// </summary>
        /// <param name="record">The record</param>
        /// <typeparam name="T2">IDataEntity implementation</typeparam>
        /// <returns>
        /// Deserialized class
        /// </returns>
        public T2 Deserialize<T2>(IDataRecord record) where T2 : IDataEntity, new()
        {
            IDictionary<string, object> dictionary = new Dictionary<string, object>();

            for (int i = 0; i < record.FieldCount; i++)
            {
                dictionary.Add(record.GetName(i), record.GetValue(i));
            }

            return this.Deserialize<T2>(dictionary);
        }

        /// <summary>
        /// Deserializes the specified reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <typeparam name="T2">IDataEntity implementation</typeparam>
        /// <returns>Deserialized class</returns>
        public T2 Deserialize<T2>(IDataReader reader) where T2 : IDataEntity, new()
        {
            if (reader.Read())
            {
                return this.Deserialize<T2>((IDataRecord)reader);
            }

            return default(T2);
        }

        /// <summary>
        /// Deserializes to enumerable.
        /// </summary>
        /// <param name="reader">The reader</param>
        /// <typeparam name="T2">IDataEntity implementation</typeparam>
        /// <returns>Set of deserialized classes</returns>
        public IEnumerable<T2> DeserializeToEnumerable<T2>(IDataReader reader) where T2 : IDataEntity, new()
        {
            while (reader.Read())
            {
                yield return this.Deserialize<T2>((IDataRecord)reader);
            }
        }

        /// <summary>
        /// Deserializes to collection.
        /// </summary>
        /// <param name="reader">The reader</param>
        /// <typeparam name="T2">IDataEntity implementation</typeparam>
        /// <returns>Set of deserialized classes</returns>
        public IEnumerable<T2> DeserializeToCollection<T2>(IDataReader reader) where T2 : IDataEntity, new()
        {
            ICollection<T2> collection = new Collection<T2>();

            while (reader.Read())
            {
                collection.Add(this.Deserialize<T2>((IDataRecord)reader));
            }

            return collection;
        }

        /// <summary>
        /// Deserializes to dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the key</typeparam>
        /// <param name="key">The key</param>
        /// <param name="reader">The reader</param>
        /// <typeparam name="T2">IDataEntity implementation</typeparam>
        /// <returns>Set of deserialized classes</returns>
        public IDictionary<TKey, T2> DeserializeToDictionary<TKey, T2>(string key, IDataReader reader) where T2 : IDataEntity, new()
        {
            IDictionary<TKey, T2> dictionary = new Dictionary<TKey, T2>();

            while (reader.Read())
            {
                dictionary.Add((TKey)reader[key], this.Deserialize<T2>((IDataRecord)reader));
            }

            return dictionary;
        }

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
        public DictionaryBase<TKey1, TKey2, T2> DeserializeToBaseDictionary<TKey1, TKey2, T2>(string key1, string key2, IDataReader reader) where T2 : IDataEntity, new()
        {
            DictionaryBase<TKey1, TKey2, T2> dictionary = new DictionaryBase<TKey1, TKey2, T2>();

            while (reader.Read())
            {
                dictionary.Add((TKey1)reader[key1], (TKey2)reader[key2], this.Deserialize<T2>((IDataRecord)reader));
            }

            return dictionary;
        }

        /// <summary>
        /// Deserializes the item.
        /// </summary>
        /// <param name="dictionary">The dictionary</param>
        /// <returns>Deserialized class</returns>
        public T Deserialize(IDictionary<string, object> dictionary)
        {
            return this.Deserialize<T>(dictionary);
        }

        /// <summary>
        /// Deserializes the item.
        /// </summary>
        /// <param name="record">The record</param>
        /// <returns>
        /// Deserialized class
        /// </returns>
        public T Deserialize(IDataRecord record)
        {
            return this.Deserialize<T>(record);
        }

        /// <summary>
        /// Deserializes the specified reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>Deserialized class</returns>
        public T Deserialize(IDataReader reader)
        {
            return this.Deserialize<T>(reader);
        }

        /// <summary>
        /// Deserializes to enumerable.
        /// </summary>
        /// <param name="reader">The reader</param>
        /// <returns>Set of deserialized classes</returns>
        public IEnumerable<T> DeserializeToEnumerable(IDataReader reader)
        {
            return this.DeserializeToEnumerable<T>(reader);
        }

        /// <summary>
        /// Deserializes to collection.
        /// </summary>
        /// <param name="reader">The reader</param>
        /// <returns>Set of deserialized classes</returns>
        public IEnumerable<T> DeserializeToCollection(IDataReader reader)
        {
            return this.DeserializeToCollection<T>(reader);
        }

        /// <summary>
        /// Deserializes to dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the key</typeparam>
        /// <param name="key">The key</param>
        /// <param name="reader">The reader</param>
        /// <returns>Set of deserialized classes</returns>
        public IDictionary<TKey, T> DeserializeToDictionary<TKey>(string key, IDataReader reader)
        {
            return this.DeserializeToDictionary<TKey>(key, reader);
        }

        /// <summary>
        /// Deserializes to base dictionary.
        /// </summary>
        /// <typeparam name="TKey1">The type of the key1.</typeparam>
        /// <typeparam name="TKey2">The type of the key2.</typeparam>
        /// <param name="key1">The key1.</param>
        /// <param name="key2">The key2.</param>
        /// <param name="reader">The reader.</param>
        /// <returns>Dictionary object</returns>
        public DictionaryBase<TKey1, TKey2, T> DeserializeToBaseDictionary<TKey1, TKey2>(string key1, string key2, IDataReader reader)
        {
            return this.DeserializeToBaseDictionary<TKey1, TKey2>(key1, key2, reader);
        }
    }
}
