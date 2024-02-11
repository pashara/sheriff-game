//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class PlayerEntity {

    public Sheriff.ECS.Components.PlayerControllerComponent playerController { get { return (Sheriff.ECS.Components.PlayerControllerComponent)GetComponent(PlayerComponentsLookup.PlayerController); } }
    public bool hasPlayerController { get { return HasComponent(PlayerComponentsLookup.PlayerController); } }

    public void AddPlayerController(Sheriff.GameFlow.Players.PlayerController newValue) {
        var index = PlayerComponentsLookup.PlayerController;
        var component = (Sheriff.ECS.Components.PlayerControllerComponent)CreateComponent(index, typeof(Sheriff.ECS.Components.PlayerControllerComponent));
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplacePlayerController(Sheriff.GameFlow.Players.PlayerController newValue) {
        var index = PlayerComponentsLookup.PlayerController;
        var component = (Sheriff.ECS.Components.PlayerControllerComponent)CreateComponent(index, typeof(Sheriff.ECS.Components.PlayerControllerComponent));
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemovePlayerController() {
        RemoveComponent(PlayerComponentsLookup.PlayerController);
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

    static Entitas.IMatcher<PlayerEntity> _matcherPlayerController;

    public static Entitas.IMatcher<PlayerEntity> PlayerController {
        get {
            if (_matcherPlayerController == null) {
                var matcher = (Entitas.Matcher<PlayerEntity>)Entitas.Matcher<PlayerEntity>.AllOf(PlayerComponentsLookup.PlayerController);
                matcher.componentNames = PlayerComponentsLookup.componentNames;
                _matcherPlayerController = matcher;
            }

            return _matcherPlayerController;
        }
    }
}
