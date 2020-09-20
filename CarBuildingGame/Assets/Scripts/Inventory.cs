using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<GameObject> details;
    [SerializeField] private List<int> count;
    private List<GameObject> tempDetails;
    private List<int> tempCount;

    public void newTry(List<GameObject> detailsFromGarage, List<int> countFromGarage)
    {
        details = detailsFromGarage;
        count = countFromGarage;
        tempDetails = new List<GameObject>();
        tempCount = new List<int>();
    }

    public void Merge()
    {
        for (int i = 0; i < tempDetails.Count; i++)
        {
            bool containsNewDetail = false;
            foreach (GameObject detail in details)
            {
                if (detail.CompareTag(tempDetails[i].tag))
                {
                    containsNewDetail = true;
                    break;
                }
            }
            if (containsNewDetail)
            {
                count[i] += tempCount[i];
            }
            else
            {
                details.Add(tempDetails[i]);
                count.Add(tempCount[i]);
            }
        }
    }

    public void PickedUp(GameObject pickedDetail)
    {
        bool containsPickedDetail = false;
        foreach (GameObject detail in tempDetails)
        {
            if (detail.CompareTag(pickedDetail.tag))
            {
                containsPickedDetail = true;
                break;
            }
        }
        if (containsPickedDetail)
        {
            tempCount[tempDetails.IndexOf(pickedDetail)]++;
        }
        else
        {
            tempDetails.Add(pickedDetail);
            tempCount.Add(1);
        }
    }

    public List<GameObject> GetDetails()
    {
        return details;
    }
    public List<int> GetCountOfDetails()
    {
        return count;
    }

}