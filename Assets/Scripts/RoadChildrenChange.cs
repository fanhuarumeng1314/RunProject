using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RoadChildrenChange : MonoBehaviour {

    public Vector3 firstLocalPos;                     //保存最开始的相对坐标
    public Quaternion firstLocalRotation;             //保存最开始的相对旋转
    public bool isTurn;                               //旋转开关
    public GameObject parentObj;                      //现在的父物体
    public float time = 0.5f;                         //现在的还原时间

    private void Awake()
    {
        firstLocalPos = transform.localPosition;
        firstLocalRotation = transform.localRotation;
        parentObj = transform.parent.gameObject;
        isTurn = false;
    }
    
	void Update ()
    {
        if (time>=0.1f)
        {
            time -= Time.deltaTime * 0.1f;
        }
        if (isTurn)
        {
            transform.RotateAround(parentObj.transform.position, parentObj.transform.forward, 30 * Time.deltaTime);
        }
	}
    /// <summary>
    /// 改变这个物体的相对坐标
    /// </summary>
    public void PosChange()
    {
        int changeValueUp = 0;
        int chageValueRight = 0;
        while (Mathf.Abs(changeValueUp)<=4.0f)
        {
            changeValueUp = Random.Range(-10,10);
        }

        while (Mathf.Abs(chageValueRight) <= 4.0f)
        {
            chageValueRight = Random.Range(-10, 10);
        }

        transform.localPosition += transform.up * changeValueUp;
        transform.localPosition += transform.right * chageValueRight;
    }
    /// <summary>
    /// 改变这个物体的相对旋转
    /// </summary>
    public void ChangeRotate()
    {
        transform.Rotate(transform.right, Random.Range(0, 180));
        transform.Rotate(transform.forward, Random.Range(0, 180));
        transform.Rotate(transform.up, Random.Range(0, 180));
    }
    /// <summary>
    /// 初始化物体相对旋转与坐标
    /// </summary>
    public void Init()
    {
        transform.DOLocalMove(firstLocalPos, time);
        transform.DOLocalRotateQuaternion(firstLocalRotation, time);
    }

}
