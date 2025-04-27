using UnityEngine;

public class ShaderCountUpdater : MonoBehaviour
{
    public Material material;    
    public int count = 0;      
    public int maxCount = 5;    

    void Start()
    {
        UpdateShader();
    }

    public void AddCount()
    {
        count++;
        UpdateShader();
    }

   private void UpdateShader()
{
    if (material != null)
    {
        // 현재 죽음 카운트 값을 Material로 전달
        material.SetFloat("_Count", count);
        material.SetFloat("_MaxCount", maxCount);
    }
}
}
