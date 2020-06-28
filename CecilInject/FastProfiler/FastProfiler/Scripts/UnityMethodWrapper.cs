using UnityEngine;

namespace FastProfiler
{
    public class UnityMethodWrapper
    {
        public static void UnityMethod_GameObject_SetActive(GameObject obj, bool ret)
        {
            Debug.LogError("this is unity method wrapper set active");
            obj.SetActive(ret);
        }
    }
}