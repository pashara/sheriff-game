using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Sheriff.ECS;
using Sheriff.ECS.Components;
using Sheriff.GameResources;
using Sirenix.OdinInspector;
using Zenject;

namespace Sheriff.GameFlow
{
    [Serializable]
    public class ProductDeclaration
    {
        [ShowInInspector]
        [JsonProperty("count")]
        public int Count { get; private set; }
        
        
        [JsonProperty("resource")]
        [ShowInInspector]
        public GameResourceType ResourceType { get; private set; }


        public ProductDeclaration()
        {
        }
        
        
        public ProductDeclaration(int count, GameResourceType resourceType)
        {
            Count = count;
            ResourceType = resourceType;
        }
        
        public ProductDeclaration(ProductDeclaration source)
        {
            Count = source.Count;
            ResourceType = source.ResourceType;
        }
    }

    [Serializable]
    public class ProductsDeclaration
    {
        [ShowInInspector]
        [JsonProperty("declarations")]
        public List<ProductDeclaration> Declarations { get; }

        public ProductsDeclaration()
        {
            Declarations = new();
        }
        public ProductsDeclaration(ProductsDeclaration source)
        {
            Declarations = source.Declarations.Select(x => new ProductDeclaration(x)).ToList();
        }
        public ProductsDeclaration(ICollection<ProductDeclaration> source)
        {
            Declarations = source.Select(x => new ProductDeclaration(x)).ToList();
        }
    }

    public class DeclareCommand : GameCommand<DeclareCommand.Params, DeclareCommand>
    {

        [Serializable]
        public class Params : ActionParam
        {
            public PlayerEntityId playerEntityId;
            public ProductsDeclaration declarations;
            
            public Params()
            {
            }
            
            public Params(Params @params)
            {
                playerEntityId = @params.playerEntityId;
                declarations = new ProductsDeclaration(@params.declarations);
            }
        }
        
        [Serializable]
        public class EmulateParams : EmulateActionParams
        {
            [JsonProperty("player_id")]
            public PlayerEntityId playerDestination;
            [JsonProperty("declarations")]
            public ProductsDeclaration declarations;
        }
        
        [Inject] private readonly EcsContextProvider _ecsContextProvider;

        [JsonIgnore] protected override Params AppliedParams => _params;
        
        [JsonProperty("params")]
        private Params _params = null;
        [JsonProperty("result")]
        private EmulateParams _result = null;

        public override DeclareCommand Calculate(Params param)
        {
            _params = new Params(param);
            _result = new EmulateParams()
            {
                playerDestination = param.playerEntityId,
                declarations = new ProductsDeclaration(param.declarations)
            };

            return this;
        }


        public override void Apply()
        {
            var entity = _ecsContextProvider.Context.player.GetEntityWithPlayerId(_result.playerDestination);
            if (entity == null)
                return;
            entity.ReplaceDeclareResourcesByPlayer(_result.declarations);
        }
    }
}