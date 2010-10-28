using System;
using System.CodeDom;
using System.Security;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.MathX.NugenCCalc
{
    public class NugenCCalcSerializer : CodeDomSerializer
    {
        public override object Serialize(IDesignerSerializationManager manager, object value)
        {
            CodeDomSerializer baseSerializer =
                (CodeDomSerializer)manager.GetSerializer(
                typeof(Component),
                typeof(CodeDomSerializer));

            object codeObject = baseSerializer.Serialize(manager, value);

            if (codeObject is CodeStatementCollection)
            {
                CodeStatementCollection statements =
                    (CodeStatementCollection)codeObject;

                CodeExpression targetObject =
                    base.SerializeToReferenceExpression(manager, value);
                if (targetObject != null)
                {
                    statements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(targetObject, "Owner"),
                        new CodeThisReferenceExpression()));
                }
            }
            return codeObject;
        }
        public override object Deserialize(IDesignerSerializationManager manager, object codeObject)
        {
            CodeDomSerializer baseClassSerializer = (CodeDomSerializer)manager.
                GetSerializer(typeof(NugenCCalcBase).BaseType, typeof(CodeDomSerializer));
            return baseClassSerializer.Deserialize(manager, codeObject);
        }
    }
}
