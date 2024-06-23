using UnityEngine;

public static class TrashColor
{
    public static Color MaterialToColor(MaterialType type)
    {
        switch (type)
        {
            case MaterialType.Glass:
                return new Color(0.106f, 0.72f, 0.129f);
            case MaterialType.Metal:
                return new Color(0.659f, 0.659f, 0.71f);
            case MaterialType.Plastic:
                return new Color(0.89f, 0.89f, 0f);
            case MaterialType.Rubber:
                return new Color(0.851f, 0.275f, 0.082f);
            default:
                return Color.white;
        }
        return Color.white;
    }
}
