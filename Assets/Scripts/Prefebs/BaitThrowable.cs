using Unity.Behavior;
using UnityEngine;

public class BaitThrowable : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 15f);
    }


    private void OnCollisionEnter(Collision collision)
    {
        //GameManager.Instance.GetBoss().GetComponent<BehaviorGraphAgent>().BlackboardReference.SetVariableValue("LastBaitLocation", transform);
        GameManager.Instance.boss.GetComponent<BehaviorGraphAgent>().BlackboardReference.Blackboard.Variables.Find(v => v.Name == "LastBaitLocation").ObjectValue = transform;
        GameManager.Instance.boss.GetComponent<BehaviorGraphAgent>().BlackboardReference.Blackboard.Variables.Find(v => v.Name == "LastBaitObject").ObjectValue = this.gameObject;
    }
}
