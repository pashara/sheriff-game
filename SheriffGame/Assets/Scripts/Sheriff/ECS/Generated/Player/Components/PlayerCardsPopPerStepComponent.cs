//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class PlayerEntity {

    public Sheriff.ECS.Components.CardsPopPerStepComponent cardsPopPerStep { get { return (Sheriff.ECS.Components.CardsPopPerStepComponent)GetComponent(PlayerComponentsLookup.CardsPopPerStep); } }
    public bool hasCardsPopPerStep { get { return HasComponent(PlayerComponentsLookup.CardsPopPerStep); } }

    public void AddCardsPopPerStep(int newCount) {
        var index = PlayerComponentsLookup.CardsPopPerStep;
        var component = (Sheriff.ECS.Components.CardsPopPerStepComponent)CreateComponent(index, typeof(Sheriff.ECS.Components.CardsPopPerStepComponent));
        component.Count = newCount;
        AddComponent(index, component);
    }

    public void ReplaceCardsPopPerStep(int newCount) {
        var index = PlayerComponentsLookup.CardsPopPerStep;
        var component = (Sheriff.ECS.Components.CardsPopPerStepComponent)CreateComponent(index, typeof(Sheriff.ECS.Components.CardsPopPerStepComponent));
        component.Count = newCount;
        ReplaceComponent(index, component);
    }

    public void RemoveCardsPopPerStep() {
        RemoveComponent(PlayerComponentsLookup.CardsPopPerStep);
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

    static Entitas.IMatcher<PlayerEntity> _matcherCardsPopPerStep;

    public static Entitas.IMatcher<PlayerEntity> CardsPopPerStep {
        get {
            if (_matcherCardsPopPerStep == null) {
                var matcher = (Entitas.Matcher<PlayerEntity>)Entitas.Matcher<PlayerEntity>.AllOf(PlayerComponentsLookup.CardsPopPerStep);
                matcher.componentNames = PlayerComponentsLookup.componentNames;
                _matcherCardsPopPerStep = matcher;
            }

            return _matcherCardsPopPerStep;
        }
    }
}
