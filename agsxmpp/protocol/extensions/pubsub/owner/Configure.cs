/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * Copyright (c) 2003-2009 by AG-Software 											 *
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

using agsXMPP.protocol.x.data;

using agsXMPP.Xml.Dom;

namespace agsXMPP.protocol.extensions.pubsub.owner
{
    public class Configure : Element
    {
        #region << Constructor >>
        public Configure()
        {
            this.TagName    = "configure";
            this.Namespace  = Uri.PUBSUB_OWNER;
        }

        public Configure(string node)
        {
            this.Node = node;
        }
        #endregion

        public string Node
        {
            get { return GetAttribute("node"); }
            set { SetAttribute("node", value); }
        }

        /// <summary>
        /// The x-Data Element
        /// </summary>
        public Data Data
        {
            get
            {
                return SelectSingleElement(typeof(Data)) as Data;
            }
            set
            {
                if (HasTag(typeof(Data)))
                    RemoveTag(typeof(Data));

                if (value != null)
                    this.AddChild(value);
            }
        }
    }
}
