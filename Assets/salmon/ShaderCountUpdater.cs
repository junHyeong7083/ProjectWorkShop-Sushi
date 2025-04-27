using UnityEngine;

public class ShaderCountUpdater : MonoBehaviour
{
    public Material material;
    public int maxCount = 30;

    void Start()
    {
        UpdateShader();
    }

    public void AddCount()
    {
        DataManager.Instance.deathCount++;
        UpdateShader();
    }

    public void SubCount()
    {
        DataManager.Instance.deathCount--;
        UpdateShader();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
            AddCount();
        if(Input.GetKeyDown(KeyCode.F2))
            SubCount();
    }



    /// 업데이트 쉐이더? 아 씬 시작할때 
    public void UpdateShader()
    {
        if (material != null)
        {
            // 현재 죽음 카운트 값을 Material로 전달
            material.SetFloat("_Count", DataManager.Instance.deathCount);
            material.SetFloat("_MaxCount", maxCount);
        }
    }
}
