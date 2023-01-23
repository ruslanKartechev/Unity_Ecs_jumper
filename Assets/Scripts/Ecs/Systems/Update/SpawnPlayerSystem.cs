using Data.Prefabs;
using Ecs.Components;
using Ecs.Components.View;
using Helpers;
using Leopotam.EcsLite;
using Services.Instantiate;
using Services.Parent;
using View;
using Zenject;

namespace Ecs.Systems
{
    public class SpawnPlayerSystem : IEcsRunSystem, IEcsInitSystem
    {
        private EcsFilter _filter;
        private EcsPool<SpawnPlayerComponent> _pool;
        [Inject] private IPrefabsRepository _prefabsRepository;
        [Inject] private IInstantiateService _instantiateService;
        [Inject] private IParentService _parentService;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var command in _filter)
            {
                var world = systems.GetWorld();

                var entity = Pool.PlayerEntity;
                var map = world.GetComponent<MapComponent>(Pool.MapEntity);
                var x = map.Height / 2;
                var y = map.Width / 2;
                var position = MapHelpers.GetPositionAtCell(x, y);
                
                var prefab = _prefabsRepository.GetPrefabGO(PrefabNames.Player);
                var instance = _instantiateService.Spawn<PlayerView>(prefab, _parentService.DefaultParent, position);
                var settings = instance.Settings;
                ref var pos = ref world.GetComponent<PositionComponent>(entity);
                pos.Value = position;
                ref var cellPos = ref world.GetComponent<CellPositionComponent>(entity);
                cellPos.x = x;
                cellPos.y = y;
                
                ref var speedComponent = ref world.GetComponent<MoveSpeedComponent>(entity);
                speedComponent.Value = settings.MoveSpeed;
                
                ref var viewComp = ref world.AddComponentToEntity<TransformViewComponent>(entity);
                viewComp.Body = instance.transform;
                
                ref var vertOffset = ref world.GetComponent<VerticalOffsetComponent>(entity);
                vertOffset.UpOffset = settings.UpOffset;
                vertOffset.UpOffsetLerpValue = settings.UpOffsetLerpValue;

                ref var maxHeight = ref world.GetComponent<MaxJumpHeightComponent>(entity);
                maxHeight.Value = settings.MaxJumpHeight;
                _pool.Del(command);

                ref var particlesView = ref world.AddComponentToEntity<JumpParticlesViewComponent>(entity);
                particlesView.PooParticles = instance.poofParticles;
                
                Dbg.LogGreen($"SPAWN PLAYER");
                ref var cameraLookPosition = ref world.GetComponent<LookAtPosition>(Pool.CameraEntity);
                cameraLookPosition.Value = position;
                
            }
        }
        
        public void Init(IEcsSystems systems)
        {
            _pool = systems.GetWorld().GetPool<SpawnPlayerComponent>();
            _filter = systems.GetWorld().Filter<SpawnPlayerComponent>().End();
        }
    }
}