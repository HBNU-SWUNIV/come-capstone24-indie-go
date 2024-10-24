using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineHandler : MonoBehaviour
{
    public Coroutine StartManagedCoroutine(IEnumerator coroutine, System.Action onComplete = null)
    {
        return StartCoroutine(RunCoroutine(coroutine, onComplete));
    }

    private IEnumerator RunCoroutine(IEnumerator coroutine, System.Action onComplete)
    {
        yield return StartCoroutine(coroutine);
        onComplete?.Invoke();
    }
}
