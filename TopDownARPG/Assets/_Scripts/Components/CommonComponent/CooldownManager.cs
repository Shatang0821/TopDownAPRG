using System;
using System.Collections.Generic;
using UnityEngine;

public class CooldownManager : MonoBehaviour
{
    [SerializeField] private List<SkillCooldown> skillCooldowns = new List<SkillCooldown>();

    private Dictionary<string, float> cooldownTimers = new Dictionary<string, float>();

    private void Start()
    {
        // 初期設定: skillCooldownsリストの各スキルをDictionaryに追加
        foreach (var skill in skillCooldowns)
        {
            if (!cooldownTimers.ContainsKey(skill.skillName))
            {
                cooldownTimers[skill.skillName] = 0; // 初期値としてクールダウン中でないことを示す
            }
        }
    }

    private void Update()
    {
        foreach (var skillCooldown in skillCooldowns)
        {
            // クールダウン中のスキルのみタイマーを減少
            if (cooldownTimers[skillCooldown.skillName] > 0)
            {
                cooldownTimers[skillCooldown.skillName] -= Time.deltaTime;
                Debug.Log(cooldownTimers[skillCooldown.skillName]); 
                // クールダウンが終了した場合
                if (cooldownTimers[skillCooldown.skillName] <= 0)
                {
                   
                    cooldownTimers[skillCooldown.skillName] = 0; // タイマーをゼロで固定
                }
            }
        }
    }
    
    /// <summary>
    /// 指定したスキルがクールダウン中かをチェック
    /// </summary>
    /// <param name="skillName"></param>
    /// <returns></returns>
    public bool IsOnCooldown(string skillName)
    {
        if (cooldownTimers.ContainsKey(skillName))
        {
            return cooldownTimers[skillName] > 0;
        }
        else
        {
            Debug.LogWarning($"Skill '{skillName}' does not exist in the cooldown manager.");
            return false;
        }
    }

    // 指定したスキルのクールダウンを開始する
    public void TriggerCooldown(string skillName)
    {
        foreach (var skillCooldown in skillCooldowns)
        {
            if (skillCooldown.skillName == skillName)
            {
                cooldownTimers[skillName] = skillCooldown.cooldownTime;
                break;
            }
        }
    }

    // 指定したスキルのクールダウン残り時間を取得する
    public float GetRemainingCooldown(string skillName)
    {
        if (cooldownTimers.ContainsKey(skillName))
        {
            float lastUsedTime = cooldownTimers[skillName];
            float cooldownTime = GetCooldownTime(skillName);
            float remaining = (lastUsedTime + cooldownTime) - Time.time;
            return remaining > 0 ? remaining : 0;
        }
        else
        {
            Debug.LogWarning($"Skill '{skillName}' does not exist in the cooldown manager.");
            return 0;
        }
    }

    // スキルのクールダウン時間を取得する
    private float GetCooldownTime(string skillName)
    {
        foreach (var skill in skillCooldowns)
        {
            if (skill.skillName == skillName)
            {
                return skill.cooldownTime;
            }
        }

        return 0;
    }
}

[Serializable]
public class SkillCooldown
{
    public string skillName;
    public float cooldownTime;
}