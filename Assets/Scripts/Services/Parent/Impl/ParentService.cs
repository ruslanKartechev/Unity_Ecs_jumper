using UnityEngine;

namespace Services.Parent.Impl
{
    public class ParentService : IParentService
    {
        public Transform DefaultParent { get; set; }
        public Transform ShardsParent { get; set; }
    }
}