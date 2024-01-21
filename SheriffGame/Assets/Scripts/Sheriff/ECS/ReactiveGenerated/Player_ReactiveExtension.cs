using Sheriff.ECS;
using UniRx;

public partial class PlayerEntity
{
    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.DealerComponent> OnDealer() =>
        this.OnChange<Sheriff.ECS.Components.DealerComponent>(0);

    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.IdComponent> OnId() =>
        this.OnChange<Sheriff.ECS.Components.IdComponent>(1);

    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.PlayerIdComponent> OnPlayerId() =>
        this.OnChange<Sheriff.ECS.Components.PlayerIdComponent>(2);

    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.SheriffComponent> OnSheriff() =>
        this.OnChange<Sheriff.ECS.Components.SheriffComponent>(3);
}