using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

public class BlockIndexing : EditorWindow
{
    SerializedObject so; // 에디터에서 SerializedObject를 사용하여 필드를 관리  
    GameObject root;

    [MenuItem("BlockIndexing/BlockIndexing")]
    private static void ShowWindow()
    {
        // 커스텀 에디터 창 생성  
        var window = GetWindow<BlockIndexing>();
        window.titleContent = new GUIContent("BlockIndexing"); // 에디터 창 제목 설정  
        window.maxSize = new Vector2(300.0f, 700.0f); // 에디터 창의 최대 크기 설정  
        window.minSize = new Vector2(300.0f, 700.0f); // 에디터 창의 최소 크기 설정  
        window.Show(); // 에디터 창 표시  
    }

    private void OnEnable()
    {
        // 현재 스크립트를 SerializedObject로 감싸서 에디터에서 관리 가능하게 설정  
        ScriptableObject target = this;
        so = new SerializedObject(target);
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Root GameObject");
        /// objectField( "필드라벨", obj, 선택가능한타입, true : 씬안의
        root = (GameObject)EditorGUILayout.ObjectField("Root", root, typeof(GameObject), true);

        // 섹션 생성 버튼  
        if (GUILayout.Button("BlockIndex"))
        {
            if (root == null) return;
            SectionIndexing(root.transform);
        }

    }

    void SectionIndexing(Transform root)
    {
        int index = 0;
        AssignSectionIndex(root,ref index);


    }

    void AssignSectionIndex(Transform root, ref int index)
    {
        foreach (Transform child in root)
        {
            var block = child.GetComponent<Block>();
            if (block != null)
            {
                block.Index = index++;
            }
        }
    }

}
