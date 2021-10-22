using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MasterLevel : MonoBehaviour
{
    [SerializeField] int enemiesToKill;
    [SerializeField] int currentKillCount;
    [SerializeField] Image progressBar;
    [SerializeField] Text completeText;
    [SerializeField] Door levelContinueDoor;
    bool levelComplete;

    private void Start()
    {
        CheckForLevelComplete();
        currentKillCount = 0;
    }

    public void AddToKillCount(int numberToAdd)
    {
        currentKillCount += numberToAdd;
        progressBar.fillAmount = (float)currentKillCount / (float)enemiesToKill;
        CheckForLevelComplete();
    }

    public bool CheckForLevelComplete()
    {
        if (!levelComplete)
        {
            if (currentKillCount >= enemiesToKill)
            {
                StartCoroutine(ShowWinText());
                levelContinueDoor.SetOpenDoor();
                levelComplete = true;
                return true;
            }
            else
            {
                completeText.gameObject.SetActive(false);
                return false;
            }
        }
        return true;
    }
    IEnumerator ShowWinText()
    {
        completeText.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);
        completeText.gameObject.SetActive(false);
        StopCoroutine(ShowWinText());
    }
}
