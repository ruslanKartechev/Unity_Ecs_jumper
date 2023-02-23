using System.Collections.Generic;
using Ecs.Components;
using Ecs.Components.View;
using Ecs.Systems;
using Game;
using Leopotam.EcsLite;
using UnityEngine;
using View;

namespace Ecs
{
    public class EntityMaker
    {
        public static int MakeMapEntity(EcsWorld world)
        {
            var entity = world.NewEntity();
            ref var map = ref world.AddComponentToEntity<MapComponent>(entity);
            map.AllBlocks = new List<int>();
            map.AllAboveBlocks = new List<int>();

            world.AddComponentToEntity<TearMatDataComponent>(entity);
            
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
            
            ref var scale = ref world.AddComponentToEntity<LocalScaleComponent>(entity);
            scale.Value = 1f * Vector3.one;
            
            world.AddComponentToEntity<CellPositionComponent>(entity);
            world.AddComponentToEntity<MoveSpeedComponent>(entity);
            world.AddComponentToEntity<VerticalOffsetComponent>(entity);
            world.AddComponentToEntity<MaxJumpHeightComponent>(entity);
            
            world.AddComponentToEntity<PlayerComponent>(entity);
            world.AddComponentToEntity<CurrentLevelComponent>(entity);
            world.AddComponentToEntity<JumpCountComponent>(entity);
            world.AddComponentToEntity<CurrentHeightComponent>(entity);
            
            world.AddComponentToEntity<JumpHeightBonusCountComponent>(entity);
            world.AddComponentToEntity<JumpToTopBonusCountComponent>(entity);
            world.AddComponentToEntity<BlockSpawnDelayComponent>(entity);
            world.AddComponentToEntity<BlocksCountComponent>(entity);
            world.AddComponentToEntity<BlockSpawnDataComponent>(entity);
            world.AddComponentToEntity<GameStateComponent>(entity);
            world.AddComponentToEntity<TransformVC>(entity);
            world.AddComponentToEntity<MaxReachedHeightComponent>(entity);

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

            world.AddComponentToEntity<LevelIndexComponent>(entity);
            world.AddComponentToEntity<LevelComponent>(entity);
            Pool.LevelEntity = entity;
            world.AddComponentToEntity<NumbersBlockVC>(entity);
            ref var passedBlocks = ref world.AddComponentToEntity<PassedHeightsVC>(entity);
            passedBlocks.Values = new HashSet<float>();
            return entity;
        }

        public static int MakeBlockEntity(EcsWorld world, Vector3 position, CellBlockView view, Vector2Int cellPos)
        {
            var entity = world.NewEntity();
            world.AddComponentToEntity<BlockComponent>(entity);
            ref var posComp = ref world.AddComponentToEntity<PositionComponent>(entity);
            posComp.Value = position;
            world.AddComponentToEntity<RotationComponent>(entity);

            ref var cellPosComp = ref world.AddComponentToEntity<CellPositionComponent>(entity);
            cellPosComp.x = cellPos.x;
            cellPosComp.y = cellPos.y;
            
            ref var viewComp = ref world.AddComponentToEntity<TransformVC>(entity);
            viewComp.Body = view.transform;
            
            ref var mainMatComp = ref world.AddComponentToEntity<MainMaterialVC>(entity);
            mainMatComp.Value = view.MainMaterial;
            
            ref var transparentMatComp = ref world.AddComponentToEntity<TransparentMaterialVC>(entity);
            transparentMatComp.Value = view.TransparentMaterial;

            ref var rendererComp = ref world.AddComponentToEntity<RendererVC>(entity);
            rendererComp.Value = view.Renderer;
            
            return entity;
        }
        
        public static int MakeBreakableBlock(EcsWorld world, Vector3 position, BreakableBlockView view, Vector2Int cellPos)
        {
            var entity = world.NewEntity();
            world.AddComponentToEntity<BlockComponent>(entity);
            ref var posComp = ref world.AddComponentToEntity<PositionComponent>(entity);
            posComp.Value = position;
            world.AddComponentToEntity<RotationComponent>(entity);

            ref var cellPosComp = ref world.AddComponentToEntity<CellPositionComponent>(entity);
            cellPosComp.x = cellPos.x;
            cellPosComp.y = cellPos.y;
            
            ref var viewComp = ref world.AddComponentToEntity<TransformVC>(entity);
            viewComp.Body = view.transform;
            
            ref var mainMatComp = ref world.AddComponentToEntity<MainMaterialVC>(entity);
            mainMatComp.Value = view.MainMaterial;
            
            // ref var transparentMatComp = ref world.AddComponentToEntity<TransparentMaterialVC>(entity);
            // transparentMatComp.Value = view.TransparentMaterial;

            ref var rendererComp = ref world.AddComponentToEntity<RendererVC>(entity);
            rendererComp.Value = view.Renderer;
            
            return entity;
        }
        
        
    }
}