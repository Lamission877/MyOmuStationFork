using Content.Shared.Chemistry.EntitySystems;
using Content.Server.Chemistry.EntitySystems;
using Content.Server.Chemistry.Components;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Components.SolutionManager;
using Content.Shared.Interaction;
using Content.Shared.Tag;

namespace Content.Server.Paper;


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

    private void OnCompInit(Entity<PoisonPenComponent> ent, ref ComponentInit args)
    {
        EnsureComp<SolutionContainerManagerComponent>(ent.Owner);
        EnsureComp<InjectorComponent>(ent.Owner);
    }


    private void AfterInteract(Entity<PoisonPenComponent> ent, ref AfterInteractEvent args)
    {
        if(args.Target == null) //It exists
        {
            return;
        }
        EntityUid target = args.Target.Value;
        if(!_tag.HasTag(target, "Paper")) //Checks if it's a paper
        {
            return;
        }


        if(TryComp<SolutionInjectOnPickupComponent>(args.Target, out var poison))
        {
        }
    }
}
