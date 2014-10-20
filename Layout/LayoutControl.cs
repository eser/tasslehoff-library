// -----------------------------------------------------------------------
// <copyright file="LayoutControl.cs" company="-">
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
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;
    using Tasslehoff.Library.Text;
    using WebUI = System.Web.UI;

    /// <summary>
    /// LayoutControl class.
    /// </summary>
    [Serializable]
    [DataContract]
    public abstract class LayoutControl : ILayoutControl
    {
        // fields

        /// <summary>
        /// Id
        /// </summary>
        [NonSerialized]
        [IgnoreDataMember]
        private readonly string type;

        /// <summary>
        /// Parameters
        /// </summary>
        [NonSerialized]
        [IgnoreDataMember]
        private Dictionary<string, object> parameters;

        /// <summary>
        /// Child objects
        /// </summary>
        [DataMember(Name = "Children")]
        private List<ILayoutControl> children;

        /// <summary>
        /// Id
        /// </summary>
        [DataMember(Name = "Id")]
        private string id;

        /// <summary>
        /// Css Class
        /// </summary>
        [DataMember(Name = "CssClass")]
        private string cssClass;

        /// <summary>
        /// Span
        /// </summary>
        [DataMember(Name = "Span")]
        private int span;

        /// <summary>
        /// Offset
        /// </summary>
        [DataMember(Name = "Offset")]
        private int offset;

        /// <summary>
        /// The webcontrol
        /// </summary>
        [IgnoreDataMember]
        private WebUI.Control webControl;

        /// <summary>
        /// The disposed
        /// </summary>
        [DataMember(Name = "Disposed")]
        private bool disposed;
        
        // constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutControl"/> class.
        /// </summary>
        public LayoutControl()
        {
            this.type = this.GetType().Name;
            this.children = new List<ILayoutControl>();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="LayoutControl"/> class.
        /// </summary>
        ~LayoutControl()
        {
            this.Dispose(false);
        }

        // properties

        /// <summary>
        /// Gets or sets type
        /// </summary>
        /// <value>
        /// Type
        /// </value>
        [IgnoreDataMember]
        public virtual string Type
        {
            get
            {
                return this.type;
            }
        }

        /// <summary>
        /// Gets or sets parameters
        /// </summary>
        /// <value>
        /// Parameters
        /// </value>
        [IgnoreDataMember]
        public virtual Dictionary<string, object> Parameters
        {
            get
            {
                return this.parameters;
            }
            set
            {
                this.parameters = value;
            }
        }

        /// <summary>
        /// Gets or sets child objects
        /// </summary>
        /// <value>
        /// Child objects
        /// </value>
        [IgnoreDataMember]
        public virtual List<ILayoutControl> Children
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
        /// Gets or sets webcontrol
        /// </summary>
        /// <value>
        /// Webcontrol
        /// </value>
        [IgnoreDataMember]
        public WebUI.Control WebControl
        {
            get
            {
                return this.webControl;
            }

            set
            {
                this.webControl = value;
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
        public abstract void CreateWebControl();

        /// <summary>
        /// Constructs class names for created element
        /// </summary>
        /// <returns>Class names</returns>
        protected virtual string GetWebControlClassNames()
        {
            string classNames = this.CssClass ?? string.Empty;

            if (this.Span != 0)
            {
                if (classNames.Length > 0)
                {
                    classNames += " ";
                }

                classNames += "col-xs-" + this.Span;
            }

            if (this.Offset != 0)
            {
                if (classNames.Length > 0)
                {
                    classNames += " ";
                }

                classNames += "col-xs-offset-" + this.Offset;
            }

            return classNames;
        }

        /// <summary>
        /// Assigns attributes of the new created control
        /// </summary>
        /// <param name="createdControl">The created control</param>
        /// <param name="attributes">The attributes property of created control</param>
        protected virtual void AddWebControlAttributes(WebUI.Control control, WebUI.AttributeCollection attributes)
        {
            control.ID = this.Id;
            // attributes["id"] = this.Id;

            string classNames = this.GetWebControlClassNames();
            if (!string.IsNullOrEmpty(classNames))
            {
                attributes["class"] = classNames;
            }
        }

        /// <summary>
        /// Adds children web elements into new created control
        /// </summary>
        /// <param name="createdControl">The created control</param>
        protected virtual void AddWebControlChildren(WebUI.Control createdControl)
        {
            foreach (ILayoutControl control in this.Children)
            {
                control.CreateWebControl();

                if (control.WebControl != null)
                {
                    createdControl.Controls.Add(control.WebControl);
                }
            }
        }

        /// <summary>
        /// Make the created control aware of layout system
        /// </summary>
        /// <param name="createdControl">The created control</param>
        protected virtual void MakeWebControlAwareOf(WebUI.Control createdControl)
        {
            if (createdControl is ILayoutAware)
            {
                (createdControl as ILayoutAware).LayoutAwareness(this);
            }
        }

        /// <summary>
        /// Updates the control and its children with given parameters
        /// </summary>
        /// <param name="parameters">Parameters</param>
        public void SetParameters(Dictionary<string, object> parameters)
        {
            this.Parameters = parameters;

            this.OnUpdate(parameters);

            foreach (ILayoutControl layoutControl in this.Children)
            {
                layoutControl.SetParameters(parameters);
            }
        }

        /// <summary>
        /// Occurs when [update].
        /// </summary>
        /// <param name="parameters">Parameters</param>
        public virtual void OnUpdate(Dictionary<string, object> parameters)
        {
        }

        /// <summary>
        /// Serializes control into json
        /// </summary>
        /// <param name="jsonOutputWriter">Json Output Writer</param>
        public virtual void Export(JsonOutputWriter jsonOutputWriter)
        {
            jsonOutputWriter.WriteStartObject();

            jsonOutputWriter.WriteProperty("Type", this.Type);

            if (!string.IsNullOrEmpty(this.Id))
            {
                jsonOutputWriter.WriteProperty("Id", this.Id);
            }

            jsonOutputWriter.WriteLine();

            if (!string.IsNullOrEmpty(this.CssClass))
            {
                jsonOutputWriter.WriteProperty("CssClass", this.CssClass);
            }

            if (this.Span != 0)
            {
                jsonOutputWriter.WriteProperty("Span", this.Span);
            }

            if (this.Offset != 0)
            {
                jsonOutputWriter.WriteProperty("Offset", this.Offset);
            }

            this.OnExport(jsonOutputWriter);

            if (this.Children.Count > 0)
            {
                jsonOutputWriter.WritePropertyName("Children");

                jsonOutputWriter.WriteStartArray();
                foreach (ILayoutControl control in this.Children)
                {
                    control.Export(jsonOutputWriter);
                }
                jsonOutputWriter.WriteEnd();
            }

            jsonOutputWriter.WriteEnd();
        }

        /// <summary>
        /// Occurs when [export].
        /// </summary>
        /// <param name="jsonOutputWriter">Json Output Writer</param>
        public abstract void OnExport(JsonOutputWriter jsonOutputWriter);

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Called when [dispose].
        /// </summary>
        /// <param name="releaseManagedResources"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources</param>
        protected virtual void OnDispose(bool releaseManagedResources)
        {
            // VariableUtils.CheckAndDispose<LoggerDelegate>(ref this.log);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="releaseManagedResources"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources</param>
        [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
        protected void Dispose(bool releaseManagedResources)
        {
            if (this.disposed)
            {
                return;
            }

            this.OnDispose(releaseManagedResources);

            this.disposed = true;
        }
    }
}
