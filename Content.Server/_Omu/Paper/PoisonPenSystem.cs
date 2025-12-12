using Content.Shared.Chemistry.EntitySystems;
using Content.Server.Chemistry.Components;
using Content.Shared.Chemistry.Components.SolutionManager;
using Content.Shared.Paper;
using Content.Shared.Interaction;
using Content.Shared.Tag;
using Content.Shared.Popups;
using Robust.Shared.Prototypes;


namespace Content.Server.Paper;

public sealed class PoisonPenSystem : EntitySystem
{
    [Dependency] private readonly SharedSolutionContainerSystem _solutionContainer = default!;
    [Dependency] private readonly TagSystem _tag = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;

    private static readonly ProtoId<TagPrototype> PaperTag = "Paper";
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<PoisonPenComponent, PaperWriteEvent>(OnWrite);
        SubscribeLocalEvent<PoisonPenComponent, ComponentInit>(OnCompInit);
    }

    private void OnCompInit(Entity<PoisonPenComponent> ent, ref ComponentInit args)
    {
        EnsureComp<SolutionContainerManagerComponent>(ent.Owner);
    }


    private void OnWrite(Entity<PoisonPenComponent> ent, ref PaperWriteEvent args)
    {
        if (TryComp<SolutionInjectOnPickupComponent>(args.Paper, out var injectorComp) && _solutionContainer.TryGetSolution(ent.Owner, "injector" , out var injector) && _solutionContainer.TryGetSolution(args.Paper, injectorComp.Solution, out var container1)) //previously poisoned
        {
            var total = container1.Value.Comp.Solution.Volume + injectorComp.TransferAmount;

            var transferredSol = _solutionContainer.SplitSolution(injector.Value, total > container1.Value.Comp.Solution.MaxVolume ? container1.Value.Comp.Solution.AvailableVolume : injectorComp.TransferAmount);
            container1.Value.Comp.Solution.AddSolution(transferredSol, _prototypeManager);
            _popup.PopupClient("You dose the paper with the solution.", args.User, args.User);
        }
        else
        {
            EnsureComp<SolutionInjectOnPickupComponent>(args.Paper, out var injector2);
            injector2.TransferAmount = ent.Comp.TransferAmount;
            injector2.Solution = ent.Comp.Solution;
            if (_solutionContainer.TryGetSolution(args.Paper, injector2.Solution, out var containerInit) &&  _solutionContainer.TryGetSolution(ent.Owner, "injector" , out var injectorInit))
            {
                var total = containerInit.Value.Comp.Solution.Volume + injector2.TransferAmount;

                var transferredSol = _solutionContainer.SplitSolution(injectorInit.Value, total > containerInit.Value.Comp.Solution.MaxVolume ? containerInit.Value.Comp.Solution.AvailableVolume : injector2.TransferAmount);
                containerInit.Value.Comp.Solution.AddSolution(transferredSol, _prototypeManager);
                _popup.PopupClient("You dose the paper with the solution.", args.User, args.User);
            }
            else
                _popup.PopupClient("What the fuck??", args.User, args.User);
        }
    }
}
