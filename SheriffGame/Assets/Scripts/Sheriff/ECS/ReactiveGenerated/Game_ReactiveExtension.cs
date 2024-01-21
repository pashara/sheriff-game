using Sheriff.ECS;
using UniRx;

public partial class GameEntity
{
    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.GameIdComponent> OnGameId() =>
        this.OnChange<Sheriff.ECS.Components.GameIdComponent>(0);

    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.IdComponent> OnId() =>
        this.OnChange<Sheriff.ECS.Components.IdComponent>(1);

    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.PotentialPlayersSequence> OnPotentialPlayersSequence() =>
        this.OnChange<Sheriff.ECS.Components.PotentialPlayersSequence>(2);

    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.RoundComponent> OnRound() =>
        this.OnChange<Sheriff.ECS.Components.RoundComponent>(3);
}