using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpText : MonoBehaviour
{
    public void SetText(string message)
    {
        GetComponent<Text>().text = message;
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
