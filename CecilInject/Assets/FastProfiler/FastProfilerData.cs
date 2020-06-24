using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FastProfiler
{
    public enum ProfilerRecordType
    {
        All,
        Method,
        Class,
    }
    
    public class BaseProfilerRecord{
        public virtual ProfilerRecordType RecordType
        {
            get { return ProfilerRecordType.All; }
        }
        
        public virtual void Clear()
        {
            
        }
        
        public virtual void Start()
        {
            
        }

        public virtual void End()
        {
            
        }
    }
    public class FastProfilerMethodRecord : BaseProfilerRecord
    {
        public int startFrame;
        public int endFrame;
        public long startTime;
        public long endTime;
        public long gcStartAllocate;
        public long gcEndAllocate;

        public override ProfilerRecordType RecordType
        {
            get { return ProfilerRecordType.Method; }
        }

        public override void Start()
        {
            
        }

        public override void End()
        {
            
        }
    }

    public class FastProfilerClassRecord : BaseProfilerRecord
    {
        public override ProfilerRecordType RecordType
        {
            get { return ProfilerRecordType.Class; }
        }
        
        public override void Start()
        {
            
        }

        public override void End()
        {
            
        }
    }
}

