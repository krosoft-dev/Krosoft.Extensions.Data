using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Krosoft.Extensions.Samples.Library.Models.Enums;

public enum SampleCode
{
    [Description("Description for Value1")]
    [Display(Name = "Display Name for Value1")]
    One = 1,

    [Description("Description for Value2")]
    [Display(Name = "Display Name for Value2")]
    Two = 2,

    [Description("Description for Value3")]
    [Display(Name = "Display Name for Value3")]
    Three = 4,

    [Description("Description for Value4")]
    [Display(Name = "Display Name for Value4")]
    Four = 8,

    Five = 16
}