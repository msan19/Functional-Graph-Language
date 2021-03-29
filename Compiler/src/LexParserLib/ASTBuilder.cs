using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.BooleanOperationNodes;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes.RelationalOperationNodes;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using ASTLib.Nodes.TypeNodes;
using Hime.Redist;
using Main.Exceptions;

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
            var type = CreateFunctionTypeNode(himeFuncNode.Children[FUNCTIONTYPE_POS]);

            var parameterIdentifiers = GetParameterIdentifiers(himeFuncNode);
            string typeID = GetTypeID(himeFuncNode);
            string functionID = GetFunctionID(himeFuncNode);

            if (typeID != functionID)
                throw new FunctionIdentifierMatchException(functionID, typeID, null);

            if (IsConditional(himeDeclNode))
            {
                List<ConditionNode> conditions = VisitConditions(GetFunctionContent(himeDeclNode));
                return new FunctionNode(conditions, typeID, parameterIdentifiers, type,
                                        himeDeclNode.Position.Line, himeDeclNode.Position.Column);
            } else
            {
                var condition = GetInsertedConditionNode(himeDeclNode);
                return new FunctionNode(typeID, condition, parameterIdentifiers, type,
                                        himeDeclNode.Position.Line, himeDeclNode.Position.Column);
            }
        }

        private ConditionNode GetInsertedConditionNode(ASTNode himeDeclerationNode)
        {
            ASTNode expr = GetFunctionContent(himeDeclerationNode);
            return new ConditionNode(DispatchExpression(expr),
                                                        himeDeclerationNode.Position.Line,
                                                        himeDeclerationNode.Position.Column);
        }

        private List<ConditionNode> VisitConditions(ASTNode himeNode)
        {
            if (himeNode.Children.Count == 1)
            {
                return new List<ConditionNode> { CreateConditionNode(himeNode.Children[0]) };
            }
            else
            {
                List<ConditionNode> conditions = VisitConditions(himeNode.Children[0]);
                conditions.Add(CreateConditionNode(himeNode.Children[2]));
                return conditions;
            }
        }

        private bool IsConditional(ASTNode himeNode)
        {
            return himeNode.Children[2].Symbol.Name == "Conditions";
        }

        private ConditionNode CreateConditionNode(ASTNode himeNode)
        {
            ASTNode conditionExpr = himeNode.Children[1];
            ASTNode expr = himeNode.Children[3];
            return new ConditionNode(DispatchExpression(conditionExpr), DispatchExpression(expr),
                                                        himeNode.Position.Line,
                                                        himeNode.Position.Column);
        }

        private ASTNode GetFunctionContent(ASTNode himeDeclerationNode)
        {
            return himeDeclerationNode.Children[2];
        }

        private ASTNode GetHimeFuncNode(ASTNode himeDeclerationNode)
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
            if (IsParameterLessFunctionDeclaration(himeFuncNode))
                return new List<string>();
            else
                return VisitIdentifiers(himeFuncNode.Children[PARAMETER_IDs_POS]);
        }

        private bool IsParameterLessFunctionDeclaration(ASTNode himeFuncNode)
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
                "boolean" => new TypeNode(ASTLib.Nodes.TypeNodes.TypeEnum.Boolean,
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
                "Expression"        => VisitExpression(himeNode),
                "MaxTerm"           => VisitExpression(himeNode),
                "MinTerm"           => VisitExpression(himeNode),
                "LogicTerm"         => VisitLogicTerm(himeNode),
                "NumExpression"  => VisitExpression(himeNode),
                "Term"              => VisitExpression(himeNode),
                "Factor"            => VisitExpression(himeNode),
                "Exponent"          => VisitExponent(himeNode),
                _ => throw new Exception($"{ himeNode.Symbol.Name } is not yet implemented"),
            };
        }

        private ExpressionNode VisitExpression(ASTNode himeNode)
        {
            ExpressionNode leftOperant = DispatchExpression(himeNode.Children[0]);
            ExpressionNode rightOperant = DispatchExpression(himeNode.Children[2]);

            return himeNode.Children[1].Value switch
            {
                "eq" => new EqualExpression(leftOperant, rightOperant,
                                            himeNode.Position.Line,
                                            himeNode.Position.Column),
                "neq" => new NotEqualExpression(leftOperant, rightOperant,
                                                himeNode.Position.Line,
                                                himeNode.Position.Column),
                "or" => new OrExpression(leftOperant, rightOperant,
                                         himeNode.Position.Line,
                                         himeNode.Position.Column),
                "and" => new AndExpression(leftOperant, rightOperant,
                                           himeNode.Position.Line,
                                           himeNode.Position.Column),
                "+" => new AdditionExpression(leftOperant, rightOperant,
                                              himeNode.Position.Line, 
                                              himeNode.Position.Column),
                "-" => new SubtractionExpression(leftOperant, rightOperant,
                                                 himeNode.Position.Line, 
                                                 himeNode.Position.Column),
                "*" => new MultiplicationExpression(leftOperant, rightOperant,
                                                    himeNode.Position.Line,
                                                    himeNode.Position.Column),
                "/" => new DivisionExpression(leftOperant, rightOperant,
                                              himeNode.Position.Line,
                                              himeNode.Position.Column),
                "mod" => new ModuloExpression(leftOperant, rightOperant,
                                              himeNode.Position.Line,
                                              himeNode.Position.Column),
                "^" => new PowerExpression(leftOperant, rightOperant,
                                           himeNode.Position.Line, 
                                           himeNode.Position.Column),
                _ => throw new Exception($"{himeNode.Children[1].Value} has not been implemented as an operator")
            };
        }

        private ExpressionNode VisitLogicTerm(ASTNode himeNode)
        {
            if (himeNode.Children.Count == 2)
            {
                ExpressionNode expr = DispatchExpression(himeNode.Children[1]);
                return new NotExpression(expr, himeNode.Position.Line,
                                         himeNode.Position.Column);
            } else
            {
                ExpressionNode leftOperant = DispatchExpression(himeNode.Children[0]);
                ExpressionNode rightOperant = DispatchExpression(himeNode.Children[2]);
                ASTNode op = himeNode.Children[1];
                if (op.Children.Count != 0)
                    op = op.Children[0];

                return op.Value switch
                {
                    "geq" => new GreaterEqualExpression(leftOperant, rightOperant,
                                            himeNode.Position.Line,
                                            himeNode.Position.Column),
                    "greater" => new GreaterExpression(leftOperant, rightOperant,
                                            himeNode.Position.Line,
                                            himeNode.Position.Column),
                    "less" => new LessExpression(leftOperant, rightOperant,
                                            himeNode.Position.Line,
                                            himeNode.Position.Column),
                    "leq" => new LessEqualExpression(leftOperant, rightOperant,
                                            himeNode.Position.Line,
                                            himeNode.Position.Column),
                    _ => throw new Exception($"{op.Value} is not a valid relational operator")
                };
            }
                
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
                "true" => new BooleanLiteralExpression(true,
                                                                himeNode.Position.Line,
                                                                himeNode.Position.Column),
                "false" => new BooleanLiteralExpression(false,
                                                                himeNode.Position.Line,
                                                                himeNode.Position.Column),
                _ => throw new Exception($"{himeNode.Symbol.Name} has not been implemented as a literal"),
            };
        }

    }
    
}