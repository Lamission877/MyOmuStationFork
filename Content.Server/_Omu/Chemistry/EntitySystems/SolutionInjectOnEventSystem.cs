using Content.Server.Chemistry.Components;
using Content.Shared.Hands; // Omu

namespace Content.Server.Chemistry.EntitySystems;

public sealed partial class SolutionInjectOnCollideSystem {

    private void HandlePickup(Entity<SolutionInjectOnPickupComponent> entity, ref GotEquippedHandEvent args)
    {
        DoInjection((entity.Owner, entity.Comp), args.User);
    }

}
