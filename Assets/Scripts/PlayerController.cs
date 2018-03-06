using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;                                                  //引入DoTween插件

public class PlayerController : MonoBehaviour {

    public CharacterController playController;
    public Animator playAnimtor;
    public Vector3 MoveIncrements;
    [SerializeField]
    float transverseSpeed = 5.0f;                               //玩家横向的移动速度
    public float moveSpeed = 6.0f;                           //玩家的游戏移动速度
    [HideInInspector]
    public GameObject nowRoad;                            //现在玩家脚下的道路
    bool isTurnleftEnd = true;                                   //左转向是否完成
    bool isTurnRightEnd = true;                                //右转向是否完成
    void Start ()
    {
        playController = GetComponent<CharacterController>();
        playAnimtor = GetComponent<Animator>();

    }
	void Update ()
    {
        moveSpeed += Time.deltaTime*0.3f;
        float moveDir = Input.GetAxis("Horizontal");
        MoveIncrements = transform.forward * moveSpeed * Time.deltaTime;
        MoveIncrements += transform.right * transverseSpeed * Time.deltaTime*moveDir;
        MoveIncrements.y += playController.isGrounded ? 0f : Physics.gravity.y * Time.deltaTime * 1f;       //更新重力
        playController.Move(MoveIncrements);
        playAnimtor.SetFloat("MoveSpeed",playController.velocity.magnitude);                                           //动画状态更新
        if (Input.GetKeyDown(KeyCode.J) && isTurnleftEnd)
        {
            isTurnleftEnd = false;                                                                                                                     //更新转向状态
            transform.Rotate(Vector3.up,-90);
            Quaternion tmpQuaternion = transform.rotation;                                                                         //计算转向后的四元数并保存
            transform.Rotate(Vector3.up, 90);                                                                                                 //角度回滚
            Tween tween = transform.DORotateQuaternion(tmpQuaternion, 0.3f);                                        //使用DoTween插件进行转向的平滑运动
            tween.OnComplete(() => isTurnleftEnd = true);                                                                           //动画结束后转向状态更新
        }
        if (Input.GetKeyDown(KeyCode.L) && isTurnRightEnd)
        {
            isTurnRightEnd = false;
            transform.Rotate(Vector3.up,90);
            Quaternion tmpQuaternion = transform.rotation;
            transform.Rotate(Vector3.up, -90);
            Tween tween = transform.DORotateQuaternion(tmpQuaternion, 0.3f);
            tween.OnComplete(() => isTurnRightEnd = true);
        }

    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject!=nowRoad)                                                 //去重复避免删除错误
        {
            nowRoad = hit.gameObject;
            Destroy(hit.gameObject,1.0f);
            GameMode.instance.BuidRoad();                                        //生成道路
            GameMode.instance.CloseRoadAnimator();
        }
    }
}
