using UnityEngine;
using System.Collections;
namespace Services.MonoHelpers
{
    public interface ICoroutineService
    {
        Coroutine StartCor(IEnumerator enumerator);
        void StopCor(Coroutine coroutine);
    }
}