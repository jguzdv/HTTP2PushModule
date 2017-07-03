using System.Configuration;

namespace ZDV.HTTP2PushModule.Configuration
{
    /// <summary>
    /// The actual PushGroup - has a name to be user friendly and a list of PushElements
    /// </summary>
    public class PushGroupElement : ConfigurationElement
    {
        [ConfigurationProperty("", IsDefaultCollection = true)]
        public PushElementCollection PushElements => (PushElementCollection)base[""];

        /// <summary>
        /// Name to be human-readable
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true, IsKey = true, DefaultValue = "")]
        public string Name
        {
            get => (string)this["name"];
            set => this["name"] = value;
        }
    }

    
    /// <summary>
    /// A PushElement is part of a PushGroup and can be triggering or not.
    /// A triggering PushElement, will create push promises for all other elements of the PushGroup.
    /// </summary>
    public class PushElement : ConfigurationElement
    {
        /// <summary>
        /// Application Relative path to the file, that should be part of the push promise.
        /// </summary>
        [ConfigurationProperty("url", IsRequired = true, IsKey = true, DefaultValue = "")]
        public string Url
        {
            get => (string)this["url"];
            set => this["url"] = value;
        }

        /// <summary>
        /// True if a request to the file should trigger the push-promise group
        /// </summary>
        [ConfigurationProperty("triggers", IsKey = true, DefaultValue = false)]
        public bool Triggers
        {
            get => (bool)this["triggers"];
            set => this["triggers"] = value;
        }
    }

    #region Configuration Infrastructure
    /// <summary>
    /// Root element of PushGroup Configuration.
    /// </summary>
    class PushGroupSection : ConfigurationSection
    {
        [ConfigurationProperty("", IsDefaultCollection = true)]
        public PushGroupCollection PushGroup => (PushGroupCollection)this[""];
    }

    /// <summary>
    /// Collection of PushGroups. Needed by configuration infrastructure
    /// </summary>
    public class PushGroupCollection : ConfigurationElementCollection
    {
        public PushGroupElement this[object key] => BaseGet(key) as PushGroupElement;

        public override ConfigurationElementCollectionType CollectionType => ConfigurationElementCollectionType.BasicMap;

        protected override string ElementName => "PushGroup";

        protected override ConfigurationElement CreateNewElement() => new PushGroupElement();

        protected override object GetElementKey(ConfigurationElement element) => (PushGroupElement)element;
    }

    /// <summary>
    /// PushElement Collection, needed by configuration infrastructure
    /// </summary>
    public class PushElementCollection : ConfigurationElementCollection
    {
        public PushElement this[object key] => BaseGet(key) as PushElement;

        public override ConfigurationElementCollectionType CollectionType => ConfigurationElementCollectionType.BasicMap;

        protected override string ElementName => "PushElement";

        protected override ConfigurationElement CreateNewElement() => new PushElement();

        protected override object GetElementKey(ConfigurationElement element) => (PushElement)element;
    }
    #endregion
}
