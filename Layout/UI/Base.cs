// -----------------------------------------------------------------------
// <copyright file="Base.cs" company="-">
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

namespace Tasslehoff.Library.Layout.UI
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using Tasslehoff.Library.Text;

    /// <summary>
    /// Container class.
    /// </summary>
    [Serializable]
    [DataContract]
    public abstract class Base : LayoutControl
    {
        // fields

        /// <summary>
        /// Tag name
        /// </summary>
        [DataMember(Name = "TagName")]
        private string tagName = "div";

        // properties
        
        /// <summary>
        /// Gets or sets tag name
        /// </summary>
        /// <value>
        /// Tag name
        /// </value>
        [IgnoreDataMember]
        public virtual string TagName
        {
            get
            {
                return this.tagName;
            }
            set
            {
                this.tagName = value;
            }
        }
        
        // methods

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
        protected virtual void AddWebControlAttributes(Control createdControl, AttributeCollection attributes)
        {
            if (!string.IsNullOrEmpty(this.Id))
            {
                createdControl.ID = this.Id;
            }

            if (this.StaticClientId)
            {
                createdControl.ClientIDMode = ClientIDMode.Static;
            }

            if (attributes != null)
            {
                string classNames = this.GetWebControlClassNames();
                if (!string.IsNullOrEmpty(classNames))
                {
                    attributes["class"] = classNames;
                }
            }
        }

        /// <summary>
        /// Adds children web elements into new created control
        /// </summary>
        /// <param name="createdControl">The created control</param>
        protected virtual void AddWebControlChildren(Control createdControl)
        {
            foreach (ILayoutControl control in this.Children)
            {
                control.CreateWebControl();

                if (control.WebControl != null)
                {
                    createdControl.Controls.Add(control.WebControl as Control);
                }
            }
        }

        /// <summary>
        /// Creates web control
        /// </summary>
        public override void CreateWebControl()
        {
            HtmlGenericControl element = new HtmlGenericControl(this.TagName);

            this.AddWebControlAttributes(element, element.Attributes);
            this.AddWebControlChildren(element);

            this.WebControl = element;
        }

        /// <summary>
        /// Occurs when [export].
        /// </summary>
        /// <param name="jsonOutputWriter">Json Output Writer</param>
        public override void OnExport(MultiFormatOutputWriter jsonOutputWriter)
        {
            if (this.TagName != "div")
            {
                jsonOutputWriter.WriteProperty("TagName", this.TagName);
            }
        }

        /// <summary>
        /// Occurs when [export].
        /// </summary>
        /// <param name="jsonOutputWriter">Json Output Writer</param>
        public override void OnGetEditProperties(Dictionary<string, string> properties)
        {
            properties.Add("TagName", "Tag Name");
        }
    }
}
