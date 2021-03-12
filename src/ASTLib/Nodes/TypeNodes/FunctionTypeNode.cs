using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Nodes.TypeNodes
{   
    public class FunctionTypeNode: TypeNode
    {
        public TypeNode ReturnType { get; private set; }
        public List<TypeNode> ParameterTypes { get; private set; }

        public FunctionTypeNode(TypeNode returnType, List<TypeNode> parameterTypes) : base(Type.Function)
        {
            ReturnType = returnType;
            ParameterTypes = parameterTypes;
        }
    }
}
