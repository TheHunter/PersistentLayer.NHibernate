using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Cfg;
using System.Xml;
using System.IO;
using NHibernate.Mapping;

namespace PersistentLayer.NHibernate
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class NhConfigurationBuilder
    {
        private Configuration config;
        private ISessionFactory sessionFactory;

        /// <summary>
        /// 
        /// </summary>
        protected NhConfigurationBuilder() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileCfg"></param>
        /// <param name="dirMappingFiles"></param>
        public NhConfigurationBuilder(string fileCfg, string dirMappingFiles)
        {
            this.Config = new Configuration()
                            .Configure(fileCfg);

            this.Config.AddDirectory(new DirectoryInfo(dirMappingFiles));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileCfg"></param>
        /// <param name="mappingFiles"></param>
        public NhConfigurationBuilder(string fileCfg, IEnumerable<string> mappingFiles)
        {
            this.Config = new Configuration()
                            .Configure(fileCfg);

            mappingFiles.All
                (
                    xmlfile =>
                        {
                            this.Config.AddXmlFile(xmlfile);
                            return true;
                        }
                );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileCfg"></param>
        /// <param name="directory"></param>
        public NhConfigurationBuilder(XmlReader fileCfg, DirectoryInfo directory)
        {
            this.Config = new Configuration()
                            .Configure(fileCfg);

            //this.Config.SetCacheConcurrencyStrategy
            //this.Config.SetCollectionCacheConcurrencyStrategy();
            
            this.Config.AddDirectory(directory);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cfg"></param>
        public NhConfigurationBuilder(Configuration cfg)
        {
            this.Config = cfg;
        }

        /// <summary>
        /// Sets the value of the Configuration property.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        private void OverrideProperty(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("property name cannot be empty and null.");

            if (string.IsNullOrEmpty(value))
                throw new ArgumentException(string.Format("property value cannot be empty or null, property name: {0}", name));
            
            this.Config.SetProperty(name, value);
        }

        /// <summary>
        /// Sets the value of the Configuration property.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetProperty(string name, string value)
        {
            if (this.sessionFactory == null)
                this.OverrideProperty(name, value);
        }

        /// <summary>
        /// Sets the values of the Configuration properties.
        /// </summary>
        /// <param name="properties"></param>
        public void SetProperties(IDictionary<string, string> properties)
        {
            if (this.sessionFactory == null && properties != null && properties.Count > 0)
                properties.All
                    (
                        current =>
                            {
                                this.OverrideProperty(current.Key, current.Value);
                                return true;
                            }
                    );
        }

        /// <summary>
        /// Sets the default interceptor object which be used by all session created by the SessionFactory.
        /// </summary>
        /// <param name="defaultInterceptor"></param>
        public void SetInterceptor(IInterceptor defaultInterceptor)
        {
            if (defaultInterceptor != null)
                this.Config.SetInterceptor(defaultInterceptor);
        }

        /// <summary>
        /// 
        /// </summary>
        public void BuildSessionFactory()
        {
            if (this.sessionFactory == null)
                this.sessionFactory = this.Config.BuildSessionFactory();
        }

        /// <summary>
        /// 
        /// </summary>
        public bool HasClassMappings
        {
            get
            {
                return this.config.ClassMappings.Count > 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Configuration Config
        {
            get
            {
                return this.config;
            }
            private set
            {
                this.config = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ISessionFactory SessionFactory
        {
            get { return this.sessionFactory; }
        }
    }
}
