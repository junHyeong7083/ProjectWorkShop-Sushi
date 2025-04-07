using UnityEngine;

public class Comment : MonoBehaviour
{
    [SerializeField]
    [TextArea(2, 15)]
    string comment = "";
}
