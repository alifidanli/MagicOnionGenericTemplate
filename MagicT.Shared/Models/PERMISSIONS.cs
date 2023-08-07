using System.ComponentModel.DataAnnotations.Schema;
using Generator.Equals;
using MagicT.Shared.Models.Base;
using MemoryPack;

namespace MagicT.Shared.Models;

[Equatable]
[MemoryPackable]
[Table(nameof(PERMISSIONS))]
// ReSharper disable once PartialTypeWithSinglePart
public partial class PERMISSIONS:AUTHORIZATIONS_BASE
{
    public PERMISSIONS() =>AB_AUTH_TYPE = nameof(PERMISSIONS);
    
}