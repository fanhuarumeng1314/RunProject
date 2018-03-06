using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour {

    public static GameMode instance;                        
    [HideInInspector]
    public GameObject guidObj;                                 //引导物体
    public Transform guideTrs;                                 //存储下一步生成道路坐标以及位置信息
    public GameObject roadTemplate;                            //道路模板
    int buidfound = 0;                                         //确定转向后的回合数
    public List<GameObject> roads;                             //保存现在能够进行还原的道路

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
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

        for (int i=0;i<15;i++)
        {
            BuidRoad();                                         //先生成15个动画效果
        }
    }

    /// <summary>
    /// 生成道路的方法
    /// </summary>
    public void BuidRoad()
    {
        int turnSeed = Random.Range(1,10);                                  //用于确定是否转向
        if (turnSeed == 1 && buidfound<=0)
        {
            buidfound = 10;                                                 //回合数更新
            int dictSeed = Random.Range(1,3);
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
        else
        {
            var tmpRoad = Instantiate(roadTemplate, guideTrs.position, guideTrs.rotation);
            PlayRoadAnimator(tmpRoad);
            roads.Add(tmpRoad);
            guideTrs.position += guideTrs.forward;              //每生成一个道路，引导物体的位置改变
        }
        buidfound--;
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
