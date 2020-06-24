using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FastProfiler
{
    public class FastProfiler
    {
        protected const int InvalidFrame = -1;
        public void AddListen()
        {
            
        }

        public void RemoveListen()
        {
            
        }
        
        protected Dictionary<string, FastProfilerClassRecord> goingClassRecord = new Dictionary<string, FastProfilerClassRecord>();
        protected Dictionary<string, FastProfilerMethodRecord> goingMethodRecord = new Dictionary<string, FastProfilerMethodRecord>();
        /// <summary>
        /// 开始记录函数
        /// </summary>
        public void StartRecordMethod(string methodName)
        {
            var record = GetMethodRecord(methodName);
            if (InvalidFrame != record.startFrame)
            {
                if (Time.frameCount == record.startFrame)
                {
                    Debug.LogError("递归调用");
                    return;                    
                }

                if (InvalidFrame == record.endFrame)
                {
                    Debug.LogError("有开始 没结束");
                }
            }
            
            record.Start();
        }

        /// <summary>
        /// 结束记录函数
        /// </summary>
        /// <param name="methodName"></param>
        public void EndRecordMethod(string methodName)
        {
            var record = GetMethodRecord(methodName);
            record.End();
            NotifyListen(record);
        }


        protected FastProfilerMethodRecord GetMethodRecord(string methodName)
        {
            FastProfilerMethodRecord ret = null;
            if (false == goingMethodRecord.ContainsKey(methodName))
            {
                ret = new FastProfilerMethodRecord();
                goingMethodRecord.Add(methodName, ret);
                return ret;
            }

            ret = goingMethodRecord[methodName];
            return ret;
        }

        protected void NotifyListen(BaseProfilerRecord record)
        {
            
        }
    }
}

