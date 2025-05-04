using UnityEngine;

namespace _Project.Scripts.Main.Extension
{
    public static class TransformExtension
    {
        public static void DestroyAllChildren(this Transform tr)
        {
            for (int i = tr.childCount - 1; i >= 0; i--)
            {
                Object.Destroy(tr.GetChild(i).gameObject);
            }
        }
    }
}