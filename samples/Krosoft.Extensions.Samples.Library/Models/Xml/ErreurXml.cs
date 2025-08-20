using System.Xml.Serialization;

namespace Krosoft.Extensions.Samples.Library.Models.Xml;

public record ErreurXml
{
    [XmlAttribute("errorMessage")]
    public string? Message { get; set; }
}