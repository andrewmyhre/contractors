using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Contractors.Web.Code
{
    public interface IRavenDbConfigurationSection
    {
        [ConfigurationProperty("storageType")]
        StorageTypeEnum StorageType { get; }

        [ConfigurationProperty("location")]
        string Location { get; }
    }

    public class RavenDbConfigurationSection : ConfigurationSection, IRavenDbConfigurationSection
    {
        [ConfigurationProperty("storageType", DefaultValue=StorageTypeEnum.Http)]
        public StorageTypeEnum StorageType
        {
            get { return (StorageTypeEnum) this["storageType"]; }
        }

        [ConfigurationProperty("location", DefaultValue="http://localhost:8080")]
        public string Location
        {
            get { return (string) this["location"]; }
        }
    }

    public enum StorageTypeEnum
    {
        Http,
        Embedded
    }
}