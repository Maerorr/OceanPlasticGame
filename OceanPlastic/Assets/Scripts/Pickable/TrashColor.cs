using UnityEngine;

public static class TrashColor
{
    public static Color MaterialToColor(MaterialType type)
    {
        switch (type)
        {
            case MaterialType.Glass:
                return new Color(0.18f, 0.773f, 0.341f);
            case MaterialType.Metal:
                return new Color(0.659f, 0.659f, 0.71f);
            case MaterialType.Plastic:
                return new Color(0.875f, 0.882f, 0.024f);
            case MaterialType.Rubber:
                return new Color(0.918f, 0.369f, 0.369f);
            default:
                return Color.white;
        }
        return Color.white;
    }
}
