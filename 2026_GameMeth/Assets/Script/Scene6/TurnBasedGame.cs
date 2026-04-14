using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Text; // Required for StringBuilder

// Class name updated to reflect assignment modification
public class TurnBasedGame_Assignment : MonoBehaviour
{
    // Keeping existing unused references for compatibility with existing scene setup
    public TextMeshProUGUI[] labels = new TextMeshProUGUI[4];

    // --- Provided Serialized Fields (Original) ---
    [SerializeField] float critChance = 0.2f;
    [SerializeField] float meanDamage = 20f;
    [SerializeField] float stdDevDamage = 5f;
    [SerializeField] float enemyHP = 100f;
    [SerializeField] float poissonLambda = 2f;
    [SerializeField] float hitRate = 0.6f;
    [SerializeField] float critDamageRate = 2f;
    [SerializeField] int maxHitsPerTurn = 5;

    // --- New Serialized Fields for Requirement 2 ---
    [Header("Rare Item Probability Settings")]
    [SerializeField] float baseRareChance = 0.2f; // Initial chance for a Weapon or Armor to be "Rare"
    float currentRareChance; // Accumulated chance, updated per turn

    // --- Assignment-specific state tracking ---
    int turn = 0;
    bool rareItemObtained = false;

    string[] rewards = { "Gold", "Weapon", "Armor", "Potion" };

    // --- Requirement 3: Data tracking fields ---
    int totalEnemiesSpawned = 0;
    int totalEnemiesDefeated = 0;
    int totalHitsAttempts = 0;
    int totalHits = 0;
    int totalCrits = 0;
    float maxDamageObserved = 0f;
    float minDamageObserved = float.MaxValue;
    Dictionary<string, int> obtainedItemsCount;

    public void StartSimulation()
    {
        // Initialization for data tracking (Requirement 3)
        obtainedItemsCount = new Dictionary<string, int>
        {
            {"Gold", 0},
            {"Potion", 0},
            {"Weapon_Generic", 0},
            {"Armor_Generic", 0},
            {"Weapon_Rare", 0},
            {"Armor_Rare", 0}
        };
        totalEnemiesSpawned = 0;
        totalEnemiesDefeated = 0;
        totalHitsAttempts = 0;
        totalHits = 0;
        totalCrits = 0;
        maxDamageObserved = 0f;
        minDamageObserved = float.MaxValue;

        // Initialize cumulative probability (Requirement 2)
        currentRareChance = baseRareChance;

        // Requirement 1: Geometric structure loop - run until rare item is found
        rareItemObtained = false;
        turn = 0;
        while (!rareItemObtained)
        {
            turn++; // Count the current turn (e.g., Turn 1)
            SimulateTurn();

            // Requirement 2: Accumulate chance if rare not found
            if (!rareItemObtained)
            {
                currentRareChance += 0.05f; // Additive increase of 5%
                // Cap probability at 100% for mathematical soundness
                if (currentRareChance > 1.0f) currentRareChance = 1.0f;
            }
        }

        // Requirement 3: Construct and output detailed summary to console
        LogFinalSummary();
    }

    void SimulateTurn()
    {
        // Debug.Log($"--- Turn {turn} --- (Rare Chance: {currentRareChance:P1})"); // Can be re-enabled for verbose logging

        // 푸아송 샘플링: 적 등장 수
        int enemyCount = SamplePoisson(poissonLambda);
        // Requirement 3: Track enemies spawned
        totalEnemiesSpawned += enemyCount;
        // Debug.Log($"적 등장 : {enemyCount}");

        for (int i = 0; i < enemyCount; i++)
        {
            // 이항 샘플링: 명중 횟수
            int hits = SampleBinomial(maxHitsPerTurn, hitRate);
            // Requirement 3: Track total hit attempts vs total hits
            totalHitsAttempts += maxHitsPerTurn;
            totalHits += hits;
            float totalDamage = 0f;

            for (int j = 0; j < hits; j++)
            {
                float damage = SampleNormal(meanDamage, stdDevDamage);

                // 베르누이 분포 샘플링: 확률 기반 치명타 발생
                if (Random.value < critChance)
                {
                    damage *= critDamageRate;
                    // Requirement 3: Track total crits
                    totalCrits++;
                    // Debug.Log($" 크리티컬 hit! {damage:F1}");
                }
                // else
                //    Debug.Log($" 일반 hit! {damage:F1}");

                // Requirement 3: Track max/min individual hit damage
                if (damage > maxDamageObserved) maxDamageObserved = damage;
                if (damage < minDamageObserved) minDamageObserved = damage;

                totalDamage += damage;
            }

            if (totalDamage >= enemyHP)
            {
                // Debug.Log($"적 {i + 1} 처치! (데미지: {totalDamage:F1})");
                // Requirement 3: Track enemies defeated
                totalEnemiesDefeated++;

                // 균등 분포 샘플링: 보상 결정
                string reward = rewards[UnityEngine.Random.Range(0, rewards.Length)];
                // Debug.Log($"보상: {reward}");

                // Requirement 3: Track reward counts, using accumulated chance and separating rare logic
                if (reward == "Weapon")
                {
                    if (Random.value < currentRareChance)
                    {
                        obtainedItemsCount["Weapon_Rare"]++;
                        rareItemObtained = true; // Loop will terminate after this turn
                        // Debug.Log("레어 무기 획득!");
                    }
                    else
                    {
                        obtainedItemsCount["Weapon_Generic"]++;
                    }
                }
                else if (reward == "Armor")
                {
                    if (Random.value < currentRareChance)
                    {
                        obtainedItemsCount["Armor_Rare"]++;
                        rareItemObtained = true; // Loop will terminate after this turn
                        // Debug.Log("레어 방어구 획득!");
                    }
                    else
                    {
                        obtainedItemsCount["Armor_Generic"]++;
                    }
                }
                else
                {
                    // For Gold and Potion, just increment the generic count
                    obtainedItemsCount[reward]++;
                }
            }
        }
    }

