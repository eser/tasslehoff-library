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

namespace Tasslehoff.Library.Layout
{
    using System;
    using System.Runtime.Serialization;
    using System.Web.UI.HtmlControls;
    using Tasslehoff.Library.Text;
    using WebUI = System.Web.UI;

    /// <summary>
    /// Container class.
    /// </summary>
    [Serializable]
    [DataContract]
    public class Container : LayoutControl
    {
        // fields

        /// <summary>
        /// Tag name
        /// </summary>
        [DataMember]
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
        /// Creates web control
        /// </summary>
        /// <returns>Web control</returns>
        public override WebUI.Control CreateWebControl()
        {
            HtmlGenericControl element = new HtmlGenericControl(this.TagName);
            this.AddWebControlAttributes(element.Attributes);
            this.AddWebControlChildren(element);
            this.MakeWebControlAwareOf(element);

            return element;
        }

        /// <summary>
        /// Occurs when [export].
        /// </summary>
        /// <param name="jsonOutputWriter">Json Output Writer</param>
        public override void OnExport(JsonOutputWriter jsonOutputWriter)
        {
            if (this.TagName != "div")
            {
                jsonOutputWriter.WriteProperty("TagName", this.TagName);
            }
        }
    }
}
