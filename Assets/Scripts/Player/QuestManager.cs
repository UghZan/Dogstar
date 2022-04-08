using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * types: 
 * Fetch - requires origin station, needed item id/amount
 * Deliver - requires origin station, target station, needed item id/amount
 * Scan - requires target pos, origin station
 * Find - requires target object, origin station
 * 
 */
public enum type
{
    Fetch,
    Deliver,
    Survey,
    Find
}
public class Quest
{
    public string name = "Quest";
    public string desc = "Desc";
    public type type;
    public int moneyReward = 100;
    public int neededItemID = -1;
    public int neededItemAmount = 0;
    public int stationOrigin = -1;
    public int stationTarget = -1;
    public Vector3 targetPos = Vector3.zero;
    public GameObject targetObject = null;


    public Quest(type t)
    {
        type = t;
    }
}


public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnQuestTake(Quest q)
    {

    }
}
