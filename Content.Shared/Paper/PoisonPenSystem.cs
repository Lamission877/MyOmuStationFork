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
        if(HasComp<PaperComponent>(args.Target))
        {
            return;
        }
        if(TryComp<SolutionInjectOnPickupComponent(args.Target, out var poison))
        {
            _solutionContainer.TryGetSolution(ent.Owner, ent.Comp.Solution, out var solution);
        }
    }
}
