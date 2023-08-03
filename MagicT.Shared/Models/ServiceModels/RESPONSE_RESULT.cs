using MemoryPack;

namespace MagicT.Shared.Models.ServiceModels;

[MemoryPackable]
// ReSharper disable once InconsistentNaming
// ReSharper disable once PartialTypeWithSinglePart
public partial class RESPONSE_RESULT<TModel>
{
    public TModel Data { get; set; } = default!;

    public RESPONSE_RESULT()
    {

    }

    [MemoryPackConstructor]
    public RESPONSE_RESULT(TModel data)
    {
        Data = data;
    }
}