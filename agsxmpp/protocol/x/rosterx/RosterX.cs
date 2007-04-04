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

using agsXMPP.Xml.Dom;

namespace agsXMPP.protocol.x.rosterx
{
	/// <summary>
	/// Roster Item Exchange (JEP-0144)
	/// </summary>
	public class RosterX : Element
	{
		/*
		<message from='horatio@denmark.lit' to='hamlet@denmark.lit'>
		<body>Some visitors, m'lord!</body>
		<x xmlns='http://jabber.org/protocol/rosterx'> 
			<item action='add'
				jid='rosencrantz@denmark.lit'
				name='Rosencrantz'>
				<group>Visitors</group>
			</item>
			<item action='add'
				jid='guildenstern@denmark.lit'
				name='Guildenstern'>
				<group>Visitors</group>
			</item>
		</x>
		</message>
		*/

		public RosterX()
		{
			this.TagName	= "x";
			this.Namespace	= Uri.X_ROSTERX;
		}
		
		public RosterItem[] GetRoster()
		{
            ElementList nl = SelectElements(typeof(RosterItem));
			int i = 0;
			RosterItem[] result = new RosterItem[nl.Count];
			foreach (RosterItem ri in nl)
			{
				result[i] = (RosterItem) ri;				
				i++;
			}
			return result;
		}
		
		public void AddRosterItem(RosterItem r)
		{
			this.ChildNodes.Add(r);
		}
	}
}
