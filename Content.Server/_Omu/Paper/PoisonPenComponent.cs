using Content.Goobstation.Maths.FixedPoint;
using Robust.Shared.Serialization;

namespace Content.Server.Paper;

[RegisterComponent]
public sealed partial class PoisonPenComponent : Component
{
    [DataField]
    public FixedPoint2 MaxVolume = 15;

    [DataField]
    public FixedPoint2 TransferAmount = 15;

    [DataField]
    public string Solution = "poison";


}
