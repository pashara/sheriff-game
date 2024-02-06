using Sheriff.ECS;
using UniRx;

public partial class CardEntity
{
    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.CardIdComponent> OnCardId() =>
        this.OnChange<Sheriff.ECS.Components.CardIdComponent>(0);

    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.CardOnHandComponent> OnCardOnHand() =>
        this.OnChange<Sheriff.ECS.Components.CardOnHandComponent>(1);

    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.CardOwnerComponent> OnCardOwner() =>
        this.OnChange<Sheriff.ECS.Components.CardOwnerComponent>(2);

    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.CardReleaseComponent> OnCardRelease() =>
        this.OnChange<Sheriff.ECS.Components.CardReleaseComponent>(3);

    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.IdComponent> OnId() =>
        this.OnChange<Sheriff.ECS.Components.IdComponent>(4);

    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.InDecComponent> OnInDec() =>
        this.OnChange<Sheriff.ECS.Components.InDecComponent>(5);

    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.ResourceCategoryComponent> OnResourceCategory() =>
        this.OnChange<Sheriff.ECS.Components.ResourceCategoryComponent>(6);

    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.ResourceTypeComponent> OnResourceType() =>
        this.OnChange<Sheriff.ECS.Components.ResourceTypeComponent>(7);

    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.SelectToDeclareComponent> OnSelectToDeclare() =>
        this.OnChange<Sheriff.ECS.Components.SelectToDeclareComponent>(8);
}