using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchEvent : MonoBehaviour
{
    void Start()
    {
        // 코루틴 시작
        StartCoroutine(DestroyAfterDelay());
    }

    private IEnumerator DestroyAfterDelay()
    {
        // 2초 대기
        yield return new WaitForSeconds(2f);
        
        // 자기 자신 삭제
        Destroy(gameObject);
    }
}
