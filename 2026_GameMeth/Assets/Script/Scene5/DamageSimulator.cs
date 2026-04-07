using TMPro;
using UnityEngine;

public class DamageSimulator : MonoBehaviour
{
    public TextMeshProUGUI statusDisplay;
    public TextMeshProUGUI logDisplay;
    public TextMeshProUGUI resultDisplay;
    public TextMeshProUGUI rangedDisplay;

    private int level = 1;
    private float totalDamage = 0, baseDamage = 20f;
    private int attackCount = 0;

    private string weaponName;
    private float stdDevMult, critRate, critMult;

    void Start()
    {
        SetWeapon(0); 
    }

    private void ResetData()
    {
        totalDamage = 0;
        attackCount = 0;
        level = 1;
        baseDamage = 20f;
    }

    public void SetWeapon(int id)
    {
        ResetData();
        if (id == 0)
        {
            SetStats("단검", 0.1f, 0.4f, 1.5f);
        }
        else if (id == 1)
        {
            SetStats("장검", 0.2f, 0.3f, 2.0f);
        }
        else
        {
            SetStats("도끼", 0.3f, 0.2f, 3.0f);
        }

        logDisplay.text = string.Format("{0} 장착!", weaponName);
        UpdateUI();
    }

    private void SetStats(string _name, float _stdDev, float _critRate, float _critMult)
    {
        weaponName = _name;
        stdDevMult = _stdDev;
        critRate = _critRate;
        critMult = _critMult;
    }

    public void LevelUp()
    {
        totalDamage = 0;
        attackCount = 0;
        level++;
        baseDamage = level * 20f;
        logDisplay.text = string.Format("레벨업! 현재 레벨: {0}", level);
        UpdateUI();
    }

    private float ExecuteSingleAttack(out bool isMiss, out bool isWeak, out bool isCrit)
    {
        float sd = baseDamage * stdDevMult;
        float rawDamage = GetNormalStdDevDamage(baseDamage, sd);

        isMiss = false;
        isWeak = false;
        isCrit = false;

        if (rawDamage < baseDamage - 2f * sd)
        {
            isMiss = true;
            return 0f;
        }

        float currentDamage = rawDamage;

        if (currentDamage > baseDamage + 2f * sd)
        {
            isWeak = true;
            currentDamage *= 2f;
        }

        if (Random.value < critRate)
        {
            isCrit = true;
            currentDamage *= critMult;
        }

        return currentDamage;
    }

    public void OnAttack()
    {
        float finalDamage = ExecuteSingleAttack(out bool isMiss, out bool isWeak, out bool isCrit);

        attackCount++;
        totalDamage += finalDamage;

        if (isMiss)
        {
            logDisplay.text = "<color=gray>[명중 실패]</color> 데미지: 0";
        }
        else
        {
            string weakMark = isWeak ? "<color=blue>[약점 공격!]</color> " : "";
            string critMark = isCrit ? "<color=red>[치명타!]</color> " : "";
            logDisplay.text = string.Format("{0}{1}데미지: {2:F1}", weakMark, critMark, finalDamage);
        }
        UpdateUI();
    }

    public void OnAttack1000()
    {
        int weakCount = 0;
        int missCount = 0;
        int critCount = 0;
        float maxDamage = 0f;
        float tempTotalDamage = 0f;

        for (int i = 0; i < 1000; i++)
        {
            float finalDamage = ExecuteSingleAttack(out bool isMiss, out bool isWeak, out bool isCrit);

            if (isMiss) missCount++;
            if (isWeak) weakCount++;
            if (isCrit) critCount++;

            if (finalDamage > maxDamage) maxDamage = finalDamage;
            tempTotalDamage += finalDamage;
        }

        attackCount += 1000;
        totalDamage += tempTotalDamage;

        logDisplay.text = string.Format(
            "<b>[1000회 공격 결과 요약]</b>\n" +
            "약점 공격 횟수: {0}회\n" +
            "명중 실패 횟수: {1}회\n" +
            "전체 크리티컬 횟수: {2}회\n" +
            "최대 데미지: {3:F1}",
            weakCount, missCount, critCount, maxDamage);

        UpdateUI();
    }

    private void UpdateUI()
    {
        statusDisplay.text = string.Format("Level: {0} / 무기: {1}\n기본 데미지: {2} / 치명타: {3}% (x{4})",
            level, weaponName, baseDamage, critRate * 100, critMult);

        rangedDisplay.text = string.Format("예상 일반 데미지 범위 : [{0:F1} ~ {1:F1}]",
            baseDamage - (3 * baseDamage * stdDevMult),
            baseDamage + (3 * baseDamage * stdDevMult));

        float dpa = attackCount > 0 ? totalDamage / attackCount : 0;
        resultDisplay.text = string.Format("누적 데미지: {0:F1}\n공격 횟수: {1}\n평균 DPA: {2:F2}",
            totalDamage, attackCount, dpa);
    }

    private float GetNormalStdDevDamage(float mean, float stdDev)
    {
        float u1 = 1.0f - Random.value;
        float u2 = 1.0f - Random.value;
        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);
        return mean + stdDev * randStdNormal;
    }
}
