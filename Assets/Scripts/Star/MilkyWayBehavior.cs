using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkyWayBehavior : StarBase
{
    private class LandInMW
    {
        public GameObject land;
        public Vector3 diff;

        public LandInMW(GameObject land, Vector3 diff)
        {
            this.land = land;
            this.diff = diff;
        }
    }
    [SerializeField] private List<LandInMW> LandList; // 同じマスにいてIN_MILKYWAY_AREAフラグの立っているLandのリスト

    [SerializeField] private GameObject m_ObjEatParticle;

    public MilkyWayBehavior() : base(StarType.MilkyWay)
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        // 同じマスでまだ捕まえてないLandを自身の場所まで移動させる。
        foreach(LandInMW landmw in LandList)
        {
            var landScript = landmw.land.GetComponent<LandStarController>();

            float time = Time.deltaTime;

            if(landScript.timeToCirculate <= Time.deltaTime + landScript.timePast) 
            {
                time = landScript.timeToCirculate - landScript.timePast;
            }

            landmw.land.transform.position += landmw.diff * time;
            landScript.timePast += time;

            if(landScript.timeToCirculate <= landScript.timePast)
            {
                landScript.AddStat(LandStarController.LANDSTAR_STAT.CAUGHT_BY_MILKYWAY);
                landScript.RemoveFlag(LandStarController.LANDSTAR_STAT.MOVING);
                LandList.Remove(landmw);
                break;
            }
        }
    }

    void Init()
    {
        LandList = new List<LandInMW>();
    }

    public override void TriggerOtherComeToSameCell(GameObject other)
    {
        if(other.tag == ObjectTag.Land)
        {
            var otherScript = other.GetComponent<LandStarController>();
            otherScript.AddStat(LandStarController.LANDSTAR_STAT.IN_MILKYWAY_AREA);
            var diff = (transform.position - other.gameObject.transform.position) / (otherScript.timeToCirculate - otherScript.timePast);

            if(LandList == null) // 生まれたときにStart関数を通っているときと通っていないときがあるので.
            {
                Init();
            }

            LandList.Add(new LandInMW(other, diff));
            Instantiate(m_ObjEatParticle, transform.position, transform.rotation);
        }
    }
}
