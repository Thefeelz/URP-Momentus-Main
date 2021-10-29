using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_CoolDownManager : MonoBehaviour
{
    List<A_OverchargeAbilities> cooldownList = new List<A_OverchargeAbilities>();
    bool updateTheList = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(cooldownList.Count > 0)
            UpdateCoolDownList();
    }

    public void AddCooldownToList(A_OverchargeAbilities newAbility)
    {
        cooldownList.Add(newAbility);
        newAbility.StartAbilityCooldown();
        updateTheList = true;
    }

    void UpdateCoolDownList()
    {
        for(int i = 0; i < cooldownList.Count; i++)
        {
            if (cooldownList[i].abilityCooldownCurrent > 0)
                cooldownList[i].DecrementCooldownDuration(Time.deltaTime);
            else
            {
                cooldownList[i].abilityCooldownCurrent = 0;
                cooldownList[i].abilityReady = true;
                cooldownList[i].StartUIImageFlash();
                cooldownList.Remove(cooldownList[i]);
            }
        }
    }
}
