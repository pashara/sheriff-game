using Sheriff.ECS;
using UniRx;

public partial class PlayerEntity
{
    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.ActualStateProviderComponent> OnActualStateProvider() =>
        this.OnChange<Sheriff.ECS.Components.ActualStateProviderComponent>(0);

    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.AllowedActionsComponent> OnAllowedActions() =>
        this.OnChange<Sheriff.ECS.Components.AllowedActionsComponent>(1);

    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.DealerComponent> OnDealer() =>
        this.OnChange<Sheriff.ECS.Components.DealerComponent>(2);

    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.DeclareResourcesByPlayerComponent> OnDeclareResourcesByPlayer() =>
        this.OnChange<Sheriff.ECS.Components.DeclareResourcesByPlayerComponent>(3);

    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.GoldCashCurrencyComponent> OnGoldCashCurrency() =>
        this.OnChange<Sheriff.ECS.Components.GoldCashCurrencyComponent>(4);

    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.IdComponent> OnId() =>
        this.OnChange<Sheriff.ECS.Components.IdComponent>(5);

    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.PlayerIdComponent> OnPlayerId() =>
        this.OnChange<Sheriff.ECS.Components.PlayerIdComponent>(6);

    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.SelectedCardsComponent> OnSelectedCards() =>
        this.OnChange<Sheriff.ECS.Components.SelectedCardsComponent>(7);

    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.SheriffComponent> OnSheriff() =>
        this.OnChange<Sheriff.ECS.Components.SheriffComponent>(8);
}