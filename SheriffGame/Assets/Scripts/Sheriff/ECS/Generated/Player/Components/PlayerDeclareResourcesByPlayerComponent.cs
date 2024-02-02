//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class PlayerEntity {

    public Sheriff.ECS.Components.DeclareResourcesByPlayerComponent declareResourcesByPlayer { get { return (Sheriff.ECS.Components.DeclareResourcesByPlayerComponent)GetComponent(PlayerComponentsLookup.DeclareResourcesByPlayer); } }
    public bool hasDeclareResourcesByPlayer { get { return HasComponent(PlayerComponentsLookup.DeclareResourcesByPlayer); } }

    public void AddDeclareResourcesByPlayer(Sheriff.GameFlow.ProductsDeclaration newValue) {
        var index = PlayerComponentsLookup.DeclareResourcesByPlayer;
        var component = (Sheriff.ECS.Components.DeclareResourcesByPlayerComponent)CreateComponent(index, typeof(Sheriff.ECS.Components.DeclareResourcesByPlayerComponent));
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceDeclareResourcesByPlayer(Sheriff.GameFlow.ProductsDeclaration newValue) {
        var index = PlayerComponentsLookup.DeclareResourcesByPlayer;
        var component = (Sheriff.ECS.Components.DeclareResourcesByPlayerComponent)CreateComponent(index, typeof(Sheriff.ECS.Components.DeclareResourcesByPlayerComponent));
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveDeclareResourcesByPlayer() {
        RemoveComponent(PlayerComponentsLookup.DeclareResourcesByPlayer);
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

    static Entitas.IMatcher<PlayerEntity> _matcherDeclareResourcesByPlayer;

    public static Entitas.IMatcher<PlayerEntity> DeclareResourcesByPlayer {
        get {
            if (_matcherDeclareResourcesByPlayer == null) {
                var matcher = (Entitas.Matcher<PlayerEntity>)Entitas.Matcher<PlayerEntity>.AllOf(PlayerComponentsLookup.DeclareResourcesByPlayer);
                matcher.componentNames = PlayerComponentsLookup.componentNames;
                _matcherDeclareResourcesByPlayer = matcher;
            }

            return _matcherDeclareResourcesByPlayer;
        }
    }
}