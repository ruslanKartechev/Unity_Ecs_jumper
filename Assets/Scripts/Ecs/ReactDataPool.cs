using UI.React;

namespace Ecs
{
    public static class ReactDataPool
    {
        public static ReactiveProperty<int> MoveCount = new ReactiveProperty<int>();
        public static ReactiveProperty<int> BlocksCount = new ReactiveProperty<int>();
        public static ReactiveProperty<float> PlayerHeight = new ReactiveProperty<float>();
        public static ReactiveProperty<int> Tier = new ReactiveProperty<int>();
        
    }
}