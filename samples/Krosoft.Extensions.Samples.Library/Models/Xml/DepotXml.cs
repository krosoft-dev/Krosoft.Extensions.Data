using System.Xml.Serialization;

namespace Krosoft.Extensions.Samples.Library.Models.Xml;

[XmlRoot("header", Namespace = "mfp:anaf:dgti:spv:respUploadFisier:v1")]
public record DepotXml
{
    [XmlAttribute("dateResponse")]
    public string? DateResponse { get; set; }

    [XmlAttribute("ExecutionStatus")]
    public string? ExecutionStatus { get; set; }

    [XmlAttribute("index_incarcare")]
    public string? NumeroFluxDepot { get; set; }

    [XmlElement("Errors")]
    public List<ErreurXml> Errors { get; set; } = new List<ErreurXml>();
}