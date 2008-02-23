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
using System.Xml;
using System.IO;
using System.Collections;
//using System.Collections.Specialized;

namespace agsXMPP.Xml.Dom
{
	/// <summary>
	/// internal class that loads a xml document from a string or stream
	/// </summary>
	internal class DomLoader
	{	
		private Document		doc;		
		private StreamParser	sp;
		
		public DomLoader(string xml, Document d)
		{
			doc	= d;
			sp = new StreamParser();
			
			sp.OnStreamStart	+= new StreamHandler(sp_OnStreamStart);
			sp.OnStreamElement	+= new StreamHandler(sp_OnStreamElement);
			sp.OnStreamEnd		+= new StreamHandler(sp_OnStreamEnd);
            
			byte[] b = System.Text.Encoding.UTF8.GetBytes(xml);
			sp.Push(b, 0, b.Length);
		}        

		public DomLoader(StreamReader sr, Document d) : this(sr.ReadToEnd(), d)
		{

		}

		// ya, the Streamparser is only usable for parsing xmpp stream.
		// it also does a very good job here
		private void sp_OnStreamStart(object sender, Node e)
		{
			doc.ChildNodes.Add(e);
		}

		private void sp_OnStreamElement(object sender, Node e)
		{
			doc.RootElement.ChildNodes.Add(e);
		}

		private void sp_OnStreamEnd(object sender, Node e)
		{

		}
	}
}