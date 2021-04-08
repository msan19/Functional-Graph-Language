using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Exceptions;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.BooleanOperationNodes;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes.RelationalOperationNodes;
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

        private AST VisitDeclarations(ASTNode himeNode)
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
                throw new FunctionIdentifierMatchException(functionID, typeID);

            if (IsConditional(himeDeclNode))
            {
                List<ConditionNode> conditions = new List<ConditionNode>();
                VisitConditions(GetFunctionContent(himeDeclNode), 
                                                   conditions, 
                                                   false);
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

        private void VisitConditions(ASTNode himeNode, List<ConditionNode> conditions, 
                                     bool hasFoundDefault)
        {
            if (himeNode.Children.Count == 1)
            {
                conditions.Add(CreateConditionNode(himeNode.Children[0]));
            }
            else
            {
                ConditionNode node = CreateConditionNode(himeNode.Children[1]);
                conditions.Add(node);
                if (hasFoundDefault && node.IsDefaultCase())
                    throw new Exception("More than one default case");
                VisitConditions(himeNode.Children[0], conditions, hasFoundDefault || node.IsDefaultCase());
            }
        }

        private bool IsConditional(ASTNode himeNode)
        {
            return himeNode.Children[2].Symbol.Name == "Conditions";
        }

        private ConditionNode CreateConditionNode(ASTNode himeNode)
        {
            
            ASTNode expr = himeNode.Children[3];
            ExpressionNode returnExpression = DispatchExpression(expr);

            if(himeNode.Children[1].Value == "_")
            {
                return new ConditionNode(returnExpression,
                                         himeNode.Position.Line, himeNode.Position.Column);
            } else
            {
                ExpressionNode conditionExpr = DispatchExpression(himeNode.Children[1]);
                return new ConditionNode(conditionExpr, returnExpression,
                                         himeNode.Position.Line, himeNode.Position.Column);
            }            
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
                _ => throw new UnimplementedASTException(himeNode.Children[0].Symbol.Name, "type"),
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
                _ => throw new UnimplementedASTException(himeNode.Symbol.Name, "symbol"),
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
                _ => throw new UnimplementedASTException(himeNode.Children[1].Value, "operator")
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
                    ">=" => new GreaterEqualExpression(leftOperant, rightOperant,
                                            himeNode.Position.Line,
                                            himeNode.Position.Column),
                    ">" => new GreaterExpression(leftOperant, rightOperant,
                                            himeNode.Position.Line,
                                            himeNode.Position.Column),
                    "<" => new LessExpression(leftOperant, rightOperant,
                                            himeNode.Position.Line,
                                            himeNode.Position.Column),
                    "<=" => new LessEqualExpression(leftOperant, rightOperant,
                                            himeNode.Position.Line,
                                            himeNode.Position.Column),
                    _ => throw new UnimplementedASTException(op.Value, "operator")
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
                List<ExpressionNode> expressions = new List<ExpressionNode>();

                if(himeNode.Children.Count != CONSTANT_FUNCTION_CALL)
                    VisitExpressions(himeNode.Children[EXPRESSIONS_POS], expressions);
                return new FunctionCallExpression(himeNode.Children[0].Value, expressions, 
                                                  himeNode.Position.Line, himeNode.Position.Column);
            }
        }

        private void VisitExpressions(ASTNode himeNode, List<ExpressionNode> expressions )
        {
            if (himeNode.Children.Count == 1)
                expressions.Add(DispatchExpression(himeNode.Children[0]));
            else
            {
                expressions.Add(DispatchExpression(himeNode.Children[0]));
                VisitExpressions(himeNode.Children[2], expressions);
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
                _ => throw new UnimplementedASTException(himeNode.Symbol.Name, "literal"),
            };
        }

    }
    
}