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

namespace agsXMPP.protocol.extensions.bookmarks
{
    public class BookmarkManager
    {
        private XmppClientConnection	m_connection	= null;

        
        public BookmarkManager(XmppClientConnection con)
        {
            m_connection = con;
        }
        
        #region << Request Bookmarks >>
        /// <summary>
        /// Request the bookmarks from the storage on the server
        /// </summary>
        public void RequestBookmarks()
        {
            RequestBookmarks(null);
        }

        /// <summary>
        /// Request the bookmarks from the storage on the server
        /// </summary>
        /// <param name="cb"></param>
        /// <param name="cbArgs"></param>
        public void RequestBookmarks(IqHandler cb)
        {
            StorageIq siq = new StorageIq(IqType.get);
                      
            if (cb == null)
                m_connection.Send(siq);
            else
                m_connection.IqGrabber.SendIq(siq, cb);
        }
        #endregion


        #region << Store Bookmarks >>
        /// <summary>
        /// Send booksmarks to the server storage
        /// </summary>
        /// <param name="urls"></param>
        public void StoreBookmarks(Url[] urls)
        {
            StoreBookmarks(urls, null, null);
        }

        /// <summary>
        /// Send booksmarks to the server storage
        /// </summary>
        /// <param name="urls"></param>
        /// <param name="cb"></param>
        /// <param name="cbArgs"></param>
        public void StoreBookmarks(Url[] urls, IqHandler cb)
        {
            StoreBookmarks(urls, null, cb);
        }

        /// <summary>
        /// Send booksmarks to the server storage
        /// </summary>
        /// <param name="conferences"></param>
        public void StoreBookmarks(Conference[] conferences)
        {
            StoreBookmarks(null, conferences, null);
        }

        /// <summary>
        /// Send booksmarks to the server storage
        /// </summary>
        /// <param name="conferences"></param>
        /// <param name="cb"></param>
        /// <param name="cbArgs"></param>
        public void StoreBookmarks(Conference[] conferences, IqHandler cb)
        {
            StoreBookmarks(null, conferences, cb);
        }

        /// <summary>
        /// Send booksmarks to the server storage
        /// </summary>
        /// <param name="urls"></param>
        /// <param name="conferences"></param>
        public void StoreBookmarks(Url[] urls, Conference[] conferences)
        {
            StoreBookmarks(urls, conferences, null);
        }

        /// <summary>
        /// Send booksmarks to the server storage
        /// </summary>
        /// <param name="urls"></param>
        /// <param name="conferences"></param>
        /// <param name="cb"></param>
        /// <param name="cbArgs"></param>
        public void StoreBookmarks(Url[] urls, Conference[] conferences, IqHandler cb)
        {
            StorageIq siq = new StorageIq(IqType.set);
            
            if (urls != null)
                siq.Query.Storage.AddUrls(urls);

            if (conferences != null)
                siq.Query.Storage.AddConferences(conferences);

            if (cb == null)
                m_connection.Send(siq);
            else
                m_connection.IqGrabber.SendIq(siq, cb);
        }
        #endregion
    }
}
