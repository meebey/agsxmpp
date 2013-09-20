/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * Copyright (c) 2003-2012 by AG-Software 											 *
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
        private XmppClientConnection	xmppConnection	= null;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="con"></param>
        public DiscoManager(XmppClientConnection con)
        {
            xmppConnection = con;
            xmppConnection.OnIq += new IqHandler(OnIq);
        }

        #region << Properties >>
        private bool m_AutoAnswerDiscoInfoRequests = true;  

        /// <summary>
        /// Automatically answer DiscoInfo requests.
        /// Set disco information (identties and features) in the DiscoInfo property object.        
        /// </summary>
        public bool AutoAnswerDiscoInfoRequests
        {
            get { return m_AutoAnswerDiscoInfoRequests; }
            set { m_AutoAnswerDiscoInfoRequests = value; }
        }
        #endregion

        private void OnIq(object sender, IQEventArgs e)
        {
            // DiscoInfo
            if (m_AutoAnswerDiscoInfoRequests && e.IQ.Query is DiscoInfo && e.IQ.Type == IqType.get) {
                e.Handled = true;
                ProcessDiscoInfo(e.IQ);
            }
        }

        private void ProcessDiscoInfo(IQ iq)
        {            
            IQ diiq = new IQ();
            diiq.To = iq.From;
            diiq.Id = iq.Id;
            diiq.Type = IqType.result;

            diiq.Query = xmppConnection.DiscoInfo;

            xmppConnection.Send(diiq);        
        }

        #region << Discover Info >>
        public void DiscoverInformation(Jid to)
        {
            DiscoverInformation(to, null, null, null);
        }

        public void DiscoverInformation(Jid to, Jid from)
        {
            DiscoverInformation(to, from, null, null);
        }

        public void DiscoverInformation(Jid to, IqHandler cb)
        {
            DiscoverInformation(to, null, null, cb);
        }

        public void DiscoverInformation(Jid to, Jid from, IqHandler cb)
        {
            DiscoverInformation(to, from, null, cb);
        }

        public void DiscoverInformation(Jid to, string node)
        {
            DiscoverInformation(to, null, node, null);
        }

        public void DiscoverInformation(Jid to, Jid from, string node)
        {
            DiscoverInformation(to, from, node, null);
        }

        public void DiscoverInformation(Jid to, string node, IqHandler cb)
        {
            DiscoverInformation(to, null, node, cb);
        }

        public void DiscoverInformation(Jid to, Jid from, string node, IqHandler cb)
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
            
            xmppConnection.IqGrabber.SendIq(discoIq, cb);
        }
        #endregion

        #region << Discover Items >>
        public void DiscoverItems(Jid to)
        {
            DiscoverItems(to, null, null, null);
        }

        public void DiscoverItems(Jid to, Jid from)
        {
            DiscoverItems(to, from, null, null);
        }

        public void DiscoverItems(Jid to, IqHandler cb)
        {
            DiscoverItems(to, null, null, cb);
        }

        public void DiscoverItems(Jid to, Jid from, IqHandler cb)
        {
            DiscoverItems(to, from, null, cb);
        }

        public void DiscoverItems(Jid to, string node)
        {
            DiscoverItems(to, null, node, null);
        }

        public void DiscoverItems(Jid to, Jid from, string node)
        {
            DiscoverItems(to, from, node, null);
        }

        public void DiscoverItems(Jid to, string node, IqHandler cb)
        {
            DiscoverItems(to, null, node, cb);
        }

        public void DiscoverItems(Jid to, Jid from, string node, IqHandler cb)
        {
            DiscoItemsIq discoIq = new DiscoItemsIq(IqType.get);
            discoIq.To = to;
            
            if (from != null)
                discoIq.From = from;

            if (node != null && node.Length > 0)
                discoIq.Query.Node = node;

            xmppConnection.IqGrabber.SendIq(discoIq, cb);
        }
        #endregion
                        
    }
}