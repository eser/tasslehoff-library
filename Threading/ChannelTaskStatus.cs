// -----------------------------------------------------------------------
// <copyright file="ChannelTaskStatus.cs" company="-">
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

namespace Laroux.ScabbiaLibrary.Threading
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// ChannelTaskStatus enumeration.
    /// </summary>
    /// <remarks>Converted from byte to int due to CLS compliancy.</remarks>
    [Serializable]
    [DataContract]
    public enum ChannelTaskStatus
    {
        /// <summary>
        /// Task is NotStarted
        /// </summary>
        [EnumMember]
        NotStarted = 0,

        /// <summary>
        /// Task is Running
        /// </summary>
        [EnumMember]
        Running = 1,

        /// <summary>
        /// Task is Finished
        /// </summary>
        [EnumMember]
        Finished = 2,

        /// <summary>
        /// Task is Cancelled
        /// </summary>
        [EnumMember]
        Cancelled = 3
    }
}