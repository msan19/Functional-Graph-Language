using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.TypeNodes;
using Hime.Redist;

namespace LexParserLib
{
    public class ASTBuilder
    {
        private const int PARAMETER_IDs_POS = 5, FUNCTIONTYPE_POS = 2, 
                          RETURNTYPE_POS = 4, CONSTANT_FUNCTION_CALL = 4,
                          EXPRESSIONS_POS = 4;
        
        public AST GetAST(ASTNode root)
        {
            return VisitDeclarations(root.Children[0]);
        }

        public AST VisitDeclarations(ASTNode himeNode)
        {
            if (himeNode.Children.Count == 1)
            {
                List<FunctionNode> functionNodes = new List<FunctionNode>();
                List<ExportNode> exportNodes = new List<ExportNode>();
                AST ast = new AST(functionNodes, exportNodes, himeNode.Position.Line, himeNode.Position.Column);
                InsertDeclaration(ast, himeNode.Children[0]);
                return ast;
            }
            else
            {
                AST ast = VisitDeclarations(himeNode.Children[0]);
                InsertDeclaration(ast, himeNode.Children[1]);
                return ast;
            }
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
        
        private FunctionNode CreateFunctionNode(ASTNode himeNode)
        {
            ASTNode himeFuncNode = himeNode.Children[0];
            ASTNode himeExpressionNode = himeNode.Children[2];
            List<string> parameterIdentifiers = VisitIdentifiers(himeFuncNode.Children[PARAMETER_IDs_POS]);
            FunctionTypeNode type = CreateFunctionTypeNode(himeFuncNode.Children[FUNCTIONTYPE_POS]);
            return new FunctionNode(new ConditionNode(DispatchExpression(himeExpressionNode),
                                                      himeNode.Position.Line, himeNode.Position.Column),
                                    parameterIdentifiers, type, himeNode.Position.Line, himeNode.Position.Column);
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
                "integer" => new TypeNode(ASTLib.Nodes.TypeNodes.Type.Integer,
                                                       himeNode.Position.Line, himeNode.Position.Column),
                "real" => new TypeNode(ASTLib.Nodes.TypeNodes.Type.Real,
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
            if (himeNode.Children[1].Value == "^")
            {
                ExpressionNode leftOperant = DispatchExpression(himeNode.Children[0]);
                ExpressionNode rightOperant = DispatchExpression(himeNode.Children[2]);
                return new PowerExpression(leftOperant, rightOperant,
                                              himeNode.Position.Line, himeNode.Position.Column);
            }
            else return new AbsoluteValueExpression(DispatchExpression(himeNode.Children[1]),
                                                  himeNode.Position.Line, himeNode.Position.Column);
        }

        private ExpressionNode VisitExponent(ASTNode himeNode)
        {
            if (himeNode.Children[0].Value == "(") 
                return DispatchExpression(himeNode.Children[1]);
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