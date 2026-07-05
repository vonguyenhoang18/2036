using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class CircleVisionController : MonoBehaviour
{
    public Image targetImage;
    [SerializeField] private Material sourceMaterial; // ← assign the original in Inspector

    [Header("Vision Control")]
    [Range(0f, 2f)] public float innerRadius = 0.2f;
    [Range(0f, 2f)] public float outerRadius = 0.4f;
    public Vector2 center = new Vector2(0.5f, 0.5f);

    private Material runtimeMat;

    void OnEnable()
    {
        Init();
        Apply();
    }

    void OnValidate()
    {
        Init();
        Apply();
    }

    void OnDisable()
    {
        Cleanup();
    }

    void OnDestroy()
    {
        Cleanup();
    }

    void Init()
    {
        if (targetImage == null || sourceMaterial == null) return;

        // Only create if we don't have one, or it was destroyed
        if (runtimeMat == null)
        {
            runtimeMat = Instantiate(sourceMaterial); // always from the ORIGINAL
            runtimeMat.name = sourceMaterial.name + "_Runtime";
        }

        targetImage.material = runtimeMat;
    }

    void Cleanup()
    {
        if (runtimeMat != null)
        {
            DestroyImmediate(runtimeMat);
            runtimeMat = null;
        }

        // Restore original material
        if (targetImage != null && sourceMaterial != null)
            targetImage.material = sourceMaterial;
    }

    void Apply()
    {
        if (runtimeMat == null) return;
        float aspect = (float)Screen.width / Screen.height;
        runtimeMat.SetFloat("_InnerRadius", innerRadius);
        runtimeMat.SetFloat("_OuterRadius", outerRadius);
        runtimeMat.SetFloat("_Aspect", aspect);
        runtimeMat.SetVector("_Center", center);

        if (!Application.isPlaying)
            targetImage.SetMaterialDirty();
    }

    public void SetRadius(float radius)
    {
        if (runtimeMat == null) return;
        innerRadius = radius;
        runtimeMat.SetFloat("_InnerRadius", innerRadius);
        targetImage.SetMaterialDirty();
    }
}