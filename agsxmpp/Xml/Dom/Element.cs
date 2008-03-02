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
using System.Collections;
#if NET_2
using System.Collections.Generic;
#endif
using System.Collections.Specialized;
using System.Globalization;
using System.Text;
using System.IO;
using System.Xml;

//using System.Linq;
//using System.Linq.Expressions;

namespace agsXMPP.Xml.Dom
{	

	public class Element : Node
	{
		// Member Variables
		private		string			m_TagName;
		private		string			m_Prefix			= null;
        private     ListDictionary  m_Attributes;
		private		Text			m_Value				= new Text();

		public Element() 
		{
			this.NodeType = NodeType.Element;
			this.AddChild(m_Value);

            m_Attributes = new ListDictionary();

            this.m_TagName	= "";
			this.Value		= "";			
		}

        /* 
         * don't think we need this 2 constructors anymore
         * 
        public Element(NodeType type)
        {
            this.NodeType = NodeType.Element;
            this.AddChild(m_Value);

            m_Attributes = new ListDictionary();

            NodeType = type;
        }

        public Element(NodeType type, string text)
        {
            this.NodeType = NodeType.Element;
            this.AddChild(m_Value);

            m_Attributes = new ListDictionary();

            NodeType = type;
            Value = text;
        }
         */

		public Element(string tagName) :this() 
		{
            this.m_TagName = tagName;
		}

		public Element(string tagName, string tagText) : this(tagName)
		{
            this.Value		= tagText;			
		}

        public Element(string tagName, bool tagText) : this(tagName, tagText ? "true" : "false")
		{            
		}		

		public Element(string tagName, string tagText, string ns) : this(tagName, tagText)
		{
           this.Namespace		= ns;			
		}
				
		/// <summary>
		/// Is this Element a Rootnode?
		/// </summary>
		public bool IsRootElement
		{
			get
			{
				return (Parent != null ? false : true);
			}
		}

		public override string Value
		{
			get	{ return m_Value.Value;	}
			set	{ m_Value.Value = value; }
		}
		
		public string Prefix
		{
			get { return m_Prefix; }
			set { m_Prefix = value; }
		}

		/// <summary>
		/// The Full Qualified Name
		/// </summary>
		public string TagName 
		{
			get	{ return m_TagName;	}
			set { this.m_TagName = value; }
		}

		public string TextBase64
		{
			get 
			{
				byte[] b = Convert.FromBase64String(Value);
				return Encoding.ASCII.GetString(b, 0, b.Length);
			}
			set 
			{
                byte[] b = Encoding.UTF8.GetBytes(value);
				//byte[] b = Encoding.Default.GetBytes(value);
				this.Value = Convert.ToBase64String(b, 0, b.Length);
			}
		}
      
        public ListDictionary Attributes
        {
            get { return this.m_Attributes; }
        }

		public object GetAttributeEnum(string name, System.Type enumType)
		{
			string att = this.GetAttribute(name);
			if ((att == null))
				return -1;
			try
			{
#if CF
				return util.Enum.Parse(enumType, att, true);
#else
				return Enum.Parse(enumType, att, true);
#endif
			}
			catch (Exception)
			{
				return -1;
			}
		}

		public string GetAttribute(string name)
		{
			if (HasAttribute(name))
				return (string) m_Attributes[name];
			else
				return null;
		}

		public int GetAttributeInt(string name)
		{
			if (HasAttribute(name))
			{				
				return int.Parse((string) m_Attributes[name]);
			}
			else
				return 0;
		}

		public long GetAttributeLong(string name)
		{
			if (HasAttribute(name))
			{				
				return long.Parse((string) m_Attributes[name]);
			}
			else
				return 0;
		}

