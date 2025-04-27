using UnityEngine;

public class ShaderCountUpdater : MonoBehaviour
{
    public Material material;
    public int maxCount = 5;

    void Start()
    {
        Debug.Log(material.GetFloat("_Count"));
        UpdateShader();
    }

    public void AddCount()
    {
        DataManager.Instance.deathCount++;
        UpdateShader();
    }

    
    
    /// 업데이트 쉐이더? 아 씬 시작할때 
    private void UpdateShader()
    {
        if (material != null)
        {
            // 현재 죽음 카운트 값을 Material로 전달
            material.SetFloat("_Count", DataManager.Instance.deathCount);
            material.SetFloat("_MaxCount", maxCount);
        }
    }
}
