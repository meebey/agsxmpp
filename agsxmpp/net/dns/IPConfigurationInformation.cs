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

#if WIN32
// This class is based from demo code from the .NET Framework SDK.  (IPConfig.vb)
// Also, search the net for MyIPConfig.cs for a C# version.  Both those versions
// do more than this class - they also can do DHCP release and renew, which we don't
// need
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace agsXMPP.net.dns
{
   //Constants used in the program as defined in IPExport.h and WinError.h
   public class IPConfigConst 
   {
      public const int MAX_ADAPTER_NAME  = 128;
      public const int MAX_HOSTNAME_LEN  = 128;
      public const int MAX_DOMAIN_NAME_LEN  = 128;
      public const int MAX_SCOPE_ID_LEN  = 256;
      public const int MAX_ADAPTER_DESCRIPTION_LENGTH  = 128;
      public const int MAX_ADAPTER_NAME_LENGTH  = 256;
      public const int MAX_ADAPTER_ADDRESS_LENGTH  = 8;
      public const int DEFAULT_MINIMUM_ENTITIES  = 32;

      public const int ERROR_BUFFER_OVERFLOW  = 111;
      public const int ERROR_INSUFFICIENT_BUFFER = 122;
      public const int ERROR_SUCCESS  = 0;
   }

   public enum NodeType_T 
   {
      Broadcast = 1,
      PeerToPeer= 2,
      Mixed= 4,
      Hybrid= 8,
   }

   public enum IPAdapterType_T 
   {
      UnknownOrOther  = 1,
      Ethernet  = 6, 
      Tokenring  = 9, 
      FDDI  = 15, 
      PPP  = 23, 
      Loopback  = 24, 
      SLIP  = 28, 
   }

   [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
   public struct IPAdapterInfo 
   {
      public IntPtr NextPointer ;
      public int ComboIndex ;

      [MarshalAs(UnmanagedType.ByValTStr, SizeConst=IPConfigConst.MAX_ADAPTER_NAME_LENGTH + 4)]
      public string AdapterName ;

      [MarshalAs(UnmanagedType.ByValTStr, SizeConst=IPConfigConst.MAX_ADAPTER_DESCRIPTION_LENGTH + 4)] 
      public string Description;

      public int AddressLength  ;

      [MarshalAs(UnmanagedType.ByValArray, 
          SizeConst=IPConfigConst.MAX_ADAPTER_ADDRESS_LENGTH)]
      public Byte[] Address;

      public int Index;

      public IPAdapterType_T Type;  

      public int DhcpEnabled;
      public IntPtr CurrentIPAddress;
      public IPAddrString IPAddressList;
      public IPAddrString GatewayList;
      public IPAddrString DhcpServer;
      public Boolean HaveWins;
      public IPAddrString PrimaryWinsServer;
      public IPAddrString SecondaryWinsServer;
      public int LeaseObtained;
      public int LeaseExpires;
   }

   [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
   public struct IPAddrString 
   {
      public IntPtr NextPointer;

      [MarshalAs(UnmanagedType.ByValTStr, SizeConst=4 * 4)]
      public String IPAddressString;

      [MarshalAs(UnmanagedType.ByValTStr, SizeConst=4 * 4)]
      public String IPMaskString;

      public int Context;
   }

   [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
   public struct IPAdapterIndexMap 
   {
      public int Index;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst=IPConfigConst.MAX_ADAPTER_NAME)]
      public String Name;
   }


   [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
   public struct IPInterfaceInfo 
   {
      public int NumAdapters;
      public IPAdapterIndexMap Adapter;
   }


   [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
   public struct FixedInfo 
   {
      [MarshalAs(UnmanagedType.ByValTStr, 
          SizeConst=IPConfigConst.MAX_HOSTNAME_LEN + 4)]
      public String HostName;

      [MarshalAs(UnmanagedType.ByValTStr, 
          SizeConst=IPConfigConst.MAX_DOMAIN_NAME_LEN + 4)]
      public String DomainName;

      public IntPtr CurrentServerList;
      public IPAddrString DnsServerList;
      public NodeType_T NodeType;

      [MarshalAs(UnmanagedType.ByValTStr, SizeConst=IPConfigConst.MAX_SCOPE_ID_LEN + 4)]
      public String ScopeId;

      public int EnableRouting;
      public int EnableProxy;
      public int EnableDns;
   }


   public class Iphlpapi 
   {
      [DllImport("Iphlpapi.dll", CharSet=CharSet.Auto)]
      public static extern int GetNetworkParams(Byte[] PFixedInfoBuffer, ref int size);

      [DllImport("Iphlpapi.dll" , CharSet=CharSet.Auto)]
      public static extern int  GetAdaptersInfo(Byte[] PAdapterInfoBuffer, ref int size);
   }


   public class MemoryCopier 
   {
      [DllImport("Kernel32.dll", EntryPoint="CopyMemory")]
      public static extern void ByteArray_To_FixedInfo(ref FixedInfo dst, Byte[] src, int size);

      [DllImport("Kernel32.dll", EntryPoint="CopyMemory")]
      public static extern void IntPtr_To_IPAddrString(ref IPAddrString dst, IntPtr src, int size);

      [DllImport("Kernel32.dll", EntryPoint="CopyMemory")]
      public static extern void ByteArray_To_IPAdapterInfo(ref IPAdapterInfo dst, Byte[] src, int size);

      [DllImport("Kernel32.dll", EntryPoint="CopyMemory")]
      public static extern void IntPtr_To_IPAdapterInfo(ref IPAdapterInfo dst, IntPtr src, int size);
   }


   /// <summary>
	/// Summary description for IPConfigurationInformation.
	/// </summary>
	public class IPConfigurationInformation
   {
      private static string _hostname;
      private static string _domain;
      private static string[] _dnsServers;
      private static object lockObject;

      static IPConfigurationInformation ()
      {
         lockObject = new Object();
      }

      public static string Hostname
      {
         get
         {
            lock (lockObject)
            {
               if (_hostname == null)
               {
                  GetHostInfo();
               }
            }
            return _hostname;
         }
      }

      public static string Domain
      {
         get
         {
            lock (lockObject)
            {
               if (_domain == null)
               {
                  GetHostInfo();
               }
               return Domain;
            }
         }
      }

      public static string[] DnsServers
      {
         get
         {
            lock (lockObject)
            {
               if (_dnsServers == null)
               {
                  GetHostInfo();
               }
            }
            return _dnsServers;
            //  for Exception testing
            // return new string[1] { "192.168.74.1" };
         }
      }

      // This method should be called whenever you want to try to get the current
      // information from the OS rather than from the cache.  E.g. it would probably
      // be good to refresh if a new/different NIC is going to be used (because
      // it might have different DNS servers set up for it, for example)
      public static void Refresh()
      {
         lock (lockObject)
         {
            _dnsServers = null;
            _domain = null;
            _hostname = null;
         }
      }

      private static void GetHostInfo()
      {
         int size=0;
         int r = Iphlpapi.GetNetworkParams(null, ref size);
         if ((r != IPConfigConst.ERROR_SUCCESS) && (r != IPConfigConst.ERROR_BUFFER_OVERFLOW)) 
         {
            throw new Exception("Error invoking GetNetworkParams() " + r);
         }

         Byte[] buffer= new Byte[size];
         r = Iphlpapi.GetNetworkParams(buffer, ref size);
         if (r != IPConfigConst.ERROR_SUCCESS) 
         {
            throw new Exception("Error invoking GetNetworkParams() " + r);
         }

         FixedInfo PFixedInfo= new FixedInfo();
         MemoryCopier.ByteArray_To_FixedInfo(ref PFixedInfo, buffer, Marshal.SizeOf(PFixedInfo));

         _hostname = PFixedInfo.HostName;
         _domain = PFixedInfo.DomainName;

         IPAddrString ListItem = new IPAddrString(); 
         IntPtr ListNext = new IntPtr();

         System.Collections.ArrayList servers = new System.Collections.ArrayList();
         servers.Add(PFixedInfo.DnsServerList.IPAddressString);

         ListNext = PFixedInfo.DnsServerList.NextPointer;
         while (ListNext.ToInt32() != 0) 
         {
            MemoryCopier.IntPtr_To_IPAddrString(ref ListItem, ListNext, Marshal.SizeOf(ListItem));
            servers.Add(ListItem.IPAddressString);
            ListNext = ListItem.NextPointer;
         }
         if (servers.Count > 0)
         {
            _dnsServers = (string[])servers.ToArray(typeof(string));
         }
      }
	}
}
#endif