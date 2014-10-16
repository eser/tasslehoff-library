// -----------------------------------------------------------------------
// <copyright file="Container.cs" company="-">
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
    using System.Runtime.Serialization;
    using System.Web.UI.HtmlControls;
    using WebUI = System.Web.UI;

    /// <summary>
    /// Container class.
    /// </summary>
    [Serializable]
    [DataContract]
    public class Html : Control
    {
        // fields

        /// <summary>
        /// Tag name
        /// </summary>
        [DataMember]
        private string tagName = "div";

        /// <summary>
        /// Inner content
        /// </summary>
        [DataMember]
        private string innerContent;

        /// <summary>
        /// Whether encode contents or not
        /// </summary>
        [DataMember]
        private bool encodeContents;

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

        /// <summary>
        /// Gets or sets inner content
        /// </summary>
        /// <value>
        /// Inner content
        /// </value>
        [IgnoreDataMember]
        public virtual string InnerContent
        {
            get
            {
                return this.innerContent;
            }
            set
            {
                this.innerContent = value;
            }
        }

        /// <summary>
        /// Gets or sets whether encode contents or not
        /// </summary>
        /// <value>
        /// Whether encode contents or not
        /// </value>
        [IgnoreDataMember]
        public virtual bool EncodeContents
        {
            get
            {
                return this.encodeContents;
            }
            set
            {
                this.encodeContents = value;
            }
        }

        // methods

        /// <summary>
        /// Creates web control
        /// </summary>
        /// <returns>Web control</returns>
        public override WebUI.Control CreateWebControl()
        {
            HtmlGenericControl element = new HtmlGenericControl(this.tagName);
            this.AddWebControlAttributes(element.Attributes);

            this.AddWebControlChildren(element);
            if (element.Controls.Count == 0)
            {
                if (this.encodeContents)
                {
                    element.InnerText = this.innerContent;
                }
                else
                {
                    element.InnerHtml = this.innerContent;
                }
            }

            this.MakeWebControlAwareOf(element);

            return element;
        }
    }
}
