using Ecs;
using Ecs.Components;
using Ecs.Components.View;
using Ecs.Systems;
using Helpers;
using UnityEngine;
using View;

namespace Game
{
    public class SceneInitializer : MonoBehaviour
    {
        [SerializeField] private CameraView _cameraView;
        
        public void Run()
        {
            Dbg.LogRed($"SCENE INIT RUN");
            var entity = EntityMaker.MakeCameraEntity(Pool.World);
            ref var viewComponent = ref Pool.World.AddComponentToEntity<TransformViewComponent>(entity);
            viewComponent.Body = _cameraView.transform;
            
            ref var offsetComp = ref Pool.World.GetComponent<OffsetComponent>(entity);
            offsetComp.Value = _cameraView.OffsetValue;
            
            ref var moveSpeed = ref Pool.World.GetComponent<MoveSpeedComponent>(entity);
            moveSpeed.Value = _cameraView.MoveSpeed;

            ref var pos = ref Pool.World.GetComponent<PositionComponent>(entity);
            pos.Value = _cameraView.transform.position;
            
            ref var rot = ref Pool.World.GetComponent<RotationComponent>(entity);
            rot.Value = _cameraView.transform.rotation;

        }
    }
   
}
