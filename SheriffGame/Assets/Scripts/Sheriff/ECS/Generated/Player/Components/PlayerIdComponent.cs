//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class PlayerEntity {

    public Sheriff.ECS.Components.IdComponent id { get { return (Sheriff.ECS.Components.IdComponent)GetComponent(PlayerComponentsLookup.Id); } }
    public bool hasId { get { return HasComponent(PlayerComponentsLookup.Id); } }

    public void AddId(long newID) {
        var index = PlayerComponentsLookup.Id;
        var component = (Sheriff.ECS.Components.IdComponent)CreateComponent(index, typeof(Sheriff.ECS.Components.IdComponent));
        component.ID = newID;
        AddComponent(index, component);
    }

    public void ReplaceId(long newID) {
        var index = PlayerComponentsLookup.Id;
        var component = (Sheriff.ECS.Components.IdComponent)CreateComponent(index, typeof(Sheriff.ECS.Components.IdComponent));
        component.ID = newID;
        ReplaceComponent(index, component);
    }

    public void RemoveId() {
        RemoveComponent(PlayerComponentsLookup.Id);
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiInterfaceGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class PlayerEntity : IIdEntity { }

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class PlayerMatcher {

    static Entitas.IMatcher<PlayerEntity> _matcherId;

    public static Entitas.IMatcher<PlayerEntity> Id {
        get {
            if (_matcherId == null) {
                var matcher = (Entitas.Matcher<PlayerEntity>)Entitas.Matcher<PlayerEntity>.AllOf(PlayerComponentsLookup.Id);
                matcher.componentNames = PlayerComponentsLookup.componentNames;
                _matcherId = matcher;
            }

            return _matcherId;
        }
    }
}