    // --- Requirement 3: Detailed Formatting and Output ---    
    void LogFinalSummary()
    {
        // 0으로 나누는 오류(Division by Zero) 방지
        float finalHitRate = (totalHitsAttempts > 0) ? (float)totalHits / totalHitsAttempts : 0f;
        float finalCritRate = (totalHits > 0) ? (float)totalCrits / totalHits : 0f;
        float finalMinDamage = (totalHits > 0) ? minDamageObserved : 0f;
        float finalMaxDamage = (totalHits > 0) ? maxDamageObserved : 0f;

        // 1. 전투 결과 텍스트 조립
        StringBuilder battleSb = new StringBuilder();
        // Rich Text를 사용하여 타이틀 색상 변경 (과제 이미지 참고)
        battleSb.AppendLine("<color=#FFD700><b>전투 결과</b></color>");
        battleSb.AppendLine($"총 진행 턴 수 : {turn}");
        battleSb.AppendLine($"발생한 적 : {totalEnemiesSpawned}");
        battleSb.AppendLine($"처치한 적 : {totalEnemiesDefeated}");
        battleSb.AppendLine($"공격 명중 결과 : {finalHitRate:P2}"); // P2: 소수점 둘째 자리 퍼센트
        battleSb.AppendLine($"발생한 치명타율 결과 : {finalCritRate:P2}");
        battleSb.AppendLine($"최대 데미지 : {finalMaxDamage:F2}"); // F2: 소수점 둘째 자리 실수
        battleSb.AppendLine($"최소 데미지 : {finalMinDamage:F2}");

        // 2. 획득한 아이템 텍스트 조립
        StringBuilder itemSb = new StringBuilder();
        itemSb.AppendLine("<color=#FFD700><b>획득한 아이템</b></color>");
        itemSb.AppendLine($"포션 : {obtainedItemsCount["Potion"]}개");
        itemSb.AppendLine($"골드 : {obtainedItemsCount["Gold"]}개");
        itemSb.AppendLine($"무기 - 일반 : {obtainedItemsCount["Weapon_Generic"]}개");
        itemSb.AppendLine($"무기 - 레어 : {obtainedItemsCount["Weapon_Rare"]}개");
        itemSb.AppendLine($"방어구 - 일반 : {obtainedItemsCount["Armor_Generic"]}개");
        itemSb.AppendLine($"방어구 - 레어 : {obtainedItemsCount["Armor_Rare"]}개");

        // --- UI Text 업데이트 부분 ---
        // 인스펙터에 할당된 labels 배열을 확인하고 UI 텍스트를 변경합니다.
        if (labels != null && labels.Length >= 2)
        {
            // labels[0]에 전투 결과를, labels[1]에 획득한 아이템을 넣습니다.
            if (labels[0] != null) labels[0].text = battleSb.ToString();
            if (labels[1] != null) labels[1].text = itemSb.ToString();
        }
        else if (labels != null && labels.Length == 1 && labels[0] != null)
        {
            // 만약 Text를 1개만 사용하신다면 두 내용을 합쳐서 출력합니다.
            labels[0].text = battleSb.ToString() + "\n\n" + itemSb.ToString();
        }

        // 과제 조건 1번(콘솔창 출력)을 위한 Debug.Log
        Debug.Log("--- 시뮬레이션 종료 ---");
        Debug.Log(battleSb.ToString() + "\n" + itemSb.ToString());
        Debug.Log($"[레어 아이템 획득 정보] 레어 획득 턴: {turn}턴, 최종 누적 확률: {currentRareChance:P1}");
    }

    // --- Provided Distribution Sample Functions (Unchanged) ---
    int SamplePoisson(float lambda)
    {
        int k = 0;
        float p = 1f;
        float L = Mathf.Exp(-lambda);
        while (p > L)
        {
            k++;
            p *= Random.value;
        }
        return k - 1;
    }

    int SampleBinomial(int n, float p)
    {
        int success = 0;
        for (int i = 0; i < n; i++)
            if (Random.value < p) success++;
        return success;
    }

    float SampleNormal(float mean, float stdDev)
    {
        float u1 = Random.value;
        float u2 = Random.value;
        // Added practical edge-case check for u1 being zero to prevent Log(0)
        float z = Mathf.Sqrt(-2.0f * Mathf.Log(Mathf.Max(u1, 1e-10f))) * Mathf.Cos(2.0f * Mathf.PI * u2);
        return mean + stdDev * z;
    }
}
