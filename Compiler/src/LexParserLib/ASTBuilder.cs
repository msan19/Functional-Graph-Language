using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using ASTLib.Nodes.TypeNodes;
using Hime.Redist;

namespace LexParserLib
{
    public class ASTBuilder
    {
        private const int PARAMETER_IDs_POS = 5, FUNCTIONTYPE_POS = 2, 
                          RETURNTYPE_POS = 4, CONSTANT_FUNCTION_CALL = 3,
                          EXPRESSIONS_POS = 2, CONSTANT_FUNCTION_DECLARATION = 6;
        
        public AST GetAST(ASTNode root)
        {
            return VisitDeclarations(root.Children[0]);
        }

        public AST VisitDeclarations(ASTNode himeNode)
        {
            AST ast;
            int declarationIndex;
            if (himeNode.Children.Count == 1)
            {
                ast = GetNewAst(himeNode);
                declarationIndex = 0;
            }
            else
            {
                ast = VisitDeclarations(himeNode.Children[0]);
                declarationIndex = 1;
            }
            InsertDeclaration(ast, himeNode.Children[declarationIndex]);
            return ast;
        }

        private AST GetNewAst(ASTNode himeNode)
        {
            List<FunctionNode> functionNodes = new List<FunctionNode>();
            List<ExportNode> exportNodes = new List<ExportNode>();
            return new AST(functionNodes, exportNodes, himeNode.Position.Line, himeNode.Position.Column);
        }

        private void InsertDeclaration(AST ast, ASTNode himeNode)
        {
            if (himeNode.Children[0].Value == "export")
            {
                ExportNode exportNode = CreateExportNode(himeNode);
                ast.Exports.Add(exportNode);
            }
            else
            {
                FunctionNode functionNode = CreateFunctionNode(himeNode);
                ast.Functions.Add(functionNode);
            }
        }
        
        private ExportNode CreateExportNode(ASTNode himeNode)
        {
            ExpressionNode expressionNode = DispatchExpression(himeNode.Children[1]);
            return new ExportNode(expressionNode, himeNode.Position.Line, himeNode.Position.Column);
        }
        
        private FunctionNode CreateFunctionNode(ASTNode himeDeclNode)
        {
            var himeFuncNode = GetHimeFuncNode(himeDeclNode);
            var himeExprNode = GetHimeExprNode(himeDeclNode);

            var parameterIdentifiers = GetParameterIdentifiers(himeFuncNode);
            var condition = GetConditionNode(himeDeclNode, himeExprNode);
            string typeID = GetTypeID(himeFuncNode);
            string functionID = GetFunctionID(himeFuncNode);
            
            var type = CreateFunctionTypeNode(himeFuncNode.Children[FUNCTIONTYPE_POS]);

            if (typeID != functionID) 
                throw new Exception($"{typeID} and {functionID} should be equivalent");

            return new FunctionNode(typeID, condition, parameterIdentifiers, type,
                                    himeDeclNode.Position.Line, himeDeclNode.Position.Column);
        }

        private ConditionNode GetConditionNode(ASTNode himeDeclerationNode, ASTNode himeExprNode)
        {
            return new ConditionNode(DispatchExpression(himeExprNode),
                                                        himeDeclerationNode.Position.Line,
                                                        himeDeclerationNode.Position.Column);
        }

        private static ASTNode GetHimeExprNode(ASTNode himeDeclerationNode)
        {
            return himeDeclerationNode.Children[2];
        }

        private static ASTNode GetHimeFuncNode(ASTNode himeDeclerationNode)
        {
            return himeDeclerationNode.Children[0];
        }

        private string GetFunctionID(ASTNode himeFuncNode)
        {
            return himeFuncNode.Children[3].Value;
        }

        private string GetTypeID(ASTNode himeFuncNode)
        {
            return himeFuncNode.Children[0].Value;
        }

        private List<string> GetParameterIdentifiers(ASTNode himeFuncNode)
        {
            if (IsFunctionDeclaration(himeFuncNode))
                return new List<string>();
            else
                return VisitIdentifiers(himeFuncNode.Children[PARAMETER_IDs_POS]);
        }

        private bool IsFunctionDeclaration(ASTNode himeFuncNode)
        {
            return himeFuncNode.Children.Count == CONSTANT_FUNCTION_DECLARATION;
        }

        public FunctionTypeNode CreateFunctionTypeNode(ASTNode himeNode)
        {
            TypeNode returnType = CreateTypeNode(himeNode.Children[RETURNTYPE_POS]);
            List<TypeNode> parameterTypes = VisitTypes(himeNode.Children[1]);
            return new FunctionTypeNode(returnType, parameterTypes, himeNode.Position.Line, himeNode.Position.Column);
        }

        public TypeNode CreateTypeNode(ASTNode himeNode)
        {
            return himeNode.Children[0].Symbol.Name switch
            {
                "integer" => new TypeNode(ASTLib.Nodes.TypeNodes.TypeEnum.Integer,
                                                       himeNode.Position.Line, himeNode.Position.Column),
                "real" => new TypeNode(ASTLib.Nodes.TypeNodes.TypeEnum.Real,
                                       himeNode.Position.Line, himeNode.Position.Column),
                "FuncTypeDecl" => CreateFunctionTypeNode(himeNode.Children[0]),
                _ => throw new Exception($"'{himeNode.Children[0].Symbol.Name}' is not an accepted type"),
            };
        }

