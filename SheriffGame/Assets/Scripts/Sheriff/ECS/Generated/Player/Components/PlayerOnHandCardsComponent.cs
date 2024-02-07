//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class PlayerEntity {

    public Sheriff.ECS.Components.OnHandCardsComponent onHandCards { get { return (Sheriff.ECS.Components.OnHandCardsComponent)GetComponent(PlayerComponentsLookup.OnHandCards); } }
    public bool hasOnHandCards { get { return HasComponent(PlayerComponentsLookup.OnHandCards); } }

    public void AddOnHandCards(System.Collections.Generic.List<Sheriff.ECS.Components.CardEntityId> newValue) {
        var index = PlayerComponentsLookup.OnHandCards;
        var component = (Sheriff.ECS.Components.OnHandCardsComponent)CreateComponent(index, typeof(Sheriff.ECS.Components.OnHandCardsComponent));
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceOnHandCards(System.Collections.Generic.List<Sheriff.ECS.Components.CardEntityId> newValue) {
        var index = PlayerComponentsLookup.OnHandCards;
        var component = (Sheriff.ECS.Components.OnHandCardsComponent)CreateComponent(index, typeof(Sheriff.ECS.Components.OnHandCardsComponent));
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveOnHandCards() {
        RemoveComponent(PlayerComponentsLookup.OnHandCards);
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

    static Entitas.IMatcher<PlayerEntity> _matcherOnHandCards;

    public static Entitas.IMatcher<PlayerEntity> OnHandCards {
        get {
            if (_matcherOnHandCards == null) {
                var matcher = (Entitas.Matcher<PlayerEntity>)Entitas.Matcher<PlayerEntity>.AllOf(PlayerComponentsLookup.OnHandCards);
                matcher.componentNames = PlayerComponentsLookup.componentNames;
                _matcherOnHandCards = matcher;
            }

            return _matcherOnHandCards;
        }
    }
}