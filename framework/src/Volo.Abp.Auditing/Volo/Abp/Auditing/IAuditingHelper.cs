using System;
using System.Collections.Generic;
using System.Reflection;

namespace Volo.Abp.Auditing
{
    //TODO: Move ShouldSaveAudit and rename to IAuditingFactory
    public interface IAuditingHelper
    {
        bool ShouldSaveAudit(MethodInfo methodInfo, bool defaultValue = false);

        AuditLogInfo CreateAuditLogInfo();

        AuditLogActionInfo CreateAuditLogAction(Type type, MethodInfo method, object[] arguments);

        AuditLogActionInfo CreateAuditLogAction(Type type, MethodInfo method, IDictionary<string, object> arguments);
    }
}