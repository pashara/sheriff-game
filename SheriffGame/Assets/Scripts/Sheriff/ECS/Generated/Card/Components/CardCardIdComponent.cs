//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class CardEntity {

    public Sheriff.ECS.Components.CardIdComponent cardId { get { return (Sheriff.ECS.Components.CardIdComponent)GetComponent(CardComponentsLookup.CardId); } }
    public bool hasCardId { get { return HasComponent(CardComponentsLookup.CardId); } }

    public void AddCardId(Sheriff.ECS.Components.CardEntityId newValue) {
        var index = CardComponentsLookup.CardId;
        var component = (Sheriff.ECS.Components.CardIdComponent)CreateComponent(index, typeof(Sheriff.ECS.Components.CardIdComponent));
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceCardId(Sheriff.ECS.Components.CardEntityId newValue) {
        var index = CardComponentsLookup.CardId;
        var component = (Sheriff.ECS.Components.CardIdComponent)CreateComponent(index, typeof(Sheriff.ECS.Components.CardIdComponent));
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveCardId() {
        RemoveComponent(CardComponentsLookup.CardId);
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
public sealed partial class CardMatcher {

    static Entitas.IMatcher<CardEntity> _matcherCardId;

    public static Entitas.IMatcher<CardEntity> CardId {
        get {
            if (_matcherCardId == null) {
                var matcher = (Entitas.Matcher<CardEntity>)Entitas.Matcher<CardEntity>.AllOf(CardComponentsLookup.CardId);
                matcher.componentNames = CardComponentsLookup.componentNames;
                _matcherCardId = matcher;
            }

            return _matcherCardId;
        }
    }
}