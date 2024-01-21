using Sheriff.ECS;
using UniRx;

public partial class InputEntity
{
    public IReadOnlyReactiveProperty<Sheriff.ECS.Components.IdComponent> OnId() =>
        this.OnChange<Sheriff.ECS.Components.IdComponent>(0);
}