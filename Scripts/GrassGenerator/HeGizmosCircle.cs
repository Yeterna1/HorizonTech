
using UnityEngine;
using System; // System 命名空间在这里其实不需要了

// [RequireComponent(typeof(Transform))] // 可以确保总有一个 Transform
public class HeGizmosCircle : MonoBehaviour
{
    // 这个 Transform 决定了圆圈绘制的位置和方向
    // 通常我们让它指向自身 GameObject 的 Transform
    [Tooltip("圆圈绘制所依据的变换，通常是自身 Transform")]
    public Transform m_Transform;

    [Tooltip("圆环的半径")]
    public float m_Radius = 1;

    [Tooltip("值越低圆环越平滑 (段数越多)")]
    public float m_Theta = 0.1f; // 弧度步长

    [Tooltip("线框颜色")]
    public Color m_Color = Color.cyan; // 改成 Cyan 和 Inspector 匹配

    
    void OnDrawGizmos()
    {
        // 确保有 Transform 可以绘制
        if (m_Transform == null)
        {
             // 如果未在 Inspector 中赋值，尝试获取自身的 Transform
             m_Transform = this.transform;
             // 如果还是没有（理论上不可能），则不绘制
             if(m_Transform == null) return;
        }

        // 防止 Theta 过小导致性能问题或死循环
        if (m_Theta < 0.001f) m_Theta = 0.001f;

        // 保存当前的 Gizmos 状态 (矩阵和颜色)
        Matrix4x4 oldMatrix = Gizmos.matrix;
        Color oldColor = Gizmos.color;

        // 设置 Gizmos 的矩阵为目标 Transform 的本地到世界矩阵
        // 这样我们就可以在 (0,0,0) 为中心绘制圆，它会自动出现在世界正确的位置和方向
        Gizmos.matrix = m_Transform.localToWorldMatrix;
        Gizmos.color = m_Color;

        // 绘制圆环
        Vector3 beginPoint = Vector3.zero; // 起始点设为本地坐标原点
        Vector3 firstPoint = new Vector3(m_Radius, 0, 0); // 第一个点从 X 轴正方向开始

        beginPoint = firstPoint; // 初始化起始点

        // 步长计算总段数
        int segments = Mathf.CeilToInt ( (2 * Mathf.PI) / m_Theta );
        m_Theta = (2 * Mathf.PI) / segments; // 重新计算精确的步长，确保闭合

        for (int i = 1; i <= segments; i++) // 从 1 开始循环
        {
            float angle = i * m_Theta;
            float x = m_Radius * Mathf.Cos(angle);
            float z = m_Radius * Mathf.Sin(angle);
            Vector3 endPoint = new Vector3(x, 0, z); // 在本地 XZ 平面计算点

            Gizmos.DrawLine(beginPoint, endPoint); // 绘制线段

            beginPoint = endPoint; // 更新下一个线段的起始点
        }
        // 绘制从最后一个点回到第一个点的线段 (理论上由于精度可能不需要，但加上无妨)
        // Gizmos.DrawLine(beginPoint, firstPoint);

        // 恢复 Gizmos 的默认状态
        Gizmos.color = oldColor;
        Gizmos.matrix = oldMatrix;
    }

    // (可选) 如果 GrassGroup_Inspector 需要启用/禁用这个 Gizmo，可以添加一个简单的控制方法
    // 但当前 Inspector 实现是直接移动这个 GameObject，所以不需要 SetEnable
    // public void SetGizmoEnabled(bool enabled)
    // {
    //     this.enabled = enabled; // MonoBehaviour 的 enabled 属性可以控制 OnDrawGizmos 是否调用
    // }
}