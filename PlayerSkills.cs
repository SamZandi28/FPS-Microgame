using UnityEngine;
using Unity.FPS.Game;
using UnityEngine.UI;
using UnityEngine.Playables;
using System.Collections.Generic;
using System.Collections;

public class PlayerSkills : MonoBehaviour , IDataPersistence
{
    [Header("Defending Shield Settings")]
    public bool isDefendingShieldUnlocked = false;
    public GameObject lockedShieldIcon;
    public GameObject unlockedShieldIcon;
    public DefendingShieldAbility defendingShieldAbility;

    [Header("Shockwave Settings")]
    public bool isShockwaveUnlocked = false;
    public GameObject lockedShockwaveIcon;
    public GameObject unlockedShockwaveIcon;
    public ShockwaveAbility shockwaveAbility;

    private int killCount = 0;


    public void LoadData(GameData data)
    {
        isDefendingShieldUnlocked = data.IsDefendingShieldUnlocked;
        isShockwaveUnlocked = data.IsShockwaveUnlocked;
        killCount = data.KillCount;

        UpdateSkillIcons();
    }

    public void SaveData(ref GameData data)
    {
        data.IsDefendingShieldUnlocked = isDefendingShieldUnlocked;
        data.IsShockwaveUnlocked = isShockwaveUnlocked;
        data.KillCount = killCount;
    }

    void Start()
    {
        UpdateSkillIcons();
    }

    void OnEnable()
    {
        EventManager.AddListener<EnemyKillEvent>(OnEnemyKilled);
    }

    void OnDisable()
    {
        EventManager.RemoveListener<EnemyKillEvent>(OnEnemyKilled);
    }

    void Update()
    {
        if (isDefendingShieldUnlocked && Input.GetKeyDown(KeyCode.K))
        {
            defendingShieldAbility.ActivateShield();
        }

        if (isShockwaveUnlocked && Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(shockwaveAbility.ActivateShockwave());
        }
    }

    private void OnEnemyKilled(GameEvent evt)
    {
        killCount++;

        if (killCount == 1 && !isDefendingShieldUnlocked)
        {
            isDefendingShieldUnlocked = true;
            UpdateSkillIcons();
            Debug.Log("Defending Shield Unlocked!");
        }
        else if (killCount == 2 && !isShockwaveUnlocked)
        {
            isShockwaveUnlocked = true;
            UpdateSkillIcons();
            Debug.Log("Shockwave Unlocked!");
        }
    }

    private void UpdateSkillIcons()
    {
        if (lockedShieldIcon != null)
            lockedShieldIcon.SetActive(!isDefendingShieldUnlocked);
        if (unlockedShieldIcon != null)
            unlockedShieldIcon.SetActive(isDefendingShieldUnlocked);

        if (lockedShockwaveIcon != null)
            lockedShockwaveIcon.SetActive(!isShockwaveUnlocked);
        if (unlockedShockwaveIcon != null)
            unlockedShockwaveIcon.SetActive(isShockwaveUnlocked);
    }
}
