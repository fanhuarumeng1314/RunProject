using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMode : MonoBehaviour {

    public static GameMode instance;                        
    [HideInInspector]
    public GameObject guidObj;                                 //引导物体
    public Transform guideTrs;                                 //存储下一步生成道路坐标以及位置信息
    public GameObject roadTemplate;                            //道路模板
    int buidfound = 0;                                         //确定转向后的回合数
    public List<GameObject> roads;                             //保存现在能够进行还原的道路
    public bool isBuidDirRoad;                                 //是否生成方向道路
    int dirRoadType;                                           //方向道路的类型
    int dirRoadNumber;                                         //方向道路的数量
    [HideInInspector]
    public int goldNumber;                                     //吃到的金币数
    public Text numberText;                                    //数量的显示文本
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        numberText = GameObject.Find("Canvas/GoldNumber").GetComponent<Text>();
        guidObj = Instantiate(roadTemplate);
        guidObj.transform.position = Vector3.zero;
        guidObj.transform.rotation = Quaternion.identity;
        guidObj.name = "Guid";
        guideTrs = guidObj.transform;                         //上面实例化生成引导物体并存储下一步的道路信息

        for (int i=0;i<20;i++)                                //预先生成20格道路作为跑道
        {
            var tmpRoad = Instantiate(roadTemplate,guideTrs.position,guideTrs.rotation);
            guideTrs.position += guideTrs.forward;           //每生成一个道路，引导物体的位置改变
        }

        for (int i=0;i<30;i++)
        {
            BuidRoad();                                         //先生成30个动画效果
        }
    }

    private void Update()
    {
        numberText.text ="当前金币数："+ goldNumber.ToString();
    }

    /// <summary>
    /// 生成道路的方法
    /// </summary>
    public void BuidRoad()
    {
        if (isBuidDirRoad && dirRoadNumber > 0)
        {
            switch (dirRoadType)
            {
                case 1:
                    BuidUpTerrain();
                    dirRoadNumber--;
                    break;
                case 2:
                    BuidDownTerrain();
                    dirRoadNumber--;
                    break;
                case 3:
                    BuidLeftTerrain();
                    dirRoadNumber--;
                    break;
                case 4:
                    BuidRightTerrain();
                    dirRoadNumber--;
                    break;
            }

            if (dirRoadNumber <= 0)
            {
                isBuidDirRoad = false;
            }
        }
        else
        {
            int turnSeed = Random.Range(1, 12);                                  //用于确定是否转向

            if (turnSeed == 1 && buidfound <= 0)
            {
                buidfound = 10;                                                 //回合数更新
                int dictSeed = Random.Range(1, 3);
                for (int i = 0; i < 3; i++)                                     //先生成3个格子的道路，作为转向区
                {
                    var tmpRoad = Instantiate(roadTemplate, guideTrs.position, guideTrs.rotation);
                    PlayRoadAnimator(tmpRoad);
                    roads.Add(tmpRoad);
                    guideTrs.position += guideTrs.forward;
                }
                if (dictSeed == 1)
                {
                    guideTrs.position -= guideTrs.forward * 2;     //转向区域生成完成后，引导物体回退2格
                    guideTrs.Rotate(Vector3.up, 90);               //转向
                    guideTrs.position += guideTrs.forward * 2;     //转向后引导物体的forward轴改变，改变pos值，到达下一个道路的生成地点
                }
                else
                {
                    guideTrs.position -= guideTrs.forward * 2;
                    guideTrs.Rotate(Vector3.up, -90);
                    guideTrs.position += guideTrs.forward * 2;
                }
            }
            else if (turnSeed == 3)
            {
                int trunTerrain = Random.Range(1, 5);
                isBuidDirRoad = true;
                dirRoadType = trunTerrain;
                dirRoadNumber = 10;
                switch (trunTerrain)
                {
                    case 1:
                        BuidUpTerrain();
                        dirRoadNumber--;
                        break;
                    case 2:
                        BuidDownTerrain();
                        dirRoadNumber--;
                        break;
                    case 3:
                        BuidLeftTerrain();
                        dirRoadNumber--;
                        break;
                    case 4:
                        BuidRightTerrain();
                        dirRoadNumber--;
                        break;
                }
            }
            else if (turnSeed==6)
            {
                BuidTrapRoad();
            }
            else
            {
                var tmpRoad = Instantiate(roadTemplate, guideTrs.position, guideTrs.rotation);
                PlayRoadAnimator(tmpRoad);
                roads.Add(tmpRoad);
                guideTrs.position += guideTrs.forward;              //每生成一个道路，引导物体的位置改变
            }
            buidfound--;
        }
    }
    /// <summary>
    /// 生成上升地形
    /// </summary>
    public void BuidUpTerrain()
    {
        var tmpRoad = Instantiate(roadTemplate, guideTrs.position, guideTrs.rotation);
        PlayRoadAnimator(tmpRoad);
        roads.Add(tmpRoad);
        guideTrs.position += guideTrs.forward;
        guideTrs.position += guideTrs.up * 0.2f;
    }
    /// <summary>
    /// 生成下降地图
    /// </summary>
    public void BuidDownTerrain()
    {
        var tmpRoad = Instantiate(roadTemplate, guideTrs.position, guideTrs.rotation);
        PlayRoadAnimator(tmpRoad);
        roads.Add(tmpRoad);
        guideTrs.position += guideTrs.forward;
        guideTrs.position -= guideTrs.up * 0.2f;
    }
    /// <summary>
    /// 生成左斜地图
    /// </summary>
    public void BuidLeftTerrain()
    {
        var tmpRoad = Instantiate(roadTemplate, guideTrs.position, guideTrs.rotation);
        PlayRoadAnimator(tmpRoad);
        roads.Add(tmpRoad);
        guideTrs.position += guideTrs.forward;
        guideTrs.position -= guideTrs.right * 0.2f;

    }
    /// <summary>
    /// 生成右斜地图
    /// </summary>
    public void BuidRightTerrain()
    {
        var tmpRoad = Instantiate(roadTemplate, guideTrs.position, guideTrs.rotation);
        PlayRoadAnimator(tmpRoad);
        roads.Add(tmpRoad);
        guideTrs.position += guideTrs.forward;
        guideTrs.position += guideTrs.right * 0.2f;

    }
    /// <summary>
    /// 生成陷阱地图
    /// </summary>
    public void BuidTrapRoad()
    {
        guideTrs.position += guideTrs.forward;
        var tmpRoad = Instantiate(roadTemplate, guideTrs.position, guideTrs.rotation);
        var tmpController = tmpRoad.GetComponent<RoadController>();
        tmpController.ChangeRoadType();
        tmpRoad.transform.Rotate(Vector3.up,90);
        int trapType = Random.Range(1,4);                                   //用于确定陷阱的相对位置--1左边,2居中,3右边！
        switch (trapType)
        {
            case 1:
                tmpRoad.transform.position += tmpRoad.transform.forward;
                break;
            case 2:
                break;
            case 3:
                tmpRoad.transform.position -= tmpRoad.transform.forward;
                break;
        }
        guideTrs.position += guideTrs.forward * 2.0f;
        PlayRoadAnimator(tmpRoad);
        roads.Add(tmpRoad);
    }
    /// <summary>
    /// 播放道路动画
    /// </summary>
    /// <param name="road"></param>
    public void PlayRoadAnimator(GameObject road)
    {
        var tmpRoadController = road.GetComponent<RoadController>();
        if (tmpRoadController==null) { return; }
        tmpRoadController.ChangeChildrens();

    }
    /// <summary>
    /// 关闭动画
    /// </summary>
    public void CloseRoadAnimator()
    {
        if (roads.Count<=0) { return; }
        var tmpRoadController = roads[0].GetComponent<RoadController>();
        if (tmpRoadController!=null)
        {
            tmpRoadController.InitChildrens();
        }
        roads.RemoveAt(0);
    }
}
