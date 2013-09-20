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
using System.Collections;
using System.Threading;

using agsXMPP.protocol.client;

//using agsXMPP.protocol.component;

using agsXMPP.Xml;

namespace agsXMPP
{
	public class IqGrabber : PacketGrabber
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="conn"></param>
		public IqGrabber(XmppClientConnection conn)
		{
            m_connection = conn;
            conn.OnIq += OnIq;
		}

        public IqGrabber(XmppComponentConnection conn)
        {
            m_connection = conn;
            conn.OnIq += OnIq;
        }        
        
#if !CF
        private IQ  synchronousResponse     = null;

        private int m_SynchronousTimeout    = 5000;

        /// <summary>
        /// Timeout for synchronous requests, default value is 5000 (5 seconds)
        /// </summary>
        public int SynchronousTimeout
        {
            get { return m_SynchronousTimeout; }
            set { m_SynchronousTimeout = value; }
        }
#endif 

		/// <summary>
		/// An IQ Element is received. Now check if its one we are looking for and
		/// raise the event in this case.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void OnIq(object sender, IQEventArgs e)
		{
            // the tracker handles on iq responses, which are either result or error
            if (e.IQ.Type != IqType.error && e.IQ.Type != IqType.result)
                return;

            string id = e.IQ.Id;
			if(id == null)
				return;
		    
            IqHandler td;

			lock (m_grabbing)
			{
				td = (IqHandler) m_grabbing[id];

				if (td == null)
				{
					return;
				}
				m_grabbing.Remove(id);
			}
                       
            td(this, e);
		}

        /// <summary>
        /// Send an IQ Request and store the object with callback in the Hashtable
        /// </summary>
        /// <param name="iq">The iq to send</param>
        /// <param name="cb">the callback function which gets raised for the response</param>
        public void SendIq(IQ iq, IqHandler cb)
        {
            // check if the callback is null, in case of wrong usage of this class
            if (cb == null) {
                throw new ArgumentNullException("cb");
            }

            if (iq == null) {
                throw new ArgumentNullException("cb");
            }

            m_grabbing[iq.Id] = cb;

			m_connection.Send(iq);
		}

#if !CF
        /// <summary>
        /// Sends an Iq synchronous and return the response or null on timeout
        /// </summary>
        /// <param name="iq">The IQ to send</param>
        /// <param name="timeout"></param>
        /// <returns>The response IQ or null on timeout</returns>
        public IQ SendIq(agsXMPP.protocol.client.IQ iq, int timeout)
        {
            synchronousResponse = null;
            AutoResetEvent are = new AutoResetEvent(false);

            SendIq(iq,
                   delegate (object sender, IQEventArgs e)
                    {
                        e.Handled = true;
                        are.Set();
                    }
            );

            if (!are.WaitOne(timeout, true))
            {
                // Timed out
                lock (m_grabbing)
                {       
                    if (m_grabbing.ContainsKey(iq.Id))
                        m_grabbing.Remove(iq.Id);
                }                
                return null;
            }
            
            return synchronousResponse;
		}

        /// <summary>
        /// Sends an Iq synchronous and return the response or null on timeout.
        /// Timeout time used is <see cref="SynchronousTimeout"/>
        /// </summary>
        /// <param name="iq">The IQ to send</param>        
        /// <returns>The response IQ or null on timeout</returns>
        public IQ SendIq(IQ iq)
        {
            return SendIq(iq, m_SynchronousTimeout);
        }

#endif
	}
}