using Sheriff.ECS;
using UniRx;

public partial class CardEntity
{
    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.CardIdComponent> OnCardId() =>
        this.OnChange<Sheriff.ECS.Components.CardIdComponent>(0);

    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.CardOwnerComponent> OnCardOwner() =>
        this.OnChange<Sheriff.ECS.Components.CardOwnerComponent>(1);

    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.IdComponent> OnId() =>
        this.OnChange<Sheriff.ECS.Components.IdComponent>(2);

    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.InDecComponent> OnInDec() =>
        this.OnChange<Sheriff.ECS.Components.InDecComponent>(3);

    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.ResourceCategoryComponent> OnResourceCategory() =>
        this.OnChange<Sheriff.ECS.Components.ResourceCategoryComponent>(4);

    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.ResourceTypeComponent> OnResourceType() =>
        this.OnChange<Sheriff.ECS.Components.ResourceTypeComponent>(5);
}