        public List<TypeNode> VisitTypes(ASTNode himeNode)
        {
            if (himeNode.Children.Count == 1)
            {
                return new List<TypeNode> {CreateTypeNode(himeNode.Children[0])};
            } else
            {
                List<TypeNode> types = VisitTypes(himeNode.Children[0]);
                types.Add(CreateTypeNode(himeNode.Children[2]));
                return types;
            }
        }

        public List<string> VisitIdentifiers(ASTNode himeNode)
        {
            if (himeNode.Children.Count == 1)
                return new List<string> { himeNode.Children[0].Value };
            else
            {
                List<string> identifiers = VisitIdentifiers(himeNode.Children[0]);
                identifiers.Add(himeNode.Children[2].Value);
                return identifiers;
            }
        }
        
        private ExpressionNode DispatchExpression(ASTNode himeNode)
        {
            switch(himeNode.Symbol.Name)
            {
                case "id": return new IdentifierExpression(himeNode.Value,
                                                    himeNode.Position.Line,
                                                    himeNode.Position.Column);
                case "Literal": return VisitLiteral(himeNode.Children[0]);
                default: break;
            }

            if(himeNode.Children.Count == 1) return DispatchExpression(himeNode.Children[0]);

            return himeNode.Symbol.Name switch
            {
                "Expression" => VisitExpression(himeNode),
                "Term" => VisitTerm(himeNode),
                "Factor" => VisitFactor(himeNode),
                "Exponent" => VisitExponent(himeNode),
                _ => throw new Exception($"{ himeNode.Symbol.Name } is not yet implemented"),
            };
        }

        private ExpressionNode VisitExpression(ASTNode himeNode)
        {
            ExpressionNode leftOperant = DispatchExpression(himeNode.Children[0]);
            ExpressionNode rightOperant = DispatchExpression(himeNode.Children[2]);

            return himeNode.Children[1].Value switch
            {
                "+" => new AdditionExpression(leftOperant, rightOperant,
                                              himeNode.Position.Line, himeNode.Position.Column),
                "-" => new SubtractionExpression(leftOperant, rightOperant,
                                                 himeNode.Position.Line, himeNode.Position.Column),
                _ => throw new Exception($"{himeNode.Children[1].Value} has not been implemented as an operator")
            };
        }

        private ExpressionNode VisitTerm(ASTNode himeNode)
        {
            ExpressionNode leftOperant = DispatchExpression(himeNode.Children[0]);
            ExpressionNode rightOperant = DispatchExpression(himeNode.Children[2]);
            
            return himeNode.Children[1].Value switch
            {
                "*" => new MultiplicationExpression(leftOperant, rightOperant,
                                                    himeNode.Position.Line,
                                                    himeNode.Position.Column),
                "/" => new DivisionExpression(leftOperant, rightOperant,
                                              himeNode.Position.Line,
                                              himeNode.Position.Column),
                "mod" => new ModuloExpression(leftOperant, rightOperant,
                                             himeNode.Position.Line,
                                             himeNode.Position.Column),
                _ => throw new Exception($"{himeNode.Children[1].Value} has not been implemented as an operator")
            };
        }

        private ExpressionNode VisitFactor(ASTNode himeNode)
        {
            ExpressionNode leftOperant = DispatchExpression(himeNode.Children[0]);
            ExpressionNode rightOperant = DispatchExpression(himeNode.Children[2]);
            return new PowerExpression(leftOperant, rightOperant,
                                          himeNode.Position.Line, himeNode.Position.Column);
        }

        private ExpressionNode VisitExponent(ASTNode himeNode)
        {
            if (himeNode.Children[0].Value == "(") 
                return DispatchExpression(himeNode.Children[1]);
            else if(himeNode.Children[0].Value == "|") 
                return new AbsoluteValueExpression(DispatchExpression(himeNode.Children[1]),
                                                  himeNode.Position.Line, himeNode.Position.Column);
            else
            {
                List<ExpressionNode> expressions = himeNode.Children.Count == CONSTANT_FUNCTION_CALL ?
                                                   new List<ExpressionNode>() :
                                                   VisitExpressions(himeNode.Children[EXPRESSIONS_POS]);
                return new FunctionCallExpression(himeNode.Children[0].Value, expressions, 
                                                  himeNode.Position.Line, himeNode.Position.Column);
            }
        }

        private List<ExpressionNode> VisitExpressions(ASTNode himeNode)
        {
            if (himeNode.Children.Count == 1) 
                return new List<ExpressionNode> { DispatchExpression(himeNode.Children[0])};
            else
            {
                List<ExpressionNode> expressions = VisitExpressions(himeNode.Children[2]);
                expressions.Add(DispatchExpression(himeNode.Children[0]));
                return expressions;
            }
        }

        private ExpressionNode VisitLiteral(ASTNode himeNode)
        {
            return himeNode.Symbol.Name switch
            {
                "realNumber" => new RealLiteralExpression(himeNode.Value,
                                                          himeNode.Position.Line,
                                                          himeNode.Position.Column),
                "integerNumber" => new IntegerLiteralExpression(himeNode.Value,
                                                                himeNode.Position.Line,
                                                                himeNode.Position.Column),
                _ => throw new Exception($"{himeNode.Symbol.Name} has not been implemented as a literal"),
            };
        }

    }
    
}