﻿using UnityEditor;
using UnityEngine;

public class PlayerInfoUI : MonoBehaviour
{
    public bool showPlayerInfo;                     // 是否显示玩家信息
    public Font font;                               // 文本字体
    public int fontSize = 18;                       // 字体大小
    public float offset = 5;                        // 文本相对玩家偏移量
    public float vibrateRange = 2;                  // 文本抖动范围的平方（在范围内位置平滑移动，预防抖动）
    public float labelMoveSpeed = 0.1f;             // 标签移动速度

    public int ID { get { return playerID; } }                  // 玩家ID
    public string Name { get { return playerName; } }           // 玩家名
    public TeamManager Team { get { return playerTeam; } }      // 玩家所在团队
    public bool Active { get { return playerActive; } }         // 玩家是否有效
    public bool IsAI { get { return playerAI; } }               // 玩家是否是AI
    public string ColoredName { get { return coloredPlayerName; } }   // 获取带颜色的玩家名

    private int playerID;                           // 玩家ID
    private string playerName;                      // 玩家名
    private bool playerActive;                      // 玩家是否激活
    private bool playerAI;                          // 玩家是否是AI
    private TeamManager playerTeam;                 // 玩家所在团队
    private string coloredPlayerName;               // 带颜色的玩家名

    private Camera targetCamera;                    // 显示对着的镜头（默认：主相机）
    private GUIStyle style;                         // GUI风格  
    private Vector2 nameLabelSize;                  // 文本大小
    private Vector3 lastScreenPosition = Vector3.zero;  // 上一帧文本对应屏幕位置

    public void SetPlayerID(int playerID)
    {
        this.playerID = playerID;
    }

    public void SetPlayerTeam(TeamManager team)
    {
        playerTeam = team;
    }

    public void SetPlayerActive(bool active)
    {
        playerActive = active;
    }

    public void SetPlayerAI(bool isAI)
    {
        playerAI = isAI;
    }

    /// <summary>
    /// 配置玩家ID、玩家名、玩家所在团队
    /// </summary>
    /// <param name="playerID">玩家ID</param>
    /// <param name="playerName">玩家名</param>
    /// <param name="playerTeam">玩家所在团队</param>
    public void SetupPlayer(int playerID,string playerName,TeamManager playerTeam,bool playerActive,bool playerAI)
    {
        this.playerID = playerID;
        this.playerName = playerName;
        this.playerTeam = playerTeam;
        this.playerActive = playerActive;
        this.playerAI = playerAI;

        coloredPlayerName = playerName;                                             // 玩家名字富文本
        if (playerTeam != null)
            coloredPlayerName = "<color=#" + ColorUtility.ToHtmlStringRGB(playerTeam.TeamColor) + ">" + playerName + "</color>";
        }

    /// <summary>
    /// 获取目标镜头和玩家碰撞体
    /// </summary>
    private void Awake()
    {
        targetCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        SetupGUIStyle();
    }

    /// <summary>
    /// 初始化GUI风格
    /// </summary>
    private void SetupGUIStyle()
    {
        style = new GUIStyle(EditorStyles.largeLabel);
        style.alignment = TextAnchor.MiddleCenter;                  // 文本锚点左下角
        style.fontSize = fontSize;                                  // 字体大小
        style.font = font;                                          // 文本字体
        style.normal.textColor = new Color(0.9f, 0.9f, 0.9f, 1f);   // 字体默认颜色 
    }

    /// <summary>
    /// 设置显示到镜头玩家名字
    /// </summary>
    /// <param name = "name" > 名字 </ param >
    public void SetNameText(string name)
    {
        playerName = name;
        nameLabelSize = style.CalcSize(new GUIContent(playerName)); // 计算获取文本标签大小
    }

    /// <summary>
    /// 设置名字颜色
    /// </summary>
    /// <param name = "color" > 名字颜色 </ param >
    public void SetNameColor(Color color)
    {
        style.normal.textColor = color;
    }

    /// <summary>
    /// 根据距离计算并显示玩家信息
    /// </summary>
    private void OnGUI()
    {
        if (!showPlayerInfo)
            return;

        //绘制名字
        GUI.Label(CalculatePosition(), playerName, style);
    }

    /// <summary>
    /// 计算文本位置，并返回位置对应Rect
    /// </summary>
    /// <returns>位置</returns>
    private Rect CalculatePosition()
    {
        // 计算获取文本对应屏幕位置
        Vector3 screenPosition = targetCamera.WorldToScreenPoint(transform.position + offset * targetCamera.transform.up);
        screenPosition.y = Screen.height - screenPosition.y;    //翻转Y坐标值（screenPosition原点在左上角？？）

        Rect rect = new Rect(Vector2.zero, nameLabelSize);

        //和上一次位置距离在浮动范围，就平滑移动到上一次的位置
        if ((lastScreenPosition - screenPosition).sqrMagnitude < vibrateRange)
        {
            lastScreenPosition = Vector3.MoveTowards(lastScreenPosition, screenPosition, labelMoveSpeed);
            rect.center = lastScreenPosition;
        }
        else
        {
            rect.center = screenPosition;
            lastScreenPosition = screenPosition;
        }
        // 根据文本大小设置位置
        return rect;
    }

}
