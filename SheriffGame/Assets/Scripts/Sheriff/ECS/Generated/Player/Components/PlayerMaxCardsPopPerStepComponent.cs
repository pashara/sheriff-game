//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class PlayerEntity {

    public Sheriff.ECS.Components.MaxCardsPopPerStepComponent maxCardsPopPerStep { get { return (Sheriff.ECS.Components.MaxCardsPopPerStepComponent)GetComponent(PlayerComponentsLookup.MaxCardsPopPerStep); } }
    public bool hasMaxCardsPopPerStep { get { return HasComponent(PlayerComponentsLookup.MaxCardsPopPerStep); } }

    public void AddMaxCardsPopPerStep(int newCount) {
        var index = PlayerComponentsLookup.MaxCardsPopPerStep;
        var component = (Sheriff.ECS.Components.MaxCardsPopPerStepComponent)CreateComponent(index, typeof(Sheriff.ECS.Components.MaxCardsPopPerStepComponent));
        component.Count = newCount;
        AddComponent(index, component);
    }

    public void ReplaceMaxCardsPopPerStep(int newCount) {
        var index = PlayerComponentsLookup.MaxCardsPopPerStep;
        var component = (Sheriff.ECS.Components.MaxCardsPopPerStepComponent)CreateComponent(index, typeof(Sheriff.ECS.Components.MaxCardsPopPerStepComponent));
        component.Count = newCount;
        ReplaceComponent(index, component);
    }

    public void RemoveMaxCardsPopPerStep() {
        RemoveComponent(PlayerComponentsLookup.MaxCardsPopPerStep);
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class PlayerMatcher {

    static Entitas.IMatcher<PlayerEntity> _matcherMaxCardsPopPerStep;

    public static Entitas.IMatcher<PlayerEntity> MaxCardsPopPerStep {
        get {
            if (_matcherMaxCardsPopPerStep == null) {
                var matcher = (Entitas.Matcher<PlayerEntity>)Entitas.Matcher<PlayerEntity>.AllOf(PlayerComponentsLookup.MaxCardsPopPerStep);
                matcher.componentNames = PlayerComponentsLookup.componentNames;
                _matcherMaxCardsPopPerStep = matcher;
            }

            return _matcherMaxCardsPopPerStep;
        }
    }
}