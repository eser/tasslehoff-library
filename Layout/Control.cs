// -----------------------------------------------------------------------
// <copyright file="Control.cs" company="-">
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
    using System.Runtime.Serialization;
    using System.Web.UI.WebControls;

    /// <summary>
    /// Control class.
    /// </summary>
    [Serializable]
    [DataContract]
    public abstract class Control : IControl
    {
        // fields

        /// <summary>
        /// Child objects
        /// </summary>
        [DataMember]
        private List<IControl> children;

        /// <summary>
        /// Id
        /// </summary>
        [DataMember]
        private string id;

        /// <summary>
        /// Css Class
        /// </summary>
        [DataMember]
        private string cssClass;

        /// <summary>
        /// Span
        /// </summary>
        [DataMember]
        private int span;

        /// <summary>
        /// Offset
        /// </summary>
        [DataMember]
        private int offset;

        /// <summary>
        /// The disposed
        /// </summary>
        [DataMember]
        private bool disposed;

        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Control"/> class.
        /// </summary>
        public Control()
        {
            this.children = new List<IControl>();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="Control"/> class.
        /// </summary>
        ~Control()
        {
            this.Dispose(false);
        }

        // properties

        /// <summary>
        /// Gets or sets child objects
        /// </summary>
        /// <value>
        /// Child objects
        /// </value>
        [IgnoreDataMember]
        public virtual List<IControl> Children
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

        /// <summary>
        /// Gets or sets id
        /// </summary>
        /// <value>
        /// Id
        /// </value>
        [IgnoreDataMember]
        public virtual string Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }

        /// <summary>
        /// Gets or sets css class
        /// </summary>
        /// <value>
        /// Css class
        /// </value>
        [IgnoreDataMember]
        public virtual string CssClass
        {
            get
            {
                return this.cssClass;
            }
            set
            {
                this.cssClass = value;
            }
        }

        /// <summary>
        /// Gets or sets span
        /// </summary>
        /// <value>
        /// Span
        /// </value>
        [IgnoreDataMember]
        public virtual int Span
        {
            get
            {
                return this.span;
            }
            set
            {
                this.span = value;
            }
        }

        /// <summary>
        /// Gets or sets offset
        /// </summary>
        /// <value>
        /// Offset
        /// </value>
        [IgnoreDataMember]
        public virtual int Offset
        {
            get
            {
                return this.offset;
            }
            set
            {
                this.offset = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Service"/> is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        [IgnoreDataMember]
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
        /// Creates web control
        /// </summary>
        /// <returns>Web control</returns>
        public abstract WebControl CreateWebControl();

        public string Export(int indent = 0)
        {
            List<string> props = new List<string>();

            this.OnExport(props, indent);

            string indentStr = new string('\t', indent);
            string result = indentStr + "{";
            bool firstLoop = true;

            foreach (string prop in props)
            {
                if (firstLoop)
                {
                    firstLoop = false;
                }
                else
                {
                    result += ",";
                }

                result += Environment.NewLine + indentStr + "\t" + prop;
            }

            if (!firstLoop)
            {
                result += Environment.NewLine + indentStr;
            }

            result += "}";


            return result;
        }

        protected void OnExport(List<string> props, int indent)
        {
            if (this.span != 0)
            {
                props.Add(string.Format("span: {0}", this.span));
            }

            if (this.offset != 0)
            {
                props.Add(string.Format("offset: {0}", this.offset));
            }

            if (this.children.Count > 0)
            {
                string childrenText = "children: [";
                foreach (IControl control in this.children)
                {
                    childrenText += control.Export(indent + 1);
                }
                childrenText += "]";

                props.Add(childrenText);
            }
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
            // VariableUtils.CheckAndDispose<LoggerDelegate>(ref this.log);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                this.OnDispose();
            }

            this.disposed = true;
        }
    }
}
