using UnityEngine;

public class Comment : MonoBehaviour
{
    [SerializeField]
    [TextArea(2, 25)]
    string comment = "";
}
