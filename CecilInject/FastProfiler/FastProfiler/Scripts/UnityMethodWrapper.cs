using UnityEngine;

namespace FastProfiler
{
    public class UnityMethodWrapper
    {
        public static void UnityMethod_GameObject_SetActive(GameObject obj, bool ret)
        {
            Debug.LogError("Unity SetActive 封装");
            obj.SetActive(ret);
        }
    }
}