using Sheriff.ECS;
using UniRx;

public partial class PlayerEntity
{
    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.ActualStateProviderComponent> OnActualStateProvider() =>
        this.OnChange<Sheriff.ECS.Components.ActualStateProviderComponent>(0);

    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.AllowedActionsComponent> OnAllowedActions() =>
        this.OnChange<Sheriff.ECS.Components.AllowedActionsComponent>(1);

    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.CardsPopPerStepComponent> OnCardsPopPerStep() =>
        this.OnChange<Sheriff.ECS.Components.CardsPopPerStepComponent>(2);

    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.DealerComponent> OnDealer() =>
        this.OnChange<Sheriff.ECS.Components.DealerComponent>(3);

    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.DeclareResourcesByPlayerComponent> OnDeclareResourcesByPlayer() =>
        this.OnChange<Sheriff.ECS.Components.DeclareResourcesByPlayerComponent>(4);

    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.DropCardsComponent> OnDropCards() =>
        this.OnChange<Sheriff.ECS.Components.DropCardsComponent>(5);

    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.GoldCashCurrencyComponent> OnGoldCashCurrency() =>
        this.OnChange<Sheriff.ECS.Components.GoldCashCurrencyComponent>(6);

    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.IdComponent> OnId() =>
        this.OnChange<Sheriff.ECS.Components.IdComponent>(7);

    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.MaxCardsPopPerStepComponent> OnMaxCardsPopPerStep() =>
        this.OnChange<Sheriff.ECS.Components.MaxCardsPopPerStepComponent>(8);

    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.NicknameComponent> OnNickname() =>
        this.OnChange<Sheriff.ECS.Components.NicknameComponent>(9);

    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.OnHandCardsComponent> OnOnHandCards() =>
        this.OnChange<Sheriff.ECS.Components.OnHandCardsComponent>(10);

    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.PlayerControllerComponent> OnPlayerController() =>
        this.OnChange<Sheriff.ECS.Components.PlayerControllerComponent>(11);

    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.PlayerIdComponent> OnPlayerId() =>
        this.OnChange<Sheriff.ECS.Components.PlayerIdComponent>(12);

    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.PlayerNetworkIdComponent> OnPlayerNetworkId() =>
        this.OnChange<Sheriff.ECS.Components.PlayerNetworkIdComponent>(13);

    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.PlayerPositionIdComponent> OnPlayerPositionId() =>
        this.OnChange<Sheriff.ECS.Components.PlayerPositionIdComponent>(14);

    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.PlayerStatisticsComponent> OnPlayerStatistics() =>
        this.OnChange<Sheriff.ECS.Components.PlayerStatisticsComponent>(15);

    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.ReadyForCheckComponent> OnReadyForCheck() =>
        this.OnChange<Sheriff.ECS.Components.ReadyForCheckComponent>(16);

    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.SelectedCardsComponent> OnSelectedCards() =>
        this.OnChange<Sheriff.ECS.Components.SelectedCardsComponent>(17);

    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.SheriffCheckResultComponent> OnSheriffCheckResult() =>
        this.OnChange<Sheriff.ECS.Components.SheriffCheckResultComponent>(18);

    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.SheriffComponent> OnSheriff() =>
        this.OnChange<Sheriff.ECS.Components.SheriffComponent>(19);

    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.TransferredResourcesComponent> OnTransferredResources() =>
        this.OnChange<Sheriff.ECS.Components.TransferredResourcesComponent>(20);

    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.WorldPlayerPlaceControllerComponent> OnWorldPlayerPlaceController() =>
        this.OnChange<Sheriff.ECS.Components.WorldPlayerPlaceControllerComponent>(21);
}