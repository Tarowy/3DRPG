using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//要注意此脚本的执行顺序，如果在GameManager注册playerStats之前访问playerStats则会报错
public class PlayerHealthUI : MonoBehaviour
{
    private Text _levelText;
    private Image _healthSlider;
    private Image _expSlider;

    private void Awake()
    {
        _levelText = transform.GetChild(2).GetComponent<Text>();
        _healthSlider = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        _expSlider = transform.GetChild(1).GetChild(0).GetComponent<Image>();
    }

    private void Update()
    {
        UpdateHealthBar();
        UpdateExpBar();
    }

    private void UpdateHealthBar()
    {
        _healthSlider.fillAmount =
            (float) GameManager.Instance.playerStats.CurrentHealth / GameManager.Instance.playerStats.MaxHealth;
    }

    private void UpdateExpBar()
    {
        if (GameManager.Instance.playerStats is { })
        {
            _expSlider.fillAmount =
                (float) GameManager.Instance.playerStats.characterDataSo.currentExp /
                GameManager.Instance.playerStats.characterDataSo.baseExp;
            _levelText.text = "Level " + GameManager.Instance.playerStats.characterDataSo.currentLevel.ToString();
        }
    }
}
