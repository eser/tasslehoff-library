// -----------------------------------------------------------------------
// <copyright file="JsonOutputWriter.cs" company="-">
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

namespace Tasslehoff.Library.Text
{
    using System;
    using System.IO;
    using System.Text;
    using Newtonsoft.Json;
    using Tasslehoff.Library.Utils;

    /// <summary>
    /// GsmEncoding class.
    /// </summary>
    public class JsonOutputWriter : IDisposable
    {
        // fields

        /// <summary>
        /// StringBuilder
        /// </summary>
        private StringBuilder stringBuilder;

        /// <summary>
        /// TextWriter
        /// </summary>
        private TextWriter textWriter;

        /// <summary>
        /// Json TextWriter
        /// </summary>
        private JsonTextWriter jsonTextWriter;

        /// <summary>
        /// The disposed
        /// </summary>
        private bool disposed;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonOutputWriter"/> class.
        /// </summary>
        public JsonOutputWriter() : base()
        {
            this.stringBuilder = new StringBuilder();
            this.textWriter = new StringWriter(this.stringBuilder);
            this.jsonTextWriter = new JsonTextWriter(this.textWriter) {
                Formatting = Formatting.Indented
            };
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="JsonOutputWriter"/> class.
        /// </summary>
        ~JsonOutputWriter()
        {
            this.Dispose(false);
        }

        // properties

        /// <summary>
        /// Gets or Sets StringBuilder
        /// </summary>
        public StringBuilder StringBuilder
        {
            get
            {
                return this.stringBuilder;
            }

            set
            {
                this.stringBuilder = value;
            }
        }

        /// <summary>
        /// Gets or Sets TextWriter
        /// </summary>
        public TextWriter TextWriter
        {
            get
            {
                return this.textWriter;
            }

            set
            {
                this.textWriter = value;
            }
        }

        /// <summary>
        /// Gets or Sets Json TextWriter
        /// </summary>
        internal JsonTextWriter JsonTextWriter
        {
            get
            {
                return this.jsonTextWriter;
            }

            set
            {
                this.jsonTextWriter = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="JsonOutputWriter"/> is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        public bool Disposed
        {
            get
            {
                return this.disposed;
            }

            protected set
            {
                this.disposed = value;
            }
        }

        // methods

        /// <summary>
        /// Writes the end of current Json tag
        /// </summary>
        public void WriteEnd()
        {
            this.jsonTextWriter.WriteEnd();
        }

        /// <summary>
        /// Writes raw Json
        /// </summary>
        /// <param name="rawString">Raw Json</param>
        public void WriteRaw(string rawString)
        {
            this.jsonTextWriter.WriteRaw(rawString);
        }

        /// <summary>
        /// Writes the beginning of a Json array
        /// </summary>
        public void WriteStartArray()
        {
            this.jsonTextWriter.WriteStartArray();
        }

        /// <summary>
        /// Writes the beginning of a Json object
        /// </summary>
        public void WriteStartObject()
        {
            this.jsonTextWriter.WriteStartObject();
        }

        /// <summary>
        /// Writes a complete Json property
        /// </summary>
        /// <param name="name">Name of the property</param>
        /// <param name="value">Value of the property</param>
        /// <param name="escape">Whether escaping is enabled or not</param>
        public void WriteProperty(string name, object value, bool escape = true)
        {
            this.jsonTextWriter.WritePropertyName(name, escape);
            this.jsonTextWriter.WriteValue(value);
        }

        /// <summary>
        /// Writes the name of a Json property
        /// </summary>
        /// <param name="name">Name of the property</param>
        /// <param name="escape">Whether escaping is enabled or not</param>
        public void WritePropertyName(string name, bool escape = true)
        {
            this.jsonTextWriter.WritePropertyName(name, escape);
        }

        /// <summary>
        /// Writes the value of a Json property
        /// </summary>
        /// <param name="value">Value of the property</param>
        public void WriteValue(object value)
        {
            if (value is JsonOutputWriterPropertyValue)
            {
                switch ((JsonOutputWriterPropertyValue)value)
                {
                    case JsonOutputWriterPropertyValue.Undefined:
                        this.jsonTextWriter.WriteUndefined();
                        break;

                    case JsonOutputWriterPropertyValue.Null:
                        this.jsonTextWriter.WriteNull();
                        break;
                }

                return;
            }

            this.jsonTextWriter.WriteValue(value);
        }

        /// <summary>
        /// Writes the raw Json for a value field
        /// </summary>
        /// <param name="value">Value in raw Json</param>
        public void WriteValueRaw(string value)
        {
            this.jsonTextWriter.WriteRawValue(value);
        }

        /// <summary>
        /// Closes the current writer and releases any system resources
        /// </summary>
        public void Close()
        {
            this.jsonTextWriter.Close();
            this.textWriter.Close();
        }

        /// <summary>
        /// Converts the object instance to serialized Json string
        /// </summary>
        /// <returns>Json string</returns>
        public override string ToString()
        {
            return this.stringBuilder.ToString();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        void IDisposable.Dispose()
        {
            this.Dispose();
        }

        /// <summary>
        /// Called when [dispose].
        /// </summary>
        protected virtual void OnDispose()
        {
            this.Close();
            VariableUtils.CheckAndDispose<TextWriter>(ref this.textWriter);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="releaseManagedResources"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources</param>
        protected virtual void Dispose(bool releaseManagedResources)
        {
            if (this.disposed)
            {
                return;
            }

            if (releaseManagedResources)
            {
                this.OnDispose();
            }

            this.disposed = true;
        }
    }
}