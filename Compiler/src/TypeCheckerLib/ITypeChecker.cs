using ASTLib;

namespace TypeCheckerLib
{
    public interface ITypeChecker
    {
        void CheckTypes(AST root);
        void Dispatch();
    }
}