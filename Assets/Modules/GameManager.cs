using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text playStopText;
    private bool _isPause; 
    public bool IsPause {
        get => _isPause;
        set
        {
            _isPause = value;
            Time.timeScale = _isPause ? 1 : 0;
            playStopText.text = _isPause ? "▶" : "■";
        }
    }

    private void Start()
    {
        IsPause = false;
        monsterCountText.text = $"남은 적: {allMonsterCount}";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            IsPause = !IsPause;
        }
    }

    public int allMonsterCount;
    public int playerCount;
    public TextMeshProUGUI monsterCountText;
    public GameObject gameClearObj;
    public GameObject gameOverObj;
    public Light2D baseLight;
    public void KillMonster()
    {
        allMonsterCount--;
        monsterCountText.text = $"남은 적: {allMonsterCount}";
        if (allMonsterCount == 0)
        {
            baseLight.intensity = 1;
            gameClearObj.SetActive(true);
        }
    }

    public void DeadPlayer()
    {
        playerCount--;
        if (playerCount == 0)
        {
            gameOverObj.SetActive(true);
        }
    }

    public void B_Retry()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
