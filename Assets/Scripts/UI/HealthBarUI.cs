using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public GameObject healthUIPrefab;
    public GameObject bossHealthUI;

    private Image _sliderBar;
    private Transform _cam;
    private Transform _uiBar;
    public Transform healthBarPoint; //血条的位置
    public DamageShow damageShow;
    private EnemyType _enemyType;

    public bool alwaysVisible;
    public float visibleTime;
    private float _visibleRemainTime;

    private CharacterStats characterStats;

    private void Awake()
    {
        characterStats = GetComponent<CharacterStats>();
        healthBarPoint = transform.GetChild(0);
        _enemyType = GetComponent<EnemyController>().enemyType;

        if (_enemyType == EnemyType.BOSS)
        {
            bossHealthUI.transform.GetChild(1).GetComponent<Text>().text = gameObject.name;
        }

        characterStats.UpdateHealthBarOnAttack += UpdateHealthBar;
    }

    private void OnEnable()
    {
        if (Camera.main is { }) _cam = Camera.main.transform;

        switch (_enemyType)
        {
            case EnemyType.BOSS:
                _sliderBar = bossHealthUI.transform.GetChild(0).GetComponent<Image>();
                break;
            
            case EnemyType.NORMAL: //不是Boss则生成小血条
                foreach (var canvas in FindObjectsOfType<Canvas>())
                {
                    if (canvas.renderMode == RenderMode.WorldSpace)
                    {
                        _uiBar = Instantiate(healthUIPrefab, canvas.transform).transform;
                        _uiBar.position = healthBarPoint.position;
                        _sliderBar = _uiBar.GetChild(0).GetComponent<Image>();
                        _uiBar.gameObject.SetActive(alwaysVisible);
                        break;
                    }
                }
                break;
        }
    }

    private void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        switch (_enemyType)
        {
            case EnemyType.BOSS:
                if (currentHealth <= 0)
                {
                    characterStats.UpdateHealthBarOnAttack -= UpdateHealthBar;
                    bossHealthUI.SetActive(false);
                    return;
                }

                _sliderBar.fillAmount = (float) currentHealth / maxHealth;
                break;

            case EnemyType.NORMAL:
                if (currentHealth <= 0)
                {
                    characterStats.UpdateHealthBarOnAttack -= UpdateHealthBar;
                    Destroy(_uiBar.gameObject);
                    return;
                }

                _visibleRemainTime = visibleTime;

                _uiBar.gameObject.SetActive(true); //每次被攻击后UI强行设置为可见
                _sliderBar.fillAmount = (float) currentHealth / maxHealth;
                break;
        }
    }

    private void LateUpdate()
    {
        if (_enemyType != EnemyType.NORMAL) return;
        if (_uiBar == null) return;

        _uiBar.position = healthBarPoint.position;
        _uiBar.forward = _cam.forward; //使血条的方向始终朝着摄像机

        if (_visibleRemainTime <= 0 && !alwaysVisible)
        {
            _uiBar.gameObject.SetActive(false);
        }
        else
        {
            _visibleRemainTime -= Time.deltaTime;
        }
    }

    public void CreateDamageShow(int damage, bool isCritical)
    {
        var damageShowIns = Instantiate(damageShow, transform);
        damageShowIns.isCritical = isCritical;
        damageShowIns.cam = _cam;
        damageShowIns.damage = damage;
        damageShowIns.transform.position = healthBarPoint.position + new Vector3(0, 0.3f, 0);
        damageShowIns.targetPosition = healthBarPoint.position + new Vector3(0, 1.5f, 0);
    }
}