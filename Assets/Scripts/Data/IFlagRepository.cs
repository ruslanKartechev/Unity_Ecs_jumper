using UnityEngine;

namespace Data
{
    public interface IFlagRepository
    {
        Sprite GetRandomSprite();
        Sprite PlayerFlag();
    }
}