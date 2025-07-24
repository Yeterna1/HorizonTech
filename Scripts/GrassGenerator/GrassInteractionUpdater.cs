using UnityEngine;
using System.Collections.Generic;

public class GrassInteractionUpdater : MonoBehaviour
{
    [Header("可留空：若为空则自动查找 Tag=Player")]
    [Tooltip("玩家角色的 Transform；若不拖，脚本会在 Awake 时用 GameObject.FindWithTag(\"Player\") 查找")]
    public Transform interactorTransform;

    [Header("草地组根节点 (会搜它的所有子 Renderer)")]
    [Tooltip("把整个草地 Parent 拖到这里，脚本会自动搜集它所有子物体的 Renderer")]
    public Transform grassGroupRoot;

    [Header("交互设置")]
    [Tooltip("草地互动半径")]
    public float interactionRadius   = 1.0f;
    [Tooltip("草地互动强度 (推开距离)")]
    public float interactionStrength = 5.0f;

    // 内部缓存
    private List<Renderer> _renderers = new List<Renderer>();
    private MaterialPropertyBlock _block;
    private int _posID, _radiusID, _strengthID;

    void Awake()
    {
        // 1. 找玩家
        if (interactorTransform == null)
        {
            var go = GameObject.FindWithTag("Player");
            if (go != null) interactorTransform = go.transform;
            else
            {
                Debug.LogError("[GrassInteractionUpdater] 找不到 Tag=\"Player\" 的对象，也未手动拖入 interactorTransform");
                enabled = false;
                return;
            }
        }

        // 2. 找草地 Renderers
        if (grassGroupRoot == null)
        {
            Debug.LogError("[GrassInteractionUpdater] 未指定 grassGroupRoot，请在 Inspector 拖入草地组的根节点");
            enabled = false;
            return;
        }
        // 收集所有子 Renderer
        _renderers.AddRange(grassGroupRoot.GetComponentsInChildren<Renderer>());
        if (_renderers.Count == 0)
        {
            Debug.LogError("[GrassInteractionUpdater] 在 grassGroupRoot 下未找到任何 Renderer");
            enabled = false;
            return;
        }

        // 3. 准备 PropertyBlock 和 Shader IDs
        _block     = new MaterialPropertyBlock();
        _posID     = Shader.PropertyToID("_InteractorPos");
        _radiusID  = Shader.PropertyToID("_InteractionRadius");
        _strengthID= Shader.PropertyToID("_InteractionStrength");
    }

    void Update()
    {
        Vector4 pos = interactorTransform.position;
        float   rad = interactionRadius;
        float   str = interactionStrength;

        // 对每个草地 Renderer 应用 PropertyBlock
        foreach (var rend in _renderers)
        {
            rend.GetPropertyBlock(_block);
            _block.SetVector(_posID, pos);
            _block.SetFloat(_radiusID, rad);
            _block.SetFloat(_strengthID, str);
            rend.SetPropertyBlock(_block);
        }
    }

    void OnDrawGizmos()
    {
        if (interactorTransform != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(interactorTransform.position, interactionRadius);
        }
    }
}
