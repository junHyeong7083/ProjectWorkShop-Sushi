using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class DeathComment
{
    public string text;
}

[System.Serializable]
public class DeathCommentData
{
    public List<DeathComment> comments;
}
