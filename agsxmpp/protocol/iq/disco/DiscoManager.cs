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

using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.disco;

namespace agsXMPP.protocol.iq.disco
{
    public class DiscoManager
    {
        private XmppClientConnection	m_connection	= null;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="con"></param>
        public DiscoManager(XmppClientConnection con)
        {
            m_connection = con;
        }

        #region << Discover Info >>
        public void DiscoverInformation(Jid to)
        {
            DiscoverInformation(to, null, null, null, null);
        }

        public void DiscoverInformation(Jid to, Jid from)
        {
            DiscoverInformation(to, from, null, null, null);
        }

        public void DiscoverInformation(Jid to, IqCB cb)
        {
            DiscoverInformation(to, null, null, cb, null);
        }

        public void DiscoverInformation(Jid to, Jid from, IqCB cb)
        {
            DiscoverInformation(to, from, null, cb, null);
        }

        public void DiscoverInformation(Jid to, IqCB cb, object cbArgs)
        {
            DiscoverInformation(to, null, null, cb, cbArgs);
        }

        public void DiscoverInformation(Jid to, Jid from, IqCB cb, object cbArgs)
        {
            DiscoverInformation(to, from, null, cb, cbArgs);
        }

        public void DiscoverInformation(Jid to, string node)
        {
            DiscoverInformation(to, null, node, null, null);
        }

        public void DiscoverInformation(Jid to, Jid from, string node)
        {
            DiscoverInformation(to, from, node, null, null);
        }

        public void DiscoverInformation(Jid to, string node, IqCB cb)
        {
            DiscoverInformation(to, null, node, cb, null);
        }

        public void DiscoverInformation(Jid to, Jid from, string node, IqCB cb)
        {
            DiscoverInformation(to, from, node, cb, null);
        }

        public void DiscoverInformation(Jid to, string node, IqCB cb, object cbArgs)
        {
            DiscoverInformation(to, null, node, cb, cbArgs);
        }
        public void DiscoverInformation(Jid to, Jid from, string node, IqCB cb, object cbArgs)
        {
            /*
            
            Example 9. Querying a specific JID and node combination
            
            <iq type='get'
                from='romeo@montague.net/orchard'
                to='mim.shakespeare.lit'
                id='info3'>
              <query xmlns='http://jabber.org/protocol/disco#info' 
                     node='http://jabber.org/protocol/commands'/>
            </iq>
                  

            Example 10. JID+node result

            <iq type='result'
                from='mim.shakespeare.lit'
                to='romeo@montague.net/orchard'
                id='info3'>
              <query xmlns='http://jabber.org/protocol/disco#info' 
                     node='http://jabber.org/protocol/commands'>
                <identity
                    category='automation'
                    type='command-list'/>
              </query>
            </iq>
            */
            DiscoInfoIq discoIq = new DiscoInfoIq(IqType.get);
            discoIq.To = to;

            if (from != null)
                discoIq.From = from;

            if (node != null && node.Length > 0)
                discoIq.Query.Node = node;
            
            m_connection.IqGrabber.SendIq(discoIq, cb, cbArgs);
        }
        #endregion

        #region << Discover Items >>
        public void DisoverItems(Jid to)
        {
            DisoverItems(to, null, null, null);
        }

        public void DisoverItems(Jid to, Jid from)
        {
            DisoverItems(to, from, null, null, null);
        }

        public void DisoverItems(Jid to, IqCB cb)
        {
            DisoverItems(to, null, null, cb, null);
        }

        public void DisoverItems(Jid to, Jid from, IqCB cb)
        {
            DisoverItems(to, from, null, cb, null);
        }

        public void DisoverItems(Jid to, IqCB cb, object cbArgs)
        {
            DisoverItems(to, null, null, cb, cbArgs);
        }

        public void DisoverItems(Jid to, Jid from, IqCB cb, object cbArgs)
        {
            DisoverItems(to, from, null, cb, cbArgs);
        }

        public void DisoverItems(Jid to, string node)
        {
            DisoverItems(to, null, node, null, null);
        }

        public void DisoverItems(Jid to, Jid from, string node)
        {
            DisoverItems(to, from, node, null, null);
        }

        public void DisoverItems(Jid to, string node, IqCB cb)
        {
            DisoverItems(to, null, node, cb, null);
        }

        public void DisoverItems(Jid to, Jid from, string node, IqCB cb)
        {
            DisoverItems(to, from, node, cb, null);
        }

        public void DisoverItems(Jid to, string node, IqCB cb, object cbArgs)
        {
            DisoverItems(to, null, node, cb, cbArgs);
        }

        public void DisoverItems(Jid to, Jid from, string node, IqCB cb, object cbArgs)
        {
            DiscoItemsIq discoIq = new DiscoItemsIq(IqType.get);
            discoIq.To = to;
            
            if (from != null)
                discoIq.From = from;

            if (node != null && node.Length > 0)
                discoIq.Query.Node = node;

            m_connection.IqGrabber.SendIq(discoIq, cb, cbArgs);
        }
        #endregion
    }
}