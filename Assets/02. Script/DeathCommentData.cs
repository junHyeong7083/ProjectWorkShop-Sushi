using System.Collections.Generic;
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
