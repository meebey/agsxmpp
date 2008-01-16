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

using agsXMPP.protocol.extensions.primary;
using agsXMPP.protocol.extensions.nickname;

namespace agsXMPP.protocol.client
{
	/// <summary>
	/// Zusammenfassung für Presence.
	/// </summary>
	public class Presence : Base.Stanza
    {
        #region << Constructors >>
        public Presence()
		{
			this.TagName	= "presence";
			this.Namespace	= Uri.CLIENT;
		}

		public Presence(ShowType show, string status) : this()
		{			
			this.Show		= show;
			this.Status		= status;
		}

		public Presence(ShowType show, string status, int priority) : this(show, status)
		{
			this.Priority	= priority;
        }
        #endregion

        /// <summary>
		/// Presence status
		/// </summary>
		public string Status
		{
			get { return GetTag("status"); }
			set { SetTag("status", value); }
		}

		public PresenceType Type
		{
			get	
			{
				return (PresenceType) GetAttributeEnum("type", typeof(PresenceType));
			}
			set
			{
				// dont add type="available"
				if (value == PresenceType.available)
					RemoveAttribute("type");
				else
					SetAttribute("type", value.ToString()); 
			}

		}

        /// <summary>
        /// Error Child Element
        /// </summary>
        public agsXMPP.protocol.client.Error Error
        {
            get
            {
                return SelectSingleElement(typeof(agsXMPP.protocol.client.Error)) as agsXMPP.protocol.client.Error;

            }
            set
            {
                if (HasTag(typeof(agsXMPP.protocol.client.Error)))
                    RemoveTag(typeof(agsXMPP.protocol.client.Error));

                if (value != null)
                    this.AddChild(value);
            }
        }

		public ShowType Show
		{
			get { return (ShowType) GetTagEnum("show", typeof(ShowType)); }
			set 
			{
                if (value != ShowType.NONE)
                    SetTag("show", value.ToString());
                else
                    RemoveAttribute("show");
			}
		}

		
		/// <summary>
        // The OPTIONAL <priority/> element contains non-human-readable XML character data 
        // that specifies the priority level of the resource. The value MUST be an 
        // integer between -128 and +127. A presence stanza MUST NOT contain more than 
        // one <priority/> element. The <priority/> element MUST NOT possess any attributes.
        // If no priority is provided, a server SHOULD consider the priority to be zero. 
        // For information regarding the semantics of priority values in stanza routing 
        // within instant messaging and presence applications, refer to Server Rules 
        // for Handling XML StanzasServer Rules for Handling XML Stanzas.
		/// </summary>
		public int Priority
		{
			get 
			{ 
				try
				{
					return int.Parse(GetTag("priority")); 
				}
				catch
				{
					return 0;
				}
			}
			set { SetTag("priority", value.ToString()); }
		}

		public x.Delay XDelay
		{
			get	{ return SelectSingleElement(typeof(x.Delay)) as x.Delay; }
			set	
            {
                if (HasTag(typeof(x.Delay)))
                    RemoveTag(typeof(x.Delay));

                if (value != null)
                    this.AddChild(value);            
            }
		}
		
		public bool IsPrimary
		{
			get
			{
				return GetTag(typeof(Primary)) == null ? false : true;
			}
			set
			{
				if (value)
					SetTag(typeof(Primary));
				else
					RemoveTag(typeof(Primary));
			}
		}

        /// <summary>
        /// 
        /// </summary>
        public x.muc.User MucUser
        {
            get { return SelectSingleElement(typeof(x.muc.User)) as x.muc.User; }
            set
            {
                if (HasTag(typeof(x.muc.User)))
                    RemoveTag(typeof(x.muc.User));
                
                if (value != null)
                    this.AddChild(value);
            }
        }

        /// <summary>
        /// Nickname Element
        /// </summary>
        public Nickname Nickname
        {
            get
            {
                return SelectSingleElement(typeof(Nickname)) as Nickname;
            }
            set
            {
                if (HasTag(typeof(Nickname)))
                    RemoveTag(typeof(Nickname));

                if (value != null)
                    this.AddChild(value);
            }
        }
	}
}
