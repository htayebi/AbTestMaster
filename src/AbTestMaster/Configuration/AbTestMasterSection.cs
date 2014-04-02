using System;
using System.Configuration;

namespace AbTestMaster.Configuration
{
    public class AbTestMasterSection : ConfigurationSection
    {
        [ConfigurationProperty("targets", IsDefaultCollection = false)]
        public TargetCollection Targets
        {
            get { return (TargetCollection)base["targets"]; }
        }
    }

    public class TargetCollection : ConfigurationElementCollection
    {
        [ConfigurationProperty("throwexceptions", DefaultValue = false)]
        public bool ThrowExceptions
        {
            get { return (bool)base["throwexceptions"]; }
        }
        public TargetCollection()
        {
            var details = (TargetElement)CreateNewElement();
            if (details.Name != "")
            {
                Add(details);
            }
        }
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }
        protected override ConfigurationElement CreateNewElement()
        {
            return new TargetElement();
        }
        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((TargetElement)element).Name;
        }
        public TargetElement this[int index]
        {
            get
            {
                return (TargetElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }
        new public TargetElement this[string name]
        {
            get
            {
                return (TargetElement)BaseGet(name);
            }
        }
        public int IndexOf(TargetElement details)
        {
            return BaseIndexOf(details);
        }
        public void Add(TargetElement details)
        {
            BaseAdd(details);
        }
        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }
        public void Remove(TargetElement details)
        {
            if (BaseIndexOf(details) >= 0)
                BaseRemove(details.Name);
        }
        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }
        public void Remove(string name)
        {
            BaseRemove(name);
        }
        public void Clear()
        {
            BaseClear();
        }
        protected override string ElementName
        {
            get { return "target"; }
        }
    }

    public class TargetElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true, IsKey = true, DefaultValue = "")]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("type", IsRequired = true)]
        public string Type
        {
            get { return (string)this["type"]; }
            set { this["type"] = value; }
        }

        [ConfigurationProperty("data", IsRequired = true)]
        public string Data
        {
            get { return (string)this["data"]; }
            set { this["data"] = value; }
        }

        [ConfigurationProperty("connectionStringName")]
        public string ConnectionStringName
        {
            get { return (string)this["connectionStringName"]; }
            set { this["connectionStringName"] = value; }
        }

        [ConfigurationProperty("commandText")]
        public string CommandText
        {
            get { return (string)this["commandText"]; }
            set { this["commandText"] = value; }
        }

        [ConfigurationProperty("path")]
        public string Path
        {
            get { return (string)this["path"]; }
            set { this["path"] = value; }
        }

        [ConfigurationProperty("", IsDefaultCollection = true)]
        public ParameterCollection Parameters
        {
            get { return (ParameterCollection)base[""]; }

        }
    }

    public class ParameterCollection : ConfigurationElementCollection
    {
        public new ParameterElement this[string name]
        {
            get
            {
                if (IndexOf(name) < 0) return null;
                return (ParameterElement)BaseGet(name);
            }
        }
        public ParameterElement this[int index]
        {
            get { return (ParameterElement)BaseGet(index); }
        }
        public int IndexOf(string name)
        {
            name = name.ToLower();
            for (int idx = 0; idx < base.Count; idx++)
            {
                if (this[idx].Name.ToLower() == name)
                    return idx;
            }
            return -1;
        }
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }
        protected override ConfigurationElement CreateNewElement()
        {
            return new ParameterElement();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ParameterElement)element).Name;
        }
        protected override string ElementName
        {
            get { return "parameter"; }
        }
    }

    public class ParameterElement : ConfigurationElement
    {
        public ParameterElement()
        {
        }
        public ParameterElement(string name, string value)
        {
            this.Value = value;
            this.Name = name;
        }

        [ConfigurationProperty("name", IsRequired = true, IsKey = true, DefaultValue = "")]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("value", IsRequired = true)]
        public string Value
        {
            get { return (string)this["value"]; }
            set { this["value"] = value; }
        }
    }
}
