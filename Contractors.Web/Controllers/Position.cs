using System;
using System.Xml.Serialization;

namespace Contractors.Web.Controllers
{
    [XmlRoot("position")]
    public class Position
    {
        [XmlElement("id")]
        public string Id { get; set; }
        [XmlElement("title")]
        public string Title { get; set; }
        [XmlElement("summary")]
        public string Summary { get; set; }
        [XmlElement("start-date")]
        public DateTime StartDate { get; set; }
        [XmlElement("is-current")]
        public bool IsCurrent { get; set; }
        [XmlElement("end-date", IsNullable = true)]
        public DateTime? EndDate { get; set; }
        [XmlElement("company")]
        public PositionCompany Company { get; set; }
    }
}