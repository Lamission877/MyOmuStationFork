namespace Content.Shared.System;

public sealed class PoisonPenSystem : EntitySystem
{
    [Dependency] private readonly SharedSolutionContainerSystem _solutionContainer = default!;

    public override void Initialize()
        {
            base.Initialize();

            SubscribeLocalEvent<PoisonPenComponent, AfterInteractEvent>(AfterInteract);

        }

    private void AfterInteract(Entity<PoisonPenComponent> ent, AfterInteractEvent args)
    {
        if(TryComp<PaperComponent>(args.Target, out var paper))
        {
            return;
        }
    }
}
