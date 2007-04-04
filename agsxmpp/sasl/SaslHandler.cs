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

using agsXMPP.protocol;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.bind;
using agsXMPP.protocol.iq.session;
using agsXMPP.protocol.sasl;
using agsXMPP.protocol.stream;

using agsXMPP.sasl;

using agsXMPP.Xml;
using agsXMPP.Xml.Dom;

namespace agsXMPP.sasl
{
	/// <summary>
	/// Summary description for SaslHandler.
	/// </summary>
	internal class SaslHandler : IDisposable
	{		
		public event SaslEventHandler	OnSaslStart;
		public event ObjectHandler		OnSaslEnd;

		private XmppClientConnection	m_XmppClient	= null;		
		private agsXMPP.sasl.Mechanism	m_Mechanism		= null;
		// Track whether Dispose has been called.
		private bool					disposed		= false;

		public SaslHandler(XmppClientConnection conn)
		{
			m_XmppClient = conn;		
		
			m_XmppClient.StreamParser.OnStreamElement	+= new StreamHandler(OnStreamElement);
		}

		// Use C# destructor syntax for finalization code.
		// This destructor will run only if the Dispose method 
		// does not get called.
		// It gives your base class the opportunity to finalize.
		// Do not provide destructors in types derived from this class.
		~SaslHandler()      
		{
			// Do not re-create Dispose clean-up code here.
			// Calling Dispose(false) is optimal in terms of
			// readability and maintainability.
			Dispose(false);
		}
		
		internal void OnStreamElement(object sender, agsXMPP.Xml.Dom.Node e)
		{
            if ( m_XmppClient.XmppConnectionState == XmppConnectionState.Securing
                || m_XmppClient.XmppConnectionState == XmppConnectionState.StartCompression)
                return;

			if ( e.GetType() == typeof(protocol.stream.Features) )
			{
				Features f = e as Features;
				if (!m_XmppClient.Authenticated)
				{
					// RECV: <stream:features><mechanisms xmlns='urn:ietf:params:xml:ns:xmpp-sasl'>
					//			<mechanism>DIGEST-MD5</mechanism><mechanism>PLAIN</mechanism>
					//			</mechanisms>
					//			<register xmlns='http://jabber.org/features/iq-register'/>
					//		</stream:features>
					// SENT: <auth mechanism="DIGEST-MD5" xmlns="urn:ietf:params:xml:ns:xmpp-sasl"/>				
					// Select a SASL mechanism
					
					SaslEventArgs args = new SaslEventArgs(f.Mechanisms);
				
					if (OnSaslStart != null)				
						OnSaslStart(this, args);
				
					if (args.Auto == true)
					{	
						// Library handles the Sasl stuff
						if (f.Mechanisms!=null)
						{
                            if (m_XmppClient.UseStartTLS == false && m_XmppClient.UseSSL == false
                                && f.Mechanisms.SupportsMechanism(MechanismType.X_GOOGLE_TOKEN) )
                            {
                                // This is the only way to connect to GTalk on a unsecure Socket for now
                                // Secure authentication is done over https requests to pass the
                                // authentication credentials on a secure connection
                                args.Mechanism = agsXMPP.protocol.sasl.Mechanism.GetMechanismName(MechanismType.X_GOOGLE_TOKEN);
                            }
							else if (f.Mechanisms.SupportsMechanism(MechanismType.DIGEST_MD5))
							{
								args.Mechanism = agsXMPP.protocol.sasl.Mechanism.GetMechanismName(MechanismType.DIGEST_MD5);
							}
							else if (f.Mechanisms.SupportsMechanism(MechanismType.PLAIN))							
							{
								args.Mechanism = agsXMPP.protocol.sasl.Mechanism.GetMechanismName(MechanismType.PLAIN);
							}
							else
							{								
								args.Mechanism = null;
							}
						}
						else
						{
							// Hack for Google
                            // TODO: i don't think we need this anymore. This was in an very early version of the gtalk server.
							args.Mechanism = null;
							//args.Mechanism = agsXMPP.protocol.sasl.Mechanism.GetMechanismName(agsXMPP.protocol.sasl.MechanismType.PLAIN);
						}
					}
					if (args.Mechanism != null)
					{
						m_Mechanism = Factory.SaslFactory.GetMechanism(args.Mechanism);
						// Set properties for the SASL mechanism
						m_Mechanism.Username = m_XmppClient.Username;
						m_Mechanism.Password = m_XmppClient.Password;
						m_Mechanism.Server = m_XmppClient.Server;
						// Call Init Method on the mechanism
						m_Mechanism.Init(m_XmppClient);
					}
					else
					{
						m_XmppClient.RequestLoginInfo();												
					}
				}
				else if(!m_XmppClient.Binded)
				{
					if (f.SupportsBind)
					{
						m_XmppClient.DoChangeXmppConnectionState(XmppConnectionState.Binding);
						BindIq bIq = new BindIq(IqType.set, new Jid(m_XmppClient.Server), m_XmppClient.Resource);						
						m_XmppClient.IqGrabber.SendIq(bIq, new IqCB(BindResult), null);					
					}
				}
								
			}
			else if ( e.GetType() == typeof(protocol.sasl.Challenge) )
			{
				if (m_Mechanism != null && !m_XmppClient.Authenticated)
				{
					m_Mechanism.Parse(e);
				}			
			}
			else if ( e.GetType() == typeof(protocol.sasl.Success) )
			{
				// SASL authentication was successfull
				if (OnSaslEnd!=null)
					OnSaslEnd(this);
								
				m_XmppClient.DoChangeXmppConnectionState(XmppConnectionState.Authenticated);
								
				m_Mechanism = null;

				m_XmppClient.Reset();				
			}
			else if ( e.GetType() == typeof(protocol.sasl.Failure) )
			{
				// Authentication failure
				m_XmppClient.FireOnAuthError(e as Element);
			}
		}
	

