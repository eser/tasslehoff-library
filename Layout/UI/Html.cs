// -----------------------------------------------------------------------
// <copyright file="Html.cs" company="-">
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
    using System.Web.UI.HtmlControls;
    using Tasslehoff.Library.Text;

    /// <summary>
    /// Html class.
    /// </summary>
    [Serializable]
    [DataContract]
    [LayoutProperties(DisplayName = "Html Content", Icon = "header")]
    public class Html : Base
    {
        // fields

        /// <summary>
        /// Inner content
        /// </summary>
        [DataMember(Name = "InnerContent")]
        private string innerContent;

        /// <summary>
        /// Whether encode contents or not
        /// </summary>
        [DataMember(Name = "EncodeContents")]
        private bool encodeContents;

        // properties

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
        public override void CreateWebControl()
        {
            HtmlGenericControl element = new HtmlGenericControl(this.TagName);

            this.AddWebControlAttributes(element, element.Attributes);
            this.AddWebControlChildren(element);

            if (element.Controls.Count == 0)
            {
                if (this.EncodeContents)
                {
                    element.InnerText = this.InnerContent;
                }
                else
                {
                    element.InnerHtml = this.InnerContent;
                }
            }

            this.WebControl = element;
        }

        /// <summary>
        /// Occurs when [export].
        /// </summary>
        /// <param name="jsonOutputWriter">Json Output Writer</param>
        public override void OnExport(MultiFormatOutputWriter jsonOutputWriter)
        {
            base.OnExport(jsonOutputWriter);

            if (this.EncodeContents != false)
            {
                jsonOutputWriter.WriteProperty("EncodeContents", this.EncodeContents);
            }

            if (!string.IsNullOrEmpty(this.InnerContent))
            {
                jsonOutputWriter.WriteProperty("InnerContent", this.InnerContent);
            }
        }

        /// <summary>
        /// Occurs when [export].
        /// </summary>
        /// <param name="jsonOutputWriter">Json Output Writer</param>
        public override void OnGetEditProperties(Dictionary<string, string> properties)
        {
            base.OnGetEditProperties(properties);

            properties.Add("InnerContent", "Inner Content");
            properties.Add("EncodeContents", "Encode Contents");
        }
    }
}
