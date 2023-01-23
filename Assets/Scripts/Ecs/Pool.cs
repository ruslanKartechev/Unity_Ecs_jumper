using Leopotam.EcsLite;

namespace Ecs
{
    public static class Pool
    {
        public static EcsWorld World { get; set; }
        
        public static int MapEntity { get; set; }
        
        public static int PlayerEntity { get; set; }

        public static int CameraEntity { get; set; }
        public static int LevelEntity { get; set; }

        
    }
}