using System;
using System.Text;

using System.Xml;

using agsXMPP.Xml.Dom;

namespace CodeSnippets
{
    class Program
    {
        static void Main(string[] args)
        {
            Shim shim = new Shim();

            Presence pres = new Presence();

            Console.WriteLine("Press enter key to close this program!");
            Console.ReadLine();
        }

        internal static void Print(Element el)
        {
            Console.WriteLine(el.ToString(Formatting.Indented));
            Console.WriteLine("\r\n");
        }
    }
}
