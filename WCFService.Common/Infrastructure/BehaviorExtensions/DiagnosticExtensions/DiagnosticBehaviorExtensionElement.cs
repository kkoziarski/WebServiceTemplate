namespace WCFService.Common.Infrastructure.BehaviorExtensions.DiagnosticExtensions
{
    using System;
    using System.Configuration;
    using System.ServiceModel.Configuration;

    public abstract class DiagnosticBehaviorExtensionElementBase<TServiceMessageInspector, TDiagnosticServiceBehavior> : BehaviorExtensionElement 
        where TDiagnosticServiceBehavior : DiagnosticServiceBehaviorBase<TServiceMessageInspector> 
        where TServiceMessageInspector : ServiceMessageInspectorBase
    {
        public override Type BehaviorType
        {
            get
            {
                return typeof(TDiagnosticServiceBehavior);
            }
        }

        protected override object CreateBehavior()
        {
            return this.ServiceBehaviorFactory(this.LogRequest, this.LogReply);
        }

        protected abstract TDiagnosticServiceBehavior ServiceBehaviorFactory(bool inspectRequest, bool inspectReply);

        [ConfigurationProperty("logRequest", DefaultValue = false, IsRequired = false)]
        public bool LogRequest
        {
            get { return (bool)base["logRequest"]; }
            set { base["logRequest"] = value; }
        }

        [ConfigurationProperty("logReply", DefaultValue = false, IsRequired = false)]
        public bool LogReply
        {
            get { return (bool)base["logReply"]; }
            set { base["logReply"] = value; }
        }

    }

    // Define the SchemasCollection that will contain the SchemaConfigElement
    // elements.
    public class SchemasCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new SchemaConfigElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((SchemaConfigElement)element).Location;
        }

        public SchemaConfigElement this[int index]
        {
            get
            {
                return (SchemaConfigElement)this.BaseGet(index);
            }
            set
            {
                if (this.BaseGet(index) != null)
                {
                    this.BaseRemoveAt(index);
                }
                this.BaseAdd(index, value);
            }
        }

        new public SchemaConfigElement this[string Name]
        {
            get
            {
                return (SchemaConfigElement)this.BaseGet(Name);
            }
        }

        public int IndexOf(SchemaConfigElement Schema)
        {
            return this.BaseIndexOf(Schema);
        }

        public void Add(SchemaConfigElement Schema)
        {
            this.BaseAdd(Schema);
        }
        protected override void BaseAdd(ConfigurationElement element)
        {
            this.BaseAdd(element, false);
        }

        public void Remove(SchemaConfigElement Schema)
        {
            if (this.BaseIndexOf(Schema) >= 0)
                this.BaseRemove(Schema.Location);
        }

        public void RemoveAt(int index)
        {
            this.BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            this.BaseRemove(name);
        }

        public void Clear()
        {
            this.BaseClear();
        }
    }

    public class SchemaConfigElement : ConfigurationElement
    {
        public SchemaConfigElement(String location)
        {
            this.Location = location;
        }

        public SchemaConfigElement()
        {
        }

        [ConfigurationProperty("location", DefaultValue = null, IsRequired = true, IsKey = false)]
        public string Location
        {
            get
            {
                return (string)this["location"];
            }
            set
            {
                this["location"] = value;
            }
        }
    }
}