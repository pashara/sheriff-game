//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class PlayerEntity {

    public Sheriff.ECS.Components.PlayerStatisticsComponent playerStatistics { get { return (Sheriff.ECS.Components.PlayerStatisticsComponent)GetComponent(PlayerComponentsLookup.PlayerStatistics); } }
    public bool hasPlayerStatistics { get { return HasComponent(PlayerComponentsLookup.PlayerStatistics); } }

    public void AddPlayerStatistics(Sheriff.GameFlow.GameStatistics.PlayerStatistics newValue) {
        var index = PlayerComponentsLookup.PlayerStatistics;
        var component = (Sheriff.ECS.Components.PlayerStatisticsComponent)CreateComponent(index, typeof(Sheriff.ECS.Components.PlayerStatisticsComponent));
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplacePlayerStatistics(Sheriff.GameFlow.GameStatistics.PlayerStatistics newValue) {
        var index = PlayerComponentsLookup.PlayerStatistics;
        var component = (Sheriff.ECS.Components.PlayerStatisticsComponent)CreateComponent(index, typeof(Sheriff.ECS.Components.PlayerStatisticsComponent));
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemovePlayerStatistics() {
        RemoveComponent(PlayerComponentsLookup.PlayerStatistics);
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

    static Entitas.IMatcher<PlayerEntity> _matcherPlayerStatistics;

    public static Entitas.IMatcher<PlayerEntity> PlayerStatistics {
        get {
            if (_matcherPlayerStatistics == null) {
                var matcher = (Entitas.Matcher<PlayerEntity>)Entitas.Matcher<PlayerEntity>.AllOf(PlayerComponentsLookup.PlayerStatistics);
                matcher.componentNames = PlayerComponentsLookup.componentNames;
                _matcherPlayerStatistics = matcher;
            }

            return _matcherPlayerStatistics;
        }
    }
}
