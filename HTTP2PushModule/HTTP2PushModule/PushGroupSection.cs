using System.Configuration;

namespace HTTP2PushModule
{
    class PushGroupSection : ConfigurationSection
    {
        [ConfigurationProperty("", IsDefaultCollection = true)]
        public PushGroupCollection PushGroup => (PushGroupCollection)this[""];
    }

    public class PushGroupCollection : ConfigurationElementCollection
    {
        public PushGroupElement this[object key] => BaseGet(key) as PushGroupElement;

        public override ConfigurationElementCollectionType CollectionType => ConfigurationElementCollectionType.BasicMap;

        protected override string ElementName => "PushGroup";

        protected override ConfigurationElement CreateNewElement() => new PushGroupElement();

        protected override object GetElementKey(ConfigurationElement element) => ((PushGroupElement)element);
    }

    public class PushGroupElement : ConfigurationElement
    {
        [ConfigurationProperty("", IsDefaultCollection = true)]
        public PushElementCollection PushElements
        {
            get { return (PushElementCollection)base[""]; }
        }

        [ConfigurationProperty("name", IsRequired = true, IsKey = true, DefaultValue = "")]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

    }

    public class PushElementCollection : ConfigurationElementCollection
    {
        public PushElement this[object key] => BaseGet(key) as PushElement;

        public override ConfigurationElementCollectionType CollectionType => ConfigurationElementCollectionType.BasicMap;

        protected override string ElementName => "PushElement";

        protected override ConfigurationElement CreateNewElement() => new PushElement();

        protected override object GetElementKey(ConfigurationElement element) => ((PushElement)element);
    }

    public class PushElement : ConfigurationElement
    {
        [ConfigurationProperty("url", IsRequired = true, IsKey = true, DefaultValue = "")]
        public string Url
        {
            get { return (string)this["url"]; }
            set { this["url"] = value; }
        }

        [ConfigurationProperty("triggers", IsKey = true, DefaultValue = false)]
        public bool Triggers
        {
            get { return (bool)this["triggers"]; }
            set { this["triggers"] = value; }
        }

    }
}
