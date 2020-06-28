using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FastProfiler
{
    public class FastProfiler
    {
        protected static int InvalidFrame = -1;
        public static void AddListen()
        {
            
        }

        public static  void RemoveListen()
        {

        }
        
        protected static  Dictionary<string, FastProfilerClassRecord> goingClassRecord = new Dictionary<string, FastProfilerClassRecord>();
        protected static  Dictionary<string, FastProfilerMethodRecord> goingMethodRecord = new Dictionary<string, FastProfilerMethodRecord>();
        /// <summary>
        /// 开始记录函数
        /// </summary>
        public static void StartRecordMethod(string methodName)
        {
            Debug.LogError("StartRecordMethod::"+methodName);
            var record = GetMethodRecord(methodName);
            if (InvalidFrame != record.Duration.startFrame)
            {
                if (Time.frameCount == record.Duration.startFrame)
                {
                    Debug.LogError("递归调用");
                    return;                    
                }

                if (InvalidFrame == record.Duration.endFrame)
                {
                    throw new SystemException("有开始 没结束");
                }
            }
            
            record.Start();
        }

        /// <summary>
        /// 结束记录函数
        /// </summary>
        /// <param name="methodName"></param>
        public static void EndRecordMethod(string methodName)
        {
            Debug.LogError("EndRecordMethod::"+methodName);
            var record = GetMethodRecord(methodName);
            record.End();
            NotifyListen(record);
        }


        protected  static FastProfilerMethodRecord GetMethodRecord(string methodName)
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

        protected  static void NotifyListen(BaseProfilerRecord record)
        {
            
        }

        public static void Empty()
        {
            Debug.LogError("递归调用");
        }
        
        public static void ClassInstanceCreated()
        {
            Debug.LogError("ClassInstanceCreated");
        }
        
        public static void ClassInstanceWithdraw()
        {
            Debug.LogError("ClassInstanceWithdraw");
        }
    }
}

