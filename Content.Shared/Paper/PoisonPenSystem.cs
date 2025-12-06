namespace Content.Shared.System;
namespace Content.Server.Chemistry.EntitySystems;

public sealed class PoisonPenSystem : EntitySystem
{
    [Dependency] private readonly SharedSolutionContainerSystem _solutionContainer = default!;
    [Dependency] private readonly TagSystem _tag = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<PoisonPenComponent, AfterInteractEvent>(AfterInteract);
        SubscribeLocalEvent<PoisonPenComponent, ComponentInit>(OnCompInit);
    }

    private void OnCompInit(Entity<PoisonPenComponent> ent, ref ComponentInit Args)
    {
        EnsureComp<SolutionContainerComponent>(ent.Owner);
        EnsureComp<InjectorComponent>(ent.Owner);
    }


    private void AfterInteract(Entity<PoisonPenComponent> ent, AfterInteractEvent args)
    {
        if(!_tag.HasTag(args.target, "Paper")) //Checks if it's a paper
        {
            return;
        }


        if(TryComp<SolutionInjectOnPickupComponent>(args.Target, out var poison))
        {
            TryUseInjector(ent, args.target, args.user);
        }
    }
}
