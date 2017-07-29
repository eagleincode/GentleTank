﻿using UnityEngine;

public class TankInformation : MonoBehaviour
{
    [HideInInspector]
    public int playerID;                            // 玩家ID
    [HideInInspector]
    public string playerName;                       // 玩家名
    [HideInInspector]
    public bool playerActive;                       // 玩家是否激活
    [HideInInspector]
    public bool playerAI;                           // 玩家是否是AI
    [HideInInspector]
    public Color playerColor;                       // 玩家的颜色
    [HideInInspector]
    public TeamManager playerTeam;                  // 玩家所在团队
    [HideInInspector]
    public int playerTeamID = -1;                   // 玩家所在团队ID（没队的-1）
    [HideInInspector]
    public string playerColoredName;                // 带颜色的玩家名

    public void SetupTankInfo(int id,string name,bool active, bool isAI,Color color,TeamManager team = null,string coloredName = null)
    {
        playerID = id;
        playerName = name;
        playerActive = active;
        playerAI = isAI;
        playerColor = color;
        playerTeam = team;
        playerColoredName = coloredName == null ? name : coloredName;
        if (playerTeam != null)
            playerTeamID = playerTeam.TeamID;
    }

    /// <summary>
    /// 渲染所有带'T'类的脚本的子组件们颜色
    /// </summary>
    /// <param name="color">渲染颜色</param>
    public void RendererColorByComponent<T>(Color color) where T : Component
    {
        T[] renderChildren = GetComponentsInChildren<T>();
        if (renderChildren == null)
            return;
        for (int i = 0; i < renderChildren.Length; i++)
            SetMeshRendererColor(renderChildren[i], color);
    }

    /// <summary>
    /// 设置渲染网眼颜色
    /// </summary>
    /// <param name="gameObject">设置对象</param>
    /// <param name="color">渲染颜色</param>
    private void SetMeshRendererColor(Component needRenderComponent, Color color)
    {
        MeshRenderer[] meshRenderer = needRenderComponent.GetComponentsInChildren<MeshRenderer>();
        if (meshRenderer == null)
            return;
        for (int i = 0; i < meshRenderer.Length; i++)
            meshRenderer[i].material.color = color;
    }

}
