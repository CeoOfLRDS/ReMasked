using UnityEngine;

public class PlayerMaskSwitcher : MonoBehaviour
{
    public Transform masksRoot;

    private GameObject[] masks;
    private int currentIndex;

    private PlayerMovement movement;
    private PlayerPunch punch;
    private Health health;

    void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        punch = GetComponent<PlayerPunch>();
        health = GetComponent<Health>();

        int count = masksRoot.childCount;
        masks = new GameObject[count];

        for (int i = 0; i < count; i++)
        {
            masks[i] = masksRoot.GetChild(i).gameObject;
            masks[i].SetActive(false);
        }

        currentIndex = 0;
        masks[currentIndex].SetActive(true);
        ApplyMaskEffects();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            CycleMask();
    }

    void CycleMask()
    {
        masks[currentIndex].SetActive(false);

        movement.ResetMoveSpeed();
        punch.ResetAttackSpeed();
        punch.ResetDamage();
        health.ResetDamageMultiplier();

        currentIndex = (currentIndex + 1) % masks.Length;

        masks[currentIndex].SetActive(true);
        ApplyMaskEffects();
    }

    void ApplyMaskEffects()
    {
        switch (currentIndex)
        {
            case 0:
                break;

            case 1:
                movement.SetMoveSpeedMultiplier(1.3f);
                punch.SetAttackSpeedMultiplier(1.4f);
                break;

            case 2:
                punch.SetDamage(0);
                health.SetDamageMultiplier(0.1f);
                break;
        }
    }
}
