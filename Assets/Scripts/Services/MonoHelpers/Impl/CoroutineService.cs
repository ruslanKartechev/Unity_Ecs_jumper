using System;
using System.Collections;
using UnityEngine;
namespace Services.MonoHelpers.Impl
{
    public class CoroutineService : MonoBehaviour, ICoroutineService
    {
        public Coroutine StartCor(IEnumerator enumerator)
        {
            return StartCoroutine(enumerator);
        }

        public void StopCor(Coroutine coroutine)
        {
            if(coroutine != null)
                StopCoroutine(coroutine);
        }

        public void InvokeAfter(Action action, float delay)
        {
            StartCoroutine(Delayed(action, delay));
        }

        private IEnumerator Delayed(Action action, float delay)
        {
            yield return new WaitForSeconds(delay);
            action?.Invoke();
        }
    }
    
}