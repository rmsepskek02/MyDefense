using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
    public Wave[] waves;
    public GameObject enemyObj;
    public GameObject startPoint;
    public TextMeshProUGUI timerText;
    Vector3 spawnPoint = new Vector3();
    public float timerTime = 5f;   // 타이머 시간
    float roundDelay = 5f;  // 라운드 딜레이 시간
    int spawnCount = 0;     // 첫 스폰 개체 수
    public TextMeshProUGUI roundTextClear;
    public GameObject ClearUI;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = startPoint.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        SpawnForRound(timerTime);
    }

    // 타이머 코루틴
    IEnumerator CheckTimer(float _time)
    {
        // 타이머 시간 설정
        float timer = _time;
        // timer 초기화
        float time = 0f;

        while (time < timer)
        {
            // 경과된 시간을 누적
            time += Time.deltaTime;
            // UI 업데이트
            timerText.text = Mathf.RoundToInt(timer - time).ToString();
            yield return null;
        }
    }

    // 스폰 딜레이 코루틴
    IEnumerator DelayForSpawn()
    {
        Wave currentWave = waves[PlayerStats.Round - 1];
        // 라운드 딜레이
        yield return new WaitForSeconds(roundDelay);

        for (var i = 0; i < currentWave.enemyCount; i++)
        {
            // Enemy 생성
            GameObject go = Instantiate(currentWave.enemyPrefab, spawnPoint, Quaternion.identity);

            int num = Random.Range(0, 1000);
            go.name = go.name + num;
            // 스폰 딜레이
            yield return new WaitForSeconds(currentWave.spawnDelay);
        }
    }

    // Wave 개수에 따른 라운드 시작
    void SpawnForRound(float _timerTime)
    {
        if (PlayerStats.Round > waves.Length)
        {
            Debug.Log("LEVEL CLEAR");
            ClearUI.SetActive(true);
            roundTextClear.text = $"{SceneManager.GetActiveScene().name.Replace("Level", "")}";
            int nowLevel = PlayerPrefs.GetInt("nowLevel", 1);
            if (nowLevel < GameManager.nowLevel + 1)
            {
                PlayerPrefs.SetInt("nowLevel", GameManager.nowLevel + 1);
            }
            
            return;
        }

        if (PlayerStats.Wave > 0)
            return;
        // 개채 생성 수 증가
        PlayerStats.IncreaseRound();
        if (PlayerStats.Round > waves.Length) { return; }
        Wave currentWave = waves[PlayerStats.Round - 1];
        spawnCount = currentWave.enemyCount;
        PlayerStats.SetWave(spawnCount);
        // 타이머 코루틴
        StartCoroutine(CheckTimer(_timerTime));
        // 다음 라운드 시작, 스폰 딜레이 코루틴
        StartCoroutine(DelayForSpawn());
    }
}

