using System.Xml.Serialization;

namespace Contractors.Web.Controllers
{
    [XmlRoot("company")]
    public class PositionCompany
    {
        [XmlElement("id")]
        public string Id { get; set; }
        [XmlElement("name")]
        public string Name { get; set; }
        [XmlElement("type")]
        public string CompanyType { get; set; }
        [XmlElement("industry")]
        public string Industry { get; set; }
    }
}