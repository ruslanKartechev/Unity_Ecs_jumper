using Ecs.Components;
using Ecs.Components.View;
using Ecs.Systems;
using Leopotam.EcsLite;
using UnityEngine;

namespace Ecs
{
    public class EntityMaker
    {
        public static int MakeMapEntity(EcsWorld world)
        {
            var entity = world.NewEntity();
            ref var map = ref world.AddComponentToEntity<MapComponent>(entity);
            map = new MapComponent();
            
            ref var cellTops = ref world.AddComponentToEntity<CellTopsComponent>(entity);
            cellTops = new CellTopsComponent();
            Pool.MapEntity = entity;
            return entity;
        }

        public static int MakePlayerEntity(EcsWorld world)
        {
            var entity = world.NewEntity();
            world.AddComponentToEntity<PositionComponent>(entity);
            world.AddComponentToEntity<RotationComponent>(entity);
            world.AddComponentToEntity<CellPositionComponent>(entity);
            world.AddComponentToEntity<MoveSpeedComponent>(entity);
            world.AddComponentToEntity<VerticalOffsetComponent>(entity);
            world.AddComponentToEntity<MaxJumpHeightComponent>(entity);
            
            world.AddComponentToEntity<PlayerComponent>(entity);
            world.AddComponentToEntity<MoveInputComponent>(entity);
            world.AddComponentToEntity<CurrentLevelComponent>(entity);
            Pool.PlayerEntity = entity;
            return entity;
        }

        public static int MakeCameraEntity(EcsWorld world)
        {
            var entity = world.NewEntity();
            world.AddComponentToEntity<CameraComponent>(entity);
            world.AddComponentToEntity<PositionComponent>(entity);
            world.AddComponentToEntity<RotationComponent>(entity);
            world.AddComponentToEntity<LookAtPosition>(entity);
            world.AddComponentToEntity<OffsetComponent>(entity);
            world.AddComponentToEntity<MoveSpeedComponent>(entity);
            Pool.CameraEntity = entity;
            return entity;
        }

        public static int MakeLevelEntity(EcsWorld world)
        {
            var entity = world.NewEntity();
            world.AddComponentToEntity<BlockSpawnDelayComponent>(entity);
            world.AddComponentToEntity<LevelIndexComponent>(entity);
            world.AddComponentToEntity<LevelComponent>(entity);
            Pool.LevelEntity = entity;
            return entity;
        }
        
        
        public static int MakeBlockEntity(EcsWorld world)
        {
            var entity = world.NewEntity();
            world.AddComponentToEntity<PositionComponent>(entity);
            world.AddComponentToEntity<TransformViewComponent>(entity);
            return entity;
        }
        
        public static int MakeBlockEntity(EcsWorld world, Vector3 position, Transform view)
        {
            var entity = world.NewEntity();
            ref var posComp = ref world.AddComponentToEntity<PositionComponent>(entity);
            posComp.Value = position;
            
            ref var viewComp = ref world.AddComponentToEntity<TransformViewComponent>(entity);
            viewComp.Body = view;
            return entity;
        }
    }
}