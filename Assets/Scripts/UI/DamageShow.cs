using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageShow : MonoBehaviour
{
    public TextMeshPro textMeshPro;
    public Transform cam;
    public Vector3 targetPosition;
    public int damage;
    public bool isCritical;

    private void Start()
    {
        textMeshPro.text = damage.ToString();
        if (isCritical)
        {
            textMeshPro.fontSize = 5;
            GetComponent<TMP_Text>().color=Color.red;
        }
        Destroy(gameObject,1f);
    }

    private void LateUpdate()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        transform.forward = cam.forward;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime);
    }
}
