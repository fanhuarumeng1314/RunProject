using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RoadChildrenChange : MonoBehaviour {

    public Vector3 firstLocalPos;                     //保存最开始的相对坐标
    public Quaternion firstLocalRotation;             //保存最开始的相对旋转
    public bool isTurn;                               //旋转开关
    public GameObject parentObj;                      //现在的父物体
    public float time = 0.3f;                         //现在的还原时间
    public RoadType nowType;                          //现在的道路类型
    public GameObject gold;                           //金币物体
    Quaternion nowQuat;                               //金币的初始旋转
    private void Awake()
    {
        time = 0.8f;
        firstLocalPos = transform.localPosition;
        firstLocalRotation = transform.localRotation;
        parentObj = transform.parent.gameObject;
        isTurn = false;
    }

    private void Start()
    {
        gold = Resources.Load("Gold") as GameObject;            //读取文件金币的预制体
        bool isCreat = Random.Range(1, 10) == 3 ? true : false; //是否生成金币
        if (isCreat)
        {
            Vector3 tmpPos = transform.position;
            gold = Instantiate(gold, tmpPos += transform.up * 1.2f, Quaternion.identity);
            gold.transform.SetParent(transform);
            nowQuat = gold.transform.rotation;                  //保存当前金币的旋转，方便初始化。
        }
    }

    void Update ()
    {
        if (time>=0.1f)
        {
            time -= Time.deltaTime * 0.15f;
        }
        if (isTurn && nowType == RoadType.road)
        {
            transform.RotateAround(parentObj.transform.position, parentObj.transform.forward, 30 * Time.deltaTime);
        }
        else if (isTurn && nowType==RoadType.trap)
        {
            transform.RotateAround(parentObj.transform.position, parentObj.transform.right, 30 * Time.deltaTime);
        }
        if (gold != null)
        {
            gold.transform.Rotate(Vector3.up, 70 * Time.deltaTime);
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
        Tween t = transform.DOLocalRotateQuaternion(firstLocalRotation, time);
        t.OnComplete(InitGold);
    }
    /// <summary>
    /// 初始化金币数据并开始旋转开关
    /// </summary>
    public void InitGold()
    {
        gold.transform.rotation = nowQuat;
    }


}
