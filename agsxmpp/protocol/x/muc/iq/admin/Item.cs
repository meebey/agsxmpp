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

namespace agsXMPP.protocol.x.muc.iq.admin
{
    public class Item : agsXMPP.protocol.x.muc.Item
    {
        /// <summary>
        /// 
        /// </summary>
        public Item() : base()
        {
            this.Namespace = Uri.MUC_ADMIN;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="affiliation"></param>
        public Item(Affiliation affiliation) : this()
        {
            this.Affiliation = affiliation;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="role"></param>
        public Item(Role role) : this()
        {
            this.Role = role;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="affiliation"></param>
        /// <param name="role"></param>
        public Item(Affiliation affiliation, Role role) : this(affiliation)
        {
            this.Role = role;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="affiliation"></param>
        /// <param name="role"></param>
        /// <param name="reason"></param>
        public Item(Affiliation affiliation, Role role, string reason) : this(affiliation, role)
        {
            this.Reason = reason;
        }

    }
}
