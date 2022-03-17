using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public GameObject healthUIPrefab;
    
    private Image _sliderBar;
    private Transform _cam;
    private Transform _uiBar;
    private Transform _healthBarPoint;
    
    public bool alwaysVisible;
    public float visibleTime;
    private float _visibleRemainTime;

    private CharacterStats characterStats;

    private void Awake()
    {
        characterStats = GetComponent<CharacterStats>();
        _healthBarPoint = transform.GetChild(0);
        
        characterStats.UpdateHealthBarOnAttack += UpdateHealthBar;
    }

    private void OnEnable()
    {
        if (Camera.main is { }) _cam = Camera.main.transform;

        foreach (var canvas in GameObject.FindObjectsOfType<Canvas>())
        {
            if (canvas.renderMode==RenderMode.WorldSpace)
            {
                _uiBar = Instantiate(healthUIPrefab,canvas.transform).transform;
                _uiBar.position = _healthBarPoint.position;
                _sliderBar = _uiBar.GetChild(0).GetComponent<Image>();
                _uiBar.gameObject.SetActive(alwaysVisible);
            }
        }
    }

    private void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        if (currentHealth <= 0)
        {
            characterStats.UpdateHealthBarOnAttack -= UpdateHealthBar;
            Destroy(_uiBar.gameObject);
            return;
        }

        _visibleRemainTime = visibleTime;
        
        _uiBar.gameObject.SetActive(true); //每次被攻击后UI强行设置为可见
        _sliderBar.fillAmount = (float) currentHealth / maxHealth;
    }

    private void LateUpdate()
    {
        if (_uiBar == null) return;

        _uiBar.position = _healthBarPoint.position;
        _uiBar.forward = -_cam.forward; //使血条的方向始终朝着摄像机

        if (_visibleRemainTime <= 0 && !alwaysVisible)
        {
            _uiBar.gameObject.SetActive(false);
        }
        else
        {
            _visibleRemainTime -= Time.deltaTime;
        }
    }
}
