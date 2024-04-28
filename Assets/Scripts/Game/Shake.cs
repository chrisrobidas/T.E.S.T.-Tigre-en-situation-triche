using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public float duration;
    public float shakeAmount;
    public float waitTime;

    private Vector3 initialPosition;
    
    void Start()
    {
        initialPosition = transform.position;
        StartCoroutine(DoShake());
    }

    IEnumerator DoShake()
    {
        while(duration > 0)
        {
            transform.position = initialPosition;
            
            duration -= Time.deltaTime;
            float randomShakeX = Random.Range(0f, 1f);
            float randomShakeY = Random.Range(0f, 1f);

            transform.position += new Vector3(shakeAmount * randomShakeX, shakeAmount * randomShakeY, 0);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
