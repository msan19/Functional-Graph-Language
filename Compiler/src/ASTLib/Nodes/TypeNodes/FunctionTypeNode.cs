using System;
using System.Collections.Generic;
using System.Text;

namespace ASTLib.Nodes.TypeNodes
{   
    public class FunctionTypeNode: TypeNode
    {
        public TypeNode ReturnType { get; }
        public List<TypeNode> ParameterTypes { get; }

        public FunctionTypeNode(TypeNode returnType, List<TypeNode> parameterTypes, int line, int letter) : base(TypeEnum.Function, line, letter)
        {
            ReturnType = returnType;
            ParameterTypes = parameterTypes;
        }
    }
}
