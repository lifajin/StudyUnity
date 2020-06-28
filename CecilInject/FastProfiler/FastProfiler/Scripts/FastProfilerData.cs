using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FastProfiler
{

    public class RecordDuration
    {
         public int startFrame;
         public int endFrame;
         public long startTime;
         public long endTime;      
         
         public void Start()
         {
            
         }

         public void End()
         {
            
         } 
    }
    
    public enum ProfilerRecordType
    {
        All,
        Method,
        Class,
    }
    
    public class BaseProfilerRecord
    {
        public RecordDuration Duration = new RecordDuration();
        public virtual ProfilerRecordType RecordType
        {
            get { return ProfilerRecordType.All; }
        }
        
        public virtual void Clear()
        {
            
        }
        
        public virtual void Start()
        {
            Duration.Start();
        }

        public virtual void End()
        {
            Duration.End();
        }
    }
    public class FastProfilerMethodRecord : BaseProfilerRecord
    {

        public long gcStartAllocate;
        public long gcEndAllocate;

        public override ProfilerRecordType RecordType
        {
            get { return ProfilerRecordType.Method; }
        }

        public override void Start()
        {
            base.Start();
        }

        public override void End()
        {
            base.End(); 
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
            base.Start();   
        }

        public override void End()
        {
            base.End();  
        }
    }
}

