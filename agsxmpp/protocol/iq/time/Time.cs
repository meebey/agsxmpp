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

namespace agsXMPP.protocol.iq.time
{

	// Send:<iq type='get' id='MX_7' to='jfrankel@coversant.net/SoapBox'>
	//			<query xmlns='jabber:iq:time'></query>
	//		</iq>
	//
	// Recv:<iq from="jfrankel@coversant.net/SoapBox" id="MX_7" to="gnauck@myjabber.net/Office" type="result">
	//			<query xmlns="jabber:iq:time">
	//				<utc>20050125T00:06:15</utc>
	//				<display>Tuesday, January 25, 2005 12:06:15 AM</display>	
	//				<tz>W. Europe Standard Time</tz>
	//			</query>
	//		</iq> 

	/// <summary>
	/// Zusammenfassung für Time.
	/// </summary>
	public class Time : Element
	{
		public Time()
		{
			this.TagName	= "query";
			this.Namespace	= Uri.IQ_TIME;
		}


		public string Utc 
		{
			get 
			{ 
				return GetTag("utc"); 
			}
			set 
			{ 
				SetTag("utc", value);
			}
		}

		/// <summary>
		/// Timezone
		/// </summary>
		//TODO: return System.TimeZone?
		public string Tz
		{			
			get 
			{ 
				return GetTag("tz"); 
			}
			set 
			{ 
				SetTag("tz", value);			
			}
		}

		/// <summary>
		/// Human-readable date/time.
		/// </summary>
		public string Display
		{
			get 
			{ 
				return GetTag("display"); 
			}
			set 
			{ 
				SetTag("display", value);
			}
		}
	}
}
