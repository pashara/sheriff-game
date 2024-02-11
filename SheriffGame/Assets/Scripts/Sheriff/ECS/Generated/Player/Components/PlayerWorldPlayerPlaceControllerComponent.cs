//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class PlayerEntity {

    public Sheriff.ECS.Components.WorldPlayerPlaceControllerComponent worldPlayerPlaceController { get { return (Sheriff.ECS.Components.WorldPlayerPlaceControllerComponent)GetComponent(PlayerComponentsLookup.WorldPlayerPlaceController); } }
    public bool hasWorldPlayerPlaceController { get { return HasComponent(PlayerComponentsLookup.WorldPlayerPlaceController); } }

    public void AddWorldPlayerPlaceController(Sheriff.GameFlow.States.ClassicGame.World.WorldPlayerPlaceControllers newValue) {
        var index = PlayerComponentsLookup.WorldPlayerPlaceController;
        var component = (Sheriff.ECS.Components.WorldPlayerPlaceControllerComponent)CreateComponent(index, typeof(Sheriff.ECS.Components.WorldPlayerPlaceControllerComponent));
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceWorldPlayerPlaceController(Sheriff.GameFlow.States.ClassicGame.World.WorldPlayerPlaceControllers newValue) {
        var index = PlayerComponentsLookup.WorldPlayerPlaceController;
        var component = (Sheriff.ECS.Components.WorldPlayerPlaceControllerComponent)CreateComponent(index, typeof(Sheriff.ECS.Components.WorldPlayerPlaceControllerComponent));
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveWorldPlayerPlaceController() {
        RemoveComponent(PlayerComponentsLookup.WorldPlayerPlaceController);
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

    static Entitas.IMatcher<PlayerEntity> _matcherWorldPlayerPlaceController;

    public static Entitas.IMatcher<PlayerEntity> WorldPlayerPlaceController {
        get {
            if (_matcherWorldPlayerPlaceController == null) {
                var matcher = (Entitas.Matcher<PlayerEntity>)Entitas.Matcher<PlayerEntity>.AllOf(PlayerComponentsLookup.WorldPlayerPlaceController);
                matcher.componentNames = PlayerComponentsLookup.componentNames;
                _matcherWorldPlayerPlaceController = matcher;
            }

            return _matcherWorldPlayerPlaceController;
        }
    }
}
