using UnityEngine;

public class CheatKey : MonoBehaviour
{
    public GameObject Player;
    public GameObject Goal;

    void Update()
    {
        // µµÂøÁöÁ¡
        if(Input.GetKeyDown(KeyCode.F3))
            Player.transform.position = Goal.transform.position;

    }

}
