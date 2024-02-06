//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class CardEntity {

    static readonly Sheriff.ECS.Components.CardOnHandComponent cardOnHandComponent = new Sheriff.ECS.Components.CardOnHandComponent();

    public bool isCardOnHand {
        get { return HasComponent(CardComponentsLookup.CardOnHand); }
        set {
            if (value != isCardOnHand) {
                var index = CardComponentsLookup.CardOnHand;
                if (value) {
                    var componentPool = GetComponentPool(index);
                    var component = componentPool.Count > 0
                            ? componentPool.Pop()
                            : cardOnHandComponent;

                    AddComponent(index, component);
                } else {
                    RemoveComponent(index);
                }
            }
        }
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

    static Entitas.IMatcher<CardEntity> _matcherCardOnHand;

    public static Entitas.IMatcher<CardEntity> CardOnHand {
        get {
            if (_matcherCardOnHand == null) {
                var matcher = (Entitas.Matcher<CardEntity>)Entitas.Matcher<CardEntity>.AllOf(CardComponentsLookup.CardOnHand);
                matcher.componentNames = CardComponentsLookup.componentNames;
                _matcherCardOnHand = matcher;
            }

            return _matcherCardOnHand;
        }
    }
}