        /// <summary>
        /// Reads a boolean Attribute, if the attrib is absent it returns also false.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool GetAttributeBool(string name)
        {
            if (HasAttribute(name))
            {
                string tmp = (string) m_Attributes[name];
                if (tmp.ToLower() == "true")
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        public Jid GetAttributeJid(string name)
        {
            if (HasAttribute(name))
                return new Jid(this.GetAttribute(name));
            else
                return null;			
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ifp"></param>
        /// <returns></returns>
        public double GetAttributeDouble(string name, IFormatProvider ifp)
        {            
            if (HasAttribute(name))
            {
                try
                {
                    return double.Parse((string)m_Attributes[name], ifp);
                }
                catch
                {
                    return double.NaN;
                }
            }
            else
                return double.NaN;
        }        

        /// <summary>
        /// Get a Attribute of type double (Decimal seperator = ".")
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public double GetAttributeDouble(string name)
        {
            // Parse the double always in english format ==> "." = Decimal seperator
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberGroupSeparator = ".";
            return GetAttributeDouble(name, nfi); 
        }

		public bool HasAttribute(string name)
		{
			return this.Attributes.Contains(name);
		}
		
		/// <summary>
		/// Return the Text of the first Tag with a specified Name.
		/// It doesnt traverse the while tree and checks only the unerlying childnodes
		/// </summary>
		/// <param name="TagName">Name of Tag to find as string</param>
		/// <returns></returns>
		public string GetTag(string TagName)
		{
			Element tag = this._SelectElement(this, TagName);
			if ( tag != null)
				return tag.Value;
			else
				return null;
		}

		public string GetTag(string TagName, bool traverseChildren)
		{
			Element tag = this._SelectElement(this, TagName, traverseChildren);
			if ( tag != null)
				return tag.Value;
			else
				return null;
		}

		public string GetTag(System.Type type)
		{
			Element tag = this._SelectElement(this, type);
			if ( tag != null)
				return tag.Value;
			else
				return null;
		}

		public string GetTagBase64(string TagName)
		{
			byte[] b = Convert.FromBase64String(GetTag(TagName));
			return Encoding.ASCII.GetString(b, 0, b.Length);
		}
		
		/// <summary>
		/// Adds a Tag and encodes the Data to BASE64
		/// </summary>
		/// <param name="argTagname"></param>
		/// <param name="argText"></param>
		public void SetTagBase64(string argTagname, string argText)
		{
			byte[] b = Encoding.Unicode.GetBytes(argText);
			SetTag(argTagname, Convert.ToBase64String(b, 0, b.Length));
		}

		/// <summary>
		/// Adds a Tag end decodes the byte buffer to BASE64
		/// </summary>
		/// <param name="argTagname"></param>
		/// <param name="buffer"></param>
		public void SetTagBase64(string argTagname, byte[] buffer)
		{
			SetTag(argTagname, Convert.ToBase64String(buffer, 0, buffer.Length));
		}
		
		public void SetTag(string argTagname, string argText)
		{
			if (HasTag(argTagname) == false)
				this.AddChild(new Element(argTagname, argText));
			else
				this.SelectSingleElement(argTagname).Value = argText;
		}

		public void SetTag(System.Type type, string argText)
		{
			if (HasTag(type) == false)
			{
				Element newel;
				newel		= (Element) System.Activator.CreateInstance(type);
				newel.Value = argText;
				this.AddChild(newel);
			}
			else
				this.SelectSingleElement(type).Value = argText;
		}

		public void SetTag(System.Type type)
		{
			if (HasTag(type))			
				RemoveTag(type);
			
			this.AddChild( (Element) System.Activator.CreateInstance(type) );
		}

		public void SetTag(string argTagname)
		{
			SetTag(argTagname, "");
		}
                
		public void SetTag(string argTagname, string argText, string argNS)
		{
			if (HasTag(argTagname) == false)				
				this.AddChild(new Element(argTagname, argText, argNS));			
			else
			{
				Element e = this.SelectSingleElement(argTagname);
				e.Value		= argText;
				e.Namespace = argNS;
			}
		}

        public void SetTag(string argTagname, double dbl, IFormatProvider ifp)
        {
            SetTag(argTagname, dbl.ToString(ifp));
        }

        public void SetTag(string argTagname, double dbl)
        {
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberGroupSeparator = ".";
            SetTag(argTagname, dbl, nfi);
        }

        public void SetTag(string argTagname, bool val)
        {
            SetTag(argTagname, val == true ? "true" : "false");
        }

        public void SetTag(string argTagname, int val)
        {
            SetTag(argTagname, val.ToString());
        }

        public void SetTag(string argTagname, Jid jid)
        {
            SetTag(argTagname, jid.ToString());
        }

		public void AddTag(string argTagname, string argText)
		{			
			this.AddChild(new Element(argTagname, argText));			
		}

		public void AddTag(string argTagname)
		{			
			this.AddChild(new Element(argTagname));			
		}

		public object GetTagEnum(string name, System.Type enumType)
		{			
			string tag = this.GetTag(name);
			if ( (tag == null) || (tag.Length == 0) )
				return -1;
			try
			{
#if CF
				return util.Enum.Parse(enumType, tag, true);
#else
				return Enum.Parse(enumType, tag, true);
#endif
			}
			catch (Exception)
			{
				return -1;
			}
		}

		/// <summary>
		/// Return the Text of the first Tag with a specified Name in all childnodes as boolean
		/// </summary>
		/// <param name="TagName">name of Tag to findas string</param>
		/// <returns></returns>
		public bool GetTagBool(string TagName)
		{
			Element tag = this._SelectElement(this, TagName);
			if ( tag != null)
			{
				if (tag.Value.ToLower() == "false" || tag.Value.ToLower() == "0")
				{
					return false;
				}
				else if(tag.Value.ToLower() == "true" ||	tag.Value.ToLower() == "1")
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}

		public int GetTagInt(string TagName)
		{
			Element tag = this._SelectElement(this, TagName);
			if ( tag != null)
				return int.Parse(tag.Value);
			else
				return 0;
		}


        public Jid GetTagJid(string TagName)
        {
            string jid = GetTag(TagName);
            
            if (jid != null)
                return new Jid(jid);
            else
                return null;
        }         
         
        
        /// <summary>
        /// Get a Tag of type double (Decimal seperator = ".")
        /// </summary>
        /// <param name="TagName"></param>
        /// <returns></returns>
        public double GetTagDouble(string argTagName)
        {
            // Parse the double always in english format ==> "." = Decimal seperator
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberGroupSeparator = ".";
            
            return GetTagDouble(argTagName, nfi);
        }

        /// <summary>
        /// Get a Tag of type double with the given iFormatProvider
        /// </summary>
        /// <param name="TagName"></param>
        /// <param name="nfi"></param>
        /// <returns></returns>
        public double GetTagDouble(string argTagName, IFormatProvider ifp)
        {           
            string val = GetTag(argTagName);
            if (val != null)
                return Double.Parse(val, ifp);
            else
                return Double.NaN;
        }

		public bool HasTag(string name)
		{
			Element tag = this._SelectElement(this, name);
			if ( tag != null)
				return true;
			else
				return false;
		}

		public bool HasTag(string name, bool traverseChildren)
		{
			Element tag = this._SelectElement(this, name, traverseChildren);
			if ( tag != null)
				return true;
			else
				return false;
		}		

		public bool HasTag(System.Type type)
		{
			Element tag = this._SelectElement(this, type);
			if ( tag != null)
				return true;
			else
				return false;
		}

		public bool HasTag(System.Type type, bool traverseChildren)
		{
			Element tag = this._SelectElement(this, type, traverseChildren);
			if ( tag != null)
				return true;
			else
				return false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="enumType"></param>
		/// <returns></returns>		
        public object HasTagEnum(System.Type enumType)
		{			
#if CF || CF_2            
			string[] members = util.Enum.GetNames(enumType);	
#else
			string[] members = Enum.GetNames(enumType);
#endif
			foreach (string member in members)
			{
				if (HasTag(member))
#if CF
					return util.Enum.Parse(enumType, member, false);
#else
					return Enum.Parse(enumType, member, false);
#endif
			}
			return -1;			
		}

		/// <summary>
		/// Remove a Tag when it exists
		/// </summary>
		/// <param name="TagName">Tagname to remove</param>
		/// <returns>true when existing and removed, false when not existing</returns>
		public bool RemoveTag(string TagName)
		{
			Element tag = this._SelectElement(this, TagName);
			if ( tag != null)
			{
				tag.Remove();
				return true;
			}
			else
				return false;
		}

		/// <summary>
		/// Remove a Tag when it exists
		/// </summary>
		/// <param name="type">Type of the tag that should be removed</param>
		/// <returns>true when existing and removed, false when not existing</returns>
		public bool RemoveTag(System.Type type)
		{
			Element tag = this._SelectElement(this, type);
			if ( tag != null)
			{
				tag.Remove();
				return true;
			}
			else
				return false;
		}

        /// <summary>
        /// Removes all Tags of the given type. Doesnt traverse the tree
        /// </summary>
        /// <param name="type">Type of the tags that should be removed</param>
        /// <returns>true when tags were removed, false when no tags were found and removed</returns>
        public bool RemoveTags(System.Type type)
        {
            bool ret = false;

            ElementList list = this.SelectElements(type);
            
            if (list.Count > 0)
                ret = true;

            foreach (Element e in list)
                e.Remove();

            return ret;
        }

		/// <summary>
		/// Same as AddChild, but Replaces the childelement when it exists
		/// </summary>
		/// <param name="e"></param>
		public void ReplaceChild(Element e)
		{
			if (HasTag(e.TagName))
				RemoveTag(e.TagName);
			
			AddChild(e);
		}

		public string Attribute(string name) 
		{
			return (string) m_Attributes[name];
		}

		/// <summary>
		/// Removes a Attribute
		/// </summary>
		/// <param name="name">Attribute as string to remove</param>
		public void RemoveAttribute(string name)
		{
			if (this.HasAttribute(name))
			{
				this.Attributes.Remove(name);
				return;
			}
		}

		/// <summary>
		/// Adds a new Attribue or changes a Attriv when already exists
		/// </summary>
		/// <param name="name">name of Attribute to add/change</param>
		/// <param name="value">value of teh Attribute to add/change</param>
		public void SetAttribute(string name, string val) 
		{
			// When the attrib already exists then we overweite it
			// So we must remove it first and add it again then
			if (this.HasAttribute(name))
			{
				this.Attributes.Remove(name);
			}
			m_Attributes.Add(name, val);
			
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
		public void SetAttribute(string name, int value)
		{
			SetAttribute(name, value.ToString());
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetAttribute(string name, long value)
        {
            SetAttribute(name, value.ToString());
        }

        /// <summary>
        /// Writes a boolean attribute, the value is either 'true' or 'false'
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        public void SetAttribute(string name, bool val)
        {
            // When the attrib already exists then we overweite it
            // So we must remove it first and add it again then
            if (this.HasAttribute(name))
            {
                this.Attributes.Remove(name);
            }
            m_Attributes.Add(name, val == true ? "true" : "false");
        }

        /// <summary>
        /// Set a attribute of type Jid
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetAttribute(string name, Jid value)
        {
             if (value != null)
                    this.SetAttribute(name, value.ToString());
                else
                    RemoveAttribute(name);
        }

        /// <summary>
        /// Set a attribute from a double in english number format
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetAttribute(string name, double value)
        {
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberGroupSeparator = ".";
            SetAttribute(name, value, nfi);
        }

        /// <summary>
        /// Set a attribute from a double with the given Format provider
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="ifp"></param>
        public void SetAttribute(string name, double value, IFormatProvider ifp)
        {
            SetAttribute(name, value.ToString(ifp));
        }


		public void SetNamespace(string value) 
		{
			this.SetAttribute("xmlns", value);	
		}
		
		public string InnerXml
		{
			get
			{
				if (this.ChildNodes.Count > 0)
				{
					string xml = "";
					try
					{						
						for (int i = 0; i < this.ChildNodes.Count; i++)
						{
							if (this.ChildNodes.Item(i).NodeType == NodeType.Element)
								xml += this.ChildNodes.Item(i).ToString();
							else if (this.ChildNodes.Item(i).NodeType == NodeType.Text)
								xml += this.ChildNodes.Item(i).Value;
						
						}
					}
					catch (Exception ex)
					{
						Console.WriteLine(ex.Message);
					}
					return xml;

				}
				else
					return null;
			}
            set
            {
                Document doc = new Document();
                doc.LoadXml(value);
                Element root = doc.RootElement;
                if (root != null)
                {
                    this.ChildNodes.Clear();
                    this.AddChild(root);
                }
                
            }
		}

        /// <summary>
        /// returns whether the current element has child elements or not.
        /// cares only about element, not text nodes etc...
        /// </summary>
        public bool HasChildElements
        {
	        get
	        {
                foreach (Node e in this.ChildNodes)
                {
                    if (e.NodeType == NodeType.Element)
                        return true;
                }
		        return false;	     
	        }
        }

		/// <summary>
		/// returns the first child element (no textNodes)
		/// </summary>
		public Element FirstChild
		{
			get
			{                
                if (this.ChildNodes.Count > 0)
                {
                    foreach (Node e in this.ChildNodes)
                    {
                        if (e.NodeType == NodeType.Element)
                            return e as Element;
                    }
                    return null;
	            }
                
                else
                    return null;
			}
		}

		/// <summary>
		/// Returns the first ChildNode, doesnt matter of which type it is
		/// </summary>
		public Node FirstNode
		{
			get
			{
				if(this.ChildNodes.Count > 0)
					return this.ChildNodes.Item(0) as Node;
				else
					return null;
			}
		}

		/// <summary>
		/// Returns the last ChildNode, doesnt matter of which type it is
		/// </summary>
		public Node LastNode
		{
			get
			{
				if(this.ChildNodes.Count > 0)
					return this.ChildNodes.Item(ChildNodes.Count -1) as Node;
				else
					return null;
			}
		}

        internal string StartTag()
        {
            System.IO.StringWriter sw = new StringWriter();
            XmlTextWriter tw = new XmlTextWriter(sw);
            tw.Formatting = Formatting.None;
                        
            if (this.Prefix == null)
                tw.WriteStartElement(this.TagName);
            else
                tw.WriteStartElement(this.Prefix + ":" + this.TagName);

            // Write Namespace
            if (this.Namespace != null
                && this.Namespace.Length != 0
                )
            {
                if (this.Prefix == null)
                    tw.WriteAttributeString("xmlns", this.Namespace);
                else
                    tw.WriteAttributeString("xmlns:" + this.Prefix, this.Namespace);
            }

            foreach (string attName in this.Attributes.Keys)
            {
                tw.WriteAttributeString(attName, this.Attribute(attName));
            }
                        
            tw.Flush();
            tw.Close();
            
            return sw.ToString().Replace("/>", ">");
        }
        
        internal string EndTag()
        {
            if (this.Prefix == null)
                return "</" + this.TagName + ">";
            else
                return "</" + this.Prefix + ":" + this.TagName + ">";
        }

		#region << Xml Select Functions >>
        /// <summary>
        /// Find a Element by type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
		public Element SelectSingleElement(System.Type type)
		{
			return this._SelectElement(this, type);
		}

        /// <summary>
        /// find a Element by type and loop thru all children
        /// </summary>
        /// <param name="type"></param>
        /// <param name="loopChildren"></param>
        /// <returns></returns>
        public Element SelectSingleElement(System.Type type, bool loopChildren)
        {
            return this._SelectElement(this, type, true);
        }

		public Element SelectSingleElement(string TagName)
		{
			return this._SelectElement(this, TagName);
		}

        public Element SelectSingleElement(string TagName, bool traverseChildren)
        {
            return this._SelectElement(this, TagName, true);
        }

		public Element SelectSingleElement(string TagName, string AttribName, string AttribValue)
		{
			return this._SelectElement(this, TagName, AttribName, AttribValue);
		}

		public Element SelectSingleElement(string TagName, string ns)
		{
			//return this._SelectElement(this, TagName, "xmlns", ns);
            return this._SelectElement(this, TagName, ns, true);
		}

        public Element SelectSingleElement(string TagName, string ns, bool traverseChildren)
        {            
            return this._SelectElement(this, TagName, ns, traverseChildren);
        }

#if NET_2
        public T SelectSingleElement<T>() where T : Element
        {
            return (T)this._SelectElement(this, typeof(T));
        }

        public T SelectSingleElement<T>(bool traverseChildren) where T : Element
        {
            return (T)this._SelectElement(this, typeof(T), traverseChildren);
        } 
        
#endif
        //public Element Element(string name)
        //{
        //    return SelectSingleElement(name);
        //}

        //public T Element<T>() where T : Element
        //{
        //    return (T)this._SelectElement(this, typeof(T));
        //}

		/// <summary>
		/// Returns all childNodes with the given Tagname,
		/// this function doesn't traverse the whole tree!!!
		/// </summary>
		/// <param name="TagName"></param>
		/// <returns></returns>
		public ElementList SelectElements(string TagName)
		{
            ElementList es = new ElementList();
			//return this._SelectElements(this, TagName, es);
			return this._SelectElements(this, TagName, es, false);
		}

        public ElementList SelectElements(string TagName, bool traverseChildren)
        {
            ElementList es = new ElementList();
            //return this._SelectElements(this, TagName, es);
            return this._SelectElements(this, TagName, es, traverseChildren);
        }

        public ElementList SelectElements(System.Type type)
		{
            ElementList es = new ElementList();
			return this._SelectElements(this, type, es);
		}

		/// <summary>
		/// returns a nodelist of all found nodes of the given Type
		/// </summary>
		/// <param name="e"></param>
		/// <param name="type"></param>
		/// <param name="es"></param>
		/// <returns></returns>
        private ElementList _SelectElements(Element e, System.Type type, ElementList es)
		{			
			return _SelectElements(e, type, es, false);	
		}

        private ElementList _SelectElements(Element e, System.Type type, ElementList es, bool traverseChildren)
		{						
			if (e.ChildNodes.Count > 0) 
			{		
				foreach(Node n in e.ChildNodes) 
				{
					if (n.NodeType == NodeType.Element)
					{
                        if (n.GetType() == type)                        
						{
							es.Add(n);
						}
						if (traverseChildren)
							_SelectElements((Element) n, type, es, true);
					}						            
				}
			}								
			return es;
		}

		/// <summary>
		/// Select a single Element.
		/// This function doesnt traverse the whole tree and checks only the underlying childnodes
		/// </summary>
		/// <param name="se"></param>
		/// <param name="tagname"></param>
		/// <returns></returns>
		private Element _SelectElement(Node se, string tagname) 
		{
			return _SelectElement(se, tagname, false);
		}

		/// <summary>
		/// Select a single Elemnt
		/// </summary>
		/// <param name="se"></param>
		/// <param name="tagname"></param>
		/// <param name="traverseChildren">when set to true then the function traverses the whole tree</param>
		/// <returns></returns>
		private Element _SelectElement(Node se, string tagname, bool traverseChildren) 
		{
			Element rElement = null;			
					
			if (se.ChildNodes.Count > 0) 
			{
				foreach(Node ch in se.ChildNodes) 
				{
					if (ch.NodeType == NodeType.Element)
					{
						if ( ((Element) ch).TagName == tagname )
						{
							rElement = (Element) ch;
							return rElement;
						}
						else
						{
							if( traverseChildren)
							{
								rElement = _SelectElement(ch, tagname, true);
								if (rElement != null)
									break;
							}
						}
					}
				}				
			}    
						
			return rElement;
		}


		private Element _SelectElement(Node se, System.Type type)
		{
			return _SelectElement(se, type, false);
		}

		private Element _SelectElement(Node se, System.Type type, bool traverseChildren)
		{
			Element rElement = null;			
					
			if (se.ChildNodes.Count > 0) 
			{
				foreach(Node ch in se.ChildNodes) 
				{                    
					if (ch.NodeType == NodeType.Element)
					{                        
						if ( ch.GetType() == type )
						{
							rElement = (Element) ch;
							return rElement;
						}
						else
						{
							if( traverseChildren)
							{
								rElement = _SelectElement(ch, type, true);
								if (rElement != null)
									break;
							}
						}
					}
				}				
			}    						
			return rElement;
		}

		private Element _SelectElement(Node se, string tagname, string AttribName, string AttribValue) 
		{
			Element rElement = null;
						
			if (se.NodeType == NodeType.Element)
			{
				Element e = se as Element;
				if (e.m_TagName == tagname)
				{
					if (e.HasAttribute(AttribName))
					{
						if (e.GetAttribute(AttribName) == AttribValue)
						{
							rElement = e;
							return rElement;
						}
					}
				}
			}		
	
			if (se.ChildNodes.Count > 0) 
			{
				foreach(Node ch in se.ChildNodes) 
				{
					rElement = _SelectElement(ch, tagname, AttribName, AttribValue);            
					if (rElement != null)
						break;
				}				
			}    		
			
			return rElement;
		}

        /// <summary>
        /// Find Element by Namespace
        /// </summary>
        /// <param name="se"></param>
        /// <param name="tagname"></param>
        /// <param name="AttribName"></param>
        /// <param name="AttribValue"></param>
        /// <returns></returns>
        private Element _SelectElement(Node se, string tagname, string nameSpace, bool traverseChildren)
        {
            Element rElement = null;

            if (se.ChildNodes.Count > 0)
            {
                foreach (Node ch in se.ChildNodes)
                {
                    if (ch.NodeType == NodeType.Element)
                    {
                        Element e = ch as Element;
                        if (e.TagName == tagname && e.Namespace == nameSpace)
                        {
                            rElement = (Element)ch;
                            return rElement;
                        }
                        else
                        {
                            if (traverseChildren)
                            {
                                rElement = _SelectElement(ch, tagname, nameSpace, traverseChildren);
                                if (rElement != null)
                                    break;
                            }
                        }
                    }
                }
            }
            return rElement;
        }

        private ElementList _SelectElements(Element e, string tagname, ElementList es, bool traverseChildren) 
		{
			if (e.ChildNodes.Count > 0) 
			{		
				foreach(Node n in e.ChildNodes) 
				{
					if (n.NodeType == NodeType.Element)
					{
						if ( ((Element) n).m_TagName == tagname)
						{
							es.Add(n);
						}
						if (traverseChildren)
							_SelectElements((Element) n, tagname, es, true);
					}
									
				}
			}
			return es;
        }

#if NET_2
        public List<T> SelectElements<T>() where T : Element
        {
            return SelectElements<T>(false);
        }

        public List<T> SelectElements<T>(bool traverseChildren) where T : Element
        {
            List<T> list = new List<T>();
            return this._SelectElements<T>(this, list, traverseChildren);
        }

        private List<T> _SelectElements<T>(Element e, List<T> list, bool traverseChildren) where T : Element
        {
            if (e.ChildNodes.Count > 0)
            {
                foreach (Node n in e.ChildNodes)
                {
                    if (n.NodeType == NodeType.Element)
                    {
                        if (n.GetType() == typeof(T))
                        {
                            list.Add(n as T);
                        }
                        if (traverseChildren)
                            _SelectElements((Element)n, list, true);
                    }
                }
            }
            return list;
        }
#endif

        #endregion
    }
}