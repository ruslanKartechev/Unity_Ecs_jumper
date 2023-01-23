using UnityEngine;

namespace Services.Parent
{
    public interface IParentService
    {
        Transform DefaultParent { get; set; }
        Transform ShardsParent { get; set; }
    }
}