using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    public float JumpPower;
    [HideInInspector]public enum JumpTypeEnum {All, Only};
    public JumpTypeEnum JumpType;
    public GameObject OnlyJumpObject;

    void OnTriggerEnter(Collider Other)
    {
        if (Other.tag == "Player")
        {
            if (JumpType == JumpTypeEnum.All)
            {
            	var rigid = Other.GetComponent<Rigidbody>();
                if (rigid != null)
                {
                    rigid.AddForce(Vector3.up * JumpPower * rigid.mass, ForceMode.VelocityChange);
                }
            }
            else
            {
                if (Other.gameObject == OnlyJumpObject)
                {
                    OnlyJumpObject.GetComponent<Rigidbody>().AddForce(Vector3.up * JumpPower, ForceMode.VelocityChange);
                }
            }
        }
    }
}
