using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float smoothing = 0.125f;

    public Vector2 maxPos;
    public Vector2 minPos;

    private Vector3 orignalCameraPos;
    public List<GameObject> enemies = new List<GameObject>();

    // Shake Parameters
    public float shakeDuration = 2f;
    public float shakeAmount = 0.7f;

    private bool canShake = false;
    private float _shakeTimer;
    int i;


    private void Start()
    {
        orignalCameraPos = transform.localPosition;
    }

    private void Update()
    {
        foreach(GameObject enemy in enemies)
        {
            if (enemy.activeInHierarchy == true)
            {
                ShakeCamera();
            }

            else
            {
                return;
            }
        }


        if (canShake)
        {
            StartCameraShakeEffect();
        }
    }

    public void ShakeCamera()
    {
        canShake = true;
        _shakeTimer = shakeDuration;
    }

    public void StartCameraShakeEffect()
    {
        if (_shakeTimer > 0)
        {
            transform.localPosition = orignalCameraPos + Random.insideUnitSphere * shakeAmount;
            _shakeTimer -= Time.deltaTime;
        }
        else
        {
            i++;
            _shakeTimer = 0f;
            transform.position = orignalCameraPos;
            //  cameraController.enabled = true;
            canShake = false;
        }
    }

     private void LateUpdate()
    {
        if (transform.position != target.position && !canShake)
        {
            Vector3 targetPos = new Vector3(target.position.x, target.position.y, transform.position.z);

            targetPos.x = Mathf.Clamp(targetPos.x, minPos.x, maxPos.x);
            targetPos.y = Mathf.Clamp(targetPos.y, minPos.y, maxPos.y);

            transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);
        }

    }


  }