		private void BindResult(object sender, IQ iq, object data)
		{	
			// Once the server has generated a resource identifier for the client or accepted the resource 
			// identifier provided by the client, it MUST return an IQ stanza of type "result" 
			// to the client, which MUST include a <jid/> child element that specifies the full JID for 
			// the connected resource as determined by the server:

			// Server informs client of successful resource binding: 
			// <iq type='result' id='bind_2'>
			//  <bind xmlns='urn:ietf:params:xml:ns:xmpp-bind'>
			//    <jid>somenode@example.com/someresource</jid>
			//  </bind>
			// </iq>
			if (iq.Type == IqType.result)
			{				
				// i assume the server could assign another resource here to the client
				// so grep the resource assigned by the server now
				Element bind = iq.SelectSingleElement(typeof(Bind));
                if (bind != null)
                {
                    Jid jid = ((Bind)bind).Jid;
                    m_XmppClient.Resource = jid.Resource;
                    m_XmppClient.Username = jid.User;
                }
				
				m_XmppClient.DoChangeXmppConnectionState(XmppConnectionState.Binded);
				m_XmppClient.m_Binded = true;
				
				m_XmppClient.DoRaiseEventBinded();
				
				// success, so start the session now
				m_XmppClient.DoChangeXmppConnectionState(XmppConnectionState.StartSession);
				SessionIq sIq = new SessionIq(IqType.set, new Jid(m_XmppClient.Server));
				m_XmppClient.IqGrabber.SendIq(sIq, new IqCB(SessionResult), null);

			}
			else if (iq.Type == IqType.error)
			{
				// TODO, handle bind errors
			}			
		}

		private void SessionResult(object sender, IQ iq, object data)
		{
			if (iq.Type == IqType.result)
			{
				m_XmppClient.DoChangeXmppConnectionState(XmppConnectionState.SessionStarted);
				m_XmppClient.RaiseOnLogin();

			}
			else if (iq.Type == IqType.error)
			{

			}
		}

		#region IDisposable Members

		public void Dispose()
		{
			Dispose(true);
		}

		// Dispose(bool disposing) executes in two distinct scenarios.
		// If disposing equals true, the method has been called directly
		// or indirectly by a user's code. Managed and unmanaged resources
		// can be disposed.
		// If disposing equals false, the method has been called by the 
		// runtime from inside the finalizer and you should not reference 
		// other objects. Only unmanaged resources can be disposed.
		private void Dispose(bool disposing)
		{
			// Check to see if Dispose has already been called.
			if(!this.disposed)
			{
				// If disposing equals true, dispose all managed 
				// and unmanaged resources.
				if(disposing)
				{
					// Dispose managed resources.
					// Remove the event handler or we will be in trouble with too many events
					m_XmppClient.StreamParser.OnStreamElement	-= new StreamHandler(OnStreamElement);
					m_XmppClient	= null;		
					m_Mechanism		= null;
				}
             
				// Call the appropriate methods to clean up 
				// unmanaged resources here.
				// If disposing is false, 
				// only the following code is executed.
				        
			}
			disposed = true;         
		}


		#endregion
	}
}