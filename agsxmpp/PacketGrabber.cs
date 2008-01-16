/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * Copyright (c) 2003-2008 by AG-Software 											 *
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
using System.Collections;

namespace agsXMPP
{
	/// <summary>
	/// Summary description for Grabber.
	/// </summary>
	public class PacketGrabber
	{
		internal Hashtable				m_grabbing		= new Hashtable();		
		internal XmppClientConnection	m_connection	= null;

		public PacketGrabber()
		{
		}

		public void Clear()
		{
			// need locking here to make sure that we dont acces the Hashtable
			// from another thread
			lock(this)
			{
				m_grabbing.Clear();
			}
		}
	}
}
