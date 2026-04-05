using UnityEngine;
using TMPro;
using System.Collections;

public class CriticalManager : MonoBehaviour
{
    [Header("UI & Object References")]
    // 인덱스 0: 적 체력 & 전투 로그
    // 인덱스 1: 치명타 통계
    // 인덱스 2: 누적 전리품 개수 & 최근 획득
    // 인덱스 3: 현재 드랍 확률 (천장 시스템)
    public TextMeshProUGUI[] labels = new TextMeshProUGUI[4];

    [Tooltip("적이 사망했을 때 숨길 적 이미지(GameObject)를 여기에 넣으세요.")]
    public GameObject enemyImageObject;

    [Header("Combat Stats")]
    public int enemyMaxHP = 300;
    private int currentHP;
    public int baseAttackPower = 25;
    public float critDamageMultiplier = 1.5f;

    [Header("Critical Stats")]
    public int totalHits = 0;
    public int critHits = 0;
    public float targetCritRate = 0.1f; // 10% 목표 치명타 확률

    [Header("Loot Counts")]
    public int killCount = 0; // 총 처치 수
    public int normalCount = 0;
    public int highCount = 0;
    public int rareCount = 0;
    public int legendCount = 0;

    [Header("Loot Probabilities (%)")]
    public float probNormal = 50.0f;
    public float probHigh = 30.0f;
    public float probRare = 15.0f;
    public float probLegend = 5.0f;

    private string lastAttackLog = "전투 대기 중...";
    private string lastLootLog = "없음";
    private bool isDead = false;

    private void Start()
    {
        currentHP = enemyMaxHP;
        UpdateUI();
    }

    // ---------------- [ 전투 로직 ] ----------------

    // 이 메서드를 공격(Attack) 버튼의 OnClick() 이벤트에 연결합니다.
    public void AttackEnemy()
    {
        if (isDead) return; // 죽어있을 땐 공격 불가

        bool isCrit = RollCrit();

        int damage = baseAttackPower;
        if (isCrit)
        {
            damage = Mathf.RoundToInt(baseAttackPower * critDamageMultiplier);
            lastAttackLog = $"<color=red>크리티컬 히트!! {damage} 피해!</color>";
        }
        else
        {
            lastAttackLog = $"일반 공격. {damage} 피해.";
        }

        currentHP -= damage;

        if (currentHP <= 0)
        {
            currentHP = 0;
            lastAttackLog = $"<b>적 처치 성공!</b>\n전리품을 획득했습니다.";

            // 적 사망 시 전리품 획득 로직 실행
            GenerateLoot();

            // 부활 대기
            StartCoroutine(RespawnRoutine());
        }

        UpdateUI();
    }

    private bool RollCrit()
    {
        totalHits++;
        float currentRate = totalHits > 0 ? (float)critHits / totalHits : 0f;

        if (currentRate < targetCritRate && (float)(critHits + 1) / totalHits <= targetCritRate) { critHits++; return true; }
        if (currentRate > targetCritRate && (float)critHits / totalHits >= targetCritRate) { return false; }
        if (Random.value < targetCritRate) { critHits++; return true; }
        return false;
    }

    private IEnumerator RespawnRoutine()
    {
        isDead = true;

        if (enemyImageObject != null) enemyImageObject.SetActive(false);

        yield return new WaitForSeconds(1.5f); // 1.5초 대기

        currentHP = enemyMaxHP;
        lastAttackLog = "새로운 적이 나타났습니다!";
        isDead = false;

        if (enemyImageObject != null) enemyImageObject.SetActive(true);

        UpdateUI();
    }

    // ---------------- [ 전리품 로직 ] ----------------

    private void GenerateLoot()
    {
        killCount++;
        float roll = Random.Range(0f, 100f);

        if (roll < probNormal)
        {
            normalCount++; lastLootLog = "일반"; ApplyLootPity(false);
        }
        else if (roll < probNormal + probHigh)
        {
            highCount++; lastLootLog = "고급"; ApplyLootPity(false);
        }
        else if (roll < probNormal + probHigh + probRare)
        {
            rareCount++; lastLootLog = "희귀"; ApplyLootPity(false);
        }
        else
        {
            legendCount++; lastLootLog = "<color=red>전설</color>"; ApplyLootPity(true);
        }
    }

    private void ApplyLootPity(bool gotLegendary)
    {
        if (gotLegendary)
        {
            probNormal = 50.0f; probHigh = 30.0f; probRare = 15.0f; probLegend = 5.0f;
        }
        else
        {
            probNormal = Mathf.Max(0f, probNormal - 0.5f);
            probHigh = Mathf.Max(0f, probHigh - 0.5f);
            probRare = Mathf.Max(0f, probRare - 0.5f);
            probLegend += 1.5f;
        }
    }

    // ---------------- [ UI 업데이트 ] ----------------

    private void UpdateUI()
    {
        if (labels.Length >= 4)
        {
            float critPercent = totalHits > 0 ? ((float)critHits / totalHits) * 100f : 0f;

            // 0. 체력 및 로그
            labels[0].text = $"<b>[적 체력]</b> {currentHP} / {enemyMaxHP}\n{lastAttackLog}";

            // 1. 치명타 정보
            labels[1].text = $"<b>[치명타 정보]</b>\n공격 횟수: {totalHits} | 치명타: {critHits}\n현재 치명타 확률: {critPercent:F2}%";

            // 2. 전리품 통계
            labels[2].text = $"<b>[전리품 통계]</b> (총 {killCount}마리 처치)\n최근 획득: {lastLootLog}\n일반:{normalCount} / 고급:{highCount} / 희귀:{rareCount} / 전설:{legendCount}";

            // 3. 전리품 확률
            labels[3].text = $"<b>[다음 드랍 확률]</b>\n일반: {probNormal:F1}% | 고급: {probHigh:F1}%\n희귀: {probRare:F1}% | 전설: {probLegend:F1}%";
        }
        else
        {
            Debug.LogWarning("UI가 할당되지 않았습니다. 인스펙터에서 labels 배열 크기를 4로 맞추고 Text를 넣어주세요.");
        }
    }
}