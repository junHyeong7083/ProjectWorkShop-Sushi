using UnityEngine;

public static class JsonLoader
{
    public static T Load<T> (string resourcePath)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(resourcePath);
        if(jsonFile != null)
            return JsonUtility.FromJson<T>(jsonFile.text);

        return default;
    }
}

