/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation;
namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
    public class TraceManager
    {
        private LogWriter logWriter;
        private TracerInstrumentationListener instrumentationListener;
        public LogWriter LogWriter
        { 
            get { return this.logWriter; }
        }
        public TracerInstrumentationListener InstrumentationListener
        {
            get { return this.instrumentationListener; }
        }
        public TraceManager(LogWriter logWriter):
            this(logWriter, null)
        {
        }
        public TraceManager(LogWriter logWriter, TracerInstrumentationListener instrumentationListener)
        {
            if (logWriter == null)
            {
                throw new ArgumentNullException("logWriter");
            }
            this.logWriter = logWriter;
            this.instrumentationListener = instrumentationListener;
        }
        public Tracer StartTrace(string operation)
        {
            return new Tracer(operation, this.logWriter, this.instrumentationListener);
        }
        public Tracer StartTrace(string operation, Guid activityId)
        {
            return new Tracer(operation, activityId, this.logWriter, this.instrumentationListener);
        }
    }
}
