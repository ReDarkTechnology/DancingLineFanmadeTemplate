using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularAnimation : MonoBehaviour
{
    [Header("Object")]
    public GameObject Object;

    [Header("Position Animations")]
    public float PositionX;
    public bool AnimatePosX;
    public float PositionY;
    public bool AnimatePosY = true;
    public float PositionZ;
    public bool AnimatePosZ;

    [Header("Rotation Animations")]
    public float RotationX;
    public bool AnimateRotX;
    public float RotationY;
    public bool AnimateRotY;
    public float RotationZ;
    public bool AnimateRotZ;

    [Header("Scale Animations")]
    //public float ScaleX;
    //public bool AnimateScaleX;
    //public float ScaleY;
    //public bool AnimateScaleY;
    //public float ScaleZ;
    //public bool AnimateScaleZ;
    public GameObject TargetScaleObject;
    public bool AnimateScale;

    [Header("Misc")]
    public float speed = 5;
    public float endTime = 90;
    int CheckpointSort;
    GameManager core;
    [HideInInspector] public bool isMoving;
    [HideInInspector] public Vector3 TargetPos;
    [HideInInspector] public Vector3 TargetRot;
    [HideInInspector] public Vector3 TargetScale;
    [HideInInspector] public Vector3 StarterPosition;
    [HideInInspector] public Vector3 StarterRotation;
    [HideInInspector] public Vector3 StarterScale;
    [HideInInspector] public Vector3 TargetPosGizmos;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.tag = "Trigger";
        core = UnityEngine.Object.FindObjectOfType<GameManager>();
        core.OnCheckpointObtained += Core_OnCheckpointObtained;
        core.OnCheckpointReset += ResetObject;
        CreateVariables();
        StarterScale = Object.transform.localScale;
        StarterPosition = Object.transform.position;
        StarterRotation = Object.transform.eulerAngles;
    }

    private void Core_OnCheckpointObtained(int obj)
    {
        CheckpointSort = obj;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving == true)
        {
            Object.transform.position = Vector3.Lerp(Object.transform.position, TargetPos, speed * Time.deltaTime);
            Object.transform.eulerAngles = Vector3.Lerp(Object.transform.eulerAngles, TargetRot, speed * Time.deltaTime);
            Object.transform.localScale = Vector3.Lerp(Object.transform.localScale, TargetScale, speed * Time.deltaTime);
        }
    }
    public void CreateVariables()
    {
        //PositionVar
        if (AnimatePosX && AnimatePosY && AnimatePosZ)
        {
            TargetPos = new Vector3(PositionX, PositionY, PositionZ);
        }
        if (AnimatePosY && AnimatePosZ)
        {
            TargetPos = new Vector3(Object.transform.position.x, PositionY, PositionZ);
        }
        if (AnimatePosX && AnimatePosY)
        {
            TargetPos = new Vector3(PositionX, PositionY, Object.transform.position.z);
        }
        if (AnimatePosX && AnimatePosZ)
        {
            TargetPos = new Vector3(PositionX, Object.transform.position.y, PositionZ);
        }
        if (AnimatePosX)
        {
            TargetPos = new Vector3(PositionX, Object.transform.position.y, Object.transform.position.z);
        }
        if (AnimatePosY)
        {
            TargetPos = new Vector3(Object.transform.position.x, PositionY, Object.transform.position.z);
        }
        if (AnimatePosZ)
        {
            TargetPos = new Vector3(Object.transform.position.x, Object.transform.position.y, PositionZ);
        }
        if (!AnimatePosX && !AnimatePosY && !AnimatePosZ)
        {
            TargetPos = new Vector3(Object.transform.position.x, Object.transform.position.y, Object.transform.position.z);
        }
        //RotationVar
        if (AnimateRotX && AnimateRotY && AnimateRotZ)
        {
            TargetRot = new Vector3(RotationX, RotationY, RotationZ);
        }
        if (AnimateRotY && AnimateRotZ)
        {
            TargetRot = new Vector3(Object.transform.eulerAngles.x, RotationY, RotationZ);
        }
        if (AnimateRotX && AnimateRotY)
        {
            TargetRot = new Vector3(RotationX, RotationY, Object.transform.eulerAngles.z);
        }
        if (AnimateRotX && AnimateRotZ)
        {
            TargetRot = new Vector3(RotationX, Object.transform.eulerAngles.y, RotationZ);
        }
        if (AnimateRotX)
        {
            TargetRot = new Vector3(RotationX, Object.transform.eulerAngles.y, Object.transform.eulerAngles.z);
        }
        if (AnimateRotY)
        {
            TargetRot = new Vector3(Object.transform.eulerAngles.x, RotationY, Object.transform.eulerAngles.z);
        }
        if (AnimateRotZ)
        {
            TargetRot = new Vector3(Object.transform.eulerAngles.x, Object.transform.eulerAngles.y, RotationZ);
        }
        if (!AnimateRotX && !AnimateRotY && !AnimateRotZ)
        {
            TargetRot = new Vector3(Object.transform.eulerAngles.x, Object.transform.eulerAngles.y, Object.transform.eulerAngles.z);
        }
        //ScaleVar
        /*if(AnimateScaleX && AnimateScaleY && AnimateScaleZ){
            TargetScale = new Vector3(ScaleX,ScaleY,ScaleZ);
        }
        if(AnimateScaleY && AnimateScaleZ){
            TargetScale = new Vector3(Object.transform.localScale.x,ScaleY,ScaleZ);
        }
        if(AnimateScaleX && AnimateScaleY){
            TargetScale = new Vector3(ScaleX,ScaleY,Object.transform.localScale.z);
        }
        if(AnimateScaleX && AnimateScaleZ){
            TargetScale = new Vector3(ScaleX,Object.transform.localScale.y,ScaleZ);
        }
        if(AnimateScaleX){
            TargetScale = new Vector3(ScaleX,Object.transform.localScale.y,Object.transform.localScale.z);
        }
        if(AnimateScaleY){
            TargetScale = new Vector3(Object.transform.localScale.x,ScaleY,Object.transform.localScale.z);
        }
        if(AnimateScaleZ){
            TargetScale = new Vector3(Object.transform.localScale.x,Object.transform.localScale.y,ScaleZ);
        }
        if(!AnimateScaleX && !AnimateScaleY && !AnimateScaleZ){
            //TargetScale = new Vector3(Object.transform.localScale.x,Object.transform.localScale.y,Object.transform.localScale.z);
        }*/
        if (AnimateScale)
        {
            TargetScale = TargetScaleObject.transform.localScale;
        }
        else
        {
            TargetScale = Object.transform.localScale;
        }
    }
    public void StopAnimate()
    {
        isMoving = false;
    }
    public void ResetObject(int checkSort)
    {
        if (checkSort == CheckpointSort)
        {
            StopAnimate();
            CancelInvoke("StopAnimate");
            Object.transform.position = StarterPosition;
            Object.transform.eulerAngles = StarterRotation;
            Object.transform.localScale = StarterScale;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //CheckpointSort = core.checkpointGot;
            isMoving = true;
            Invoke("StopAnimate", endTime);
        }
    }
    void OnGizmosSelected()
    {
        if (AnimatePosX && AnimatePosY && AnimatePosZ)
        {
            TargetPosGizmos = new Vector3(PositionX, PositionY, PositionZ);
        }
        if (AnimatePosY && AnimatePosZ)
        {
            TargetPosGizmos = new Vector3(Object.transform.position.x, PositionY, PositionZ);
        }
        if (AnimatePosX && AnimatePosY)
        {
            TargetPosGizmos = new Vector3(PositionX, PositionY, Object.transform.position.z);
        }
        if (AnimatePosX && AnimatePosZ)
        {
            TargetPosGizmos = new Vector3(PositionX, Object.transform.position.y, PositionZ);
        }
        if (AnimatePosX)
        {
            TargetPosGizmos = new Vector3(PositionX, Object.transform.position.y, Object.transform.position.z);
        }
        if (AnimatePosY)
        {
            TargetPosGizmos = new Vector3(Object.transform.position.x, PositionY, Object.transform.position.z);
        }
        if (AnimatePosZ)
        {
            TargetPosGizmos = new Vector3(Object.transform.position.x, Object.transform.position.y, PositionZ);
        }
        if (!AnimatePosX && !AnimatePosY && !AnimatePosZ)
        {
            TargetPosGizmos = new Vector3(Object.transform.position.x, Object.transform.position.y, Object.transform.position.z);
        }
        Gizmos.DrawLine(transform.position, TargetPosGizmos);
    }
}