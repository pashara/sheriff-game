using Sheriff.ECS;
using UniRx;

public partial class GameEntity
{
    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.ActualStateProviderWritableComponent> OnActualStateProviderWritable() =>
        this.OnChange<Sheriff.ECS.Components.ActualStateProviderWritableComponent>(0);

    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.GameIdComponent> OnGameId() =>
        this.OnChange<Sheriff.ECS.Components.GameIdComponent>(1);

    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.IdComponent> OnId() =>
        this.OnChange<Sheriff.ECS.Components.IdComponent>(2);

    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.PotentialPlayersSequence> OnPotentialPlayersSequence() =>
        this.OnChange<Sheriff.ECS.Components.PotentialPlayersSequence>(3);

    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.RoundComponent> OnRound() =>
        this.OnChange<Sheriff.ECS.Components.RoundComponent>(4);
}