/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * Copyright (c) 2003-2007 by AG-Software 											 *
 * All Rights Reserved.																 *
 * Contact information for AG-Software is available at http://www.ag-software.de	 *
 *																					 *
 * Licence:																			 *
 * The agsXMPP SDK is released under a dual licence									 *
 * agsXMPP can be used under either of two licences									 *
 * 																					 *
 * A commercial licence which is probably the most appropriate for commercial 		 *
 * corporate use and closed source projects. 										 *
 *																					 *
 * The GNU Public License (GPL) is probably most appropriate for inclusion in		 *
 * other open source projects.														 *
 *																					 *
 * See README.html for details.														 *
 *																					 *
 * For general enquiries visit our website at:										 *
 * http://www.ag-software.de														 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Text;

using agsXMPP.Xml.Dom;

namespace agsXMPP.protocol.extensions.ibb
{
    /*
         <close xmlns='http://jabber.org/protocol/ibb' sid='mySID'/>     
      
    */

    /// <summary>
    /// 
    /// </summary>
    public class Close : Element
    {
        /// <summary>
        /// 
        /// </summary>
        public Close()
        {
            this.TagName = "close";
            this.Namespace = Uri.IBB;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sid"></param>
        public Close(string sid) : this()
        {
            this.Sid = sid;            
        }

        /// <summary>
        /// Sid
        /// </summary>
        public string Sid
        {
            get { return GetAttribute("sid"); }
            set { SetAttribute("sid", value); }
        }
    }
}