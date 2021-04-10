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
                          EXPRESSIONS_POS = 2, CONSTANT_FUNCTION_DECLARATION = 6,
                          SET_WITH_PREDICATE = 7, DOUBLE_BOUNDS = 7,
                          CONDITION_BOTH_ELEMENTS_AND_PREDICATE = 6;
        
        public AST GetAST(ASTNode root)
        {
            AST ast = VisitDeclarations(root.Children[0]);
            return ast;
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
            return new AST(functionNodes, exportNodes, 0, 0);
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
            TextPosition position = himeNode.Children[0].Position;
            return new ExportNode(expressionNode, position.Line, position.Column);
        }
        
        private FunctionNode CreateFunctionNode(ASTNode himeDeclNode)
        {
            ASTNode himeFuncNode = GetHimeFuncNode(himeDeclNode);
            FunctionTypeNode type = CreateFunctionTypeNode(himeFuncNode.Children[FUNCTIONTYPE_POS]);

            List<string> parameterIdentifiers = GetParameterIdentifiers(himeFuncNode);
            string typeID = GetTypeID(himeFuncNode);
            string functionID = GetFunctionID(himeFuncNode);

            if (typeID != functionID)
                throw new FunctionIdentifierMatchException(functionID, typeID);

            TextPosition position = himeFuncNode.Children[0].Position;
            if (IsConditional(himeDeclNode))
            {
                List<ConditionNode> conditions = new List<ConditionNode>();
                VisitConditions(GetFunctionContent(himeDeclNode), 
                                                   conditions, 
                                                   false);
                return new FunctionNode(conditions, typeID, parameterIdentifiers, type,
                                        position.Line, position.Column);
            } else
            {
                ConditionNode condition = GetInsertedConditionNode(himeDeclNode);
                return new FunctionNode(typeID, condition, parameterIdentifiers, type,
                                        position.Line, position.Column);
            }
        }

        private ConditionNode GetInsertedConditionNode(ASTNode himeDeclerationNode)
        {
            ASTNode expr = GetFunctionContent(himeDeclerationNode);
            TextPosition position = himeDeclerationNode.Children[1].Position;
            return new ConditionNode(DispatchExpression(expr),
                                                        position.Line,
                                                        position.Column);
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
                if (hasFoundDefault && node.IsDefaultCase)
                    throw new Exception("More than one default case");
                VisitConditions(himeNode.Children[0], conditions, hasFoundDefault || node.IsDefaultCase);
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

            TextPosition position = himeNode.Children[0].Position;
            if (himeNode.Children[1].Value == "_")
            {
                return new ConditionNode(returnExpression,
                                         position.Line, position.Column);
            } else
            {
                bool both = himeNode.Children.Count == CONDITION_BOTH_ELEMENTS_AND_PREDICATE;
                string symbol = himeNode.Children[1].Symbol.Name;

                ExpressionNode conditionExpr = both ? DispatchExpression(himeNode.Children[3]) : null;
                List<ElementNode> elements = null;
                if (symbol == "Elements")
                    elements = VisitElements(himeNode.Children[1]);
                else
                    conditionExpr = DispatchExpression(himeNode.Children[1]);
                return new ConditionNode(elements, conditionExpr, returnExpression,
                                         position.Line, position.Column);
            }            
        }

        public List<ElementNode> VisitElements(ASTNode himeNode)
        {
            if (himeNode.Children.Count == 1)
            {
                return new List<ElementNode> { GetElementNode(himeNode.Children[0]) };
            }
            else
            {
                List<ElementNode> elements = VisitElements(himeNode.Children[0]);
                elements.Add(GetElementNode(himeNode.Children[2]));
                return elements;
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
            TypeNode returnType;
            List<TypeNode> parameterTypes;
            if (himeNode.Children.Count == 5)
            {
                returnType = CreateTypeNode(himeNode.Children[RETURNTYPE_POS]);
                parameterTypes = VisitTypes(himeNode.Children[1]);
            }
            else
            {
                returnType = CreateTypeNode(himeNode.Children[RETURNTYPE_POS - 1]);
                parameterTypes = new List<TypeNode>();
            }

            TextPosition position = himeNode.Children[0].Position;
            return new FunctionTypeNode(returnType, parameterTypes, position.Line, position.Column);
        }

        public TypeNode CreateTypeNode(ASTNode himeNode)
        {

            TextPosition position = himeNode.Children[0].Position;
            return himeNode.Children[0].Symbol.Name switch
            {
                "integer"       => new TypeNode(TypeEnum.Integer, position.Line, position.Column),
                "real"          => new TypeNode(TypeEnum.Real,    position.Line, position.Column),
                "boolean"       => new TypeNode(TypeEnum.Boolean, position.Line, position.Column),
                "set"           => new TypeNode(TypeEnum.Set,     position.Line, position.Column),
                "element"       => new TypeNode(TypeEnum.Element, position.Line, position.Column),
                "FuncTypeDecl"  => CreateFunctionTypeNode(himeNode.Children[0]),
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
            switch (himeNode.Children.Count)
            {
                case 2: return VisitExpressionWithTwoChildren(himeNode); // Unary operator expression 
                case 3: return VisitExpressionWithThreeChildren(himeNode); // Binary operator expression
                default: throw new UnimplementedASTException(himeNode.Symbol.Name, "symbol");
            }
        }

        private ExpressionNode VisitExpressionWithTwoChildren(ASTNode himeNode)
        {
            NegativeExpression negativeExpression = new NegativeExpression(new List<ExpressionNode>(),
                                                                    himeNode.Children[0].Position.Line,
                                                                    himeNode.Children[0].Position.Column);
            
            ExpressionNode rightOperand = DispatchExpression(himeNode.Children[1]);
            negativeExpression.Children.Add(rightOperand);
            return negativeExpression;
        }

        private ExpressionNode VisitExpressionWithThreeChildren(ASTNode himeNode)
        {
            ExpressionNode leftOperant = DispatchExpression(himeNode.Children[0]);
            ExpressionNode rightOperant = DispatchExpression(himeNode.Children[2]);

            TextPosition position = himeNode.Children[1].Position;
            return himeNode.Children[1].Value switch
            {
                "==" => new EqualExpression(leftOperant, rightOperant,
                                            position.Line,
                                            position.Column),
                "!=" => new NotEqualExpression(leftOperant, rightOperant,
                                                position.Line,
                                                position.Column),
                "or" => new OrExpression(leftOperant, rightOperant,
                                         position.Line,
                                         position.Column),
                "and" => new AndExpression(leftOperant, rightOperant,
                                           position.Line,
                                           position.Column),
                "+" => new AdditionExpression(leftOperant, rightOperant,
                                              position.Line, 
                                              position.Column),
                "-" => new SubtractionExpression(leftOperant, rightOperant,
                                                 position.Line, 
                                                 position.Column),
                "*" => new MultiplicationExpression(leftOperant, rightOperant,
                                                    position.Line,
                                                    position.Column),
                "/" => new DivisionExpression(leftOperant, rightOperant,
                                              position.Line,
                                              position.Column),
                "mod" => new ModuloExpression(leftOperant, rightOperant,
                                              position.Line,
                                              position.Column),
                "^" => new PowerExpression(leftOperant, rightOperant,
                                           position.Line, 
                                           position.Column),
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
                ASTNode op = himeNode.Children[1].Children[0];
                if (op.Children.Count != 0)
                    op = op.Children[0];

                TextPosition position = op.Position;
                return op.Value switch
                {
                    ">=" => new GreaterEqualExpression(leftOperant, rightOperant,
                                            position.Line,
                                            position.Column),
                    ">" => new GreaterExpression(leftOperant, rightOperant,
                                            position.Line,
                                            position.Column),
                    "<" => new LessExpression(leftOperant, rightOperant,
                                            position.Line,
                                            position.Column),
                    "<=" => new LessEqualExpression(leftOperant, rightOperant,
                                            position.Line,
                                            position.Column),
                    _ => throw new UnimplementedASTException(op.Value, "operator")
                };
            }
                
        }

        private ExpressionNode VisitExponent(ASTNode himeNode)
        {
            return himeNode.Children[0].Value switch
            {
                "(" => DispatchExpression(himeNode.Children[1]),
                "|" => new AbsoluteValueExpression(DispatchExpression(himeNode.Children[1]),
                                                  himeNode.Children[0].Position.Line,
                                                  himeNode.Children[0].Position.Column),
                "{" => GetSet(himeNode),
                "element" => GetElementExpression(himeNode),
                _ => GetFunctionCall(himeNode)
            };
        }

        private ExpressionNode GetElementExpression(ASTNode himeNode)
        {
            List<ExpressionNode> children = new List<ExpressionNode>(); 
            VisitExpressions(himeNode, children);
            return new ElementExpression(children,
                                         himeNode.Children[0].Position.Line,
                                         himeNode.Children[0].Position.Column);
        }

        private SetExpression GetSet(ASTNode himeNode)
        {
            ExpressionNode predicate = (himeNode.Children.Count == SET_WITH_PREDICATE) ? 
                                        DispatchExpression(himeNode.Children[6]) : null;
            ElementNode element = GetElementNode(himeNode.Children[1]);
            List <BoundNode> bounds = VisitBounds(himeNode.Children[3]);
            TextPosition position = himeNode.Children[0].Position;
            return new SetExpression(element, bounds, predicate, position.Line, position.Column);
        }

        private ElementNode GetElementNode(ASTNode himeNode)
        {
            ASTNode himeElement = himeNode.Children[1];
            TextPosition position = himeElement.Children[0].Position;
            return new ElementNode(himeElement.Children[0].Value,
                                                  VisitIdentifiers(himeElement.Children[2]),
                                                  position.Line, position.Column);
        }

        private List<BoundNode> VisitBounds(ASTNode himeNode)
        {
            if (himeNode.Children.Count == 1)
                return new List<BoundNode> { CreateBoundNode(himeNode.Children[0]) };
            else
            {
                List<BoundNode> identifiers = VisitBounds(himeNode.Children[0]);
                identifiers.Add(CreateBoundNode(himeNode.Children[2]));
                return identifiers;
            }
        }

        private BoundNode CreateBoundNode(ASTNode himeNode)
        {
            TextPosition position;
            if (himeNode.Children.Count == DOUBLE_BOUNDS)
            {
                position = himeNode.Children[3].Position;
                return new BoundNode(himeNode.Children[3].Value,
                                     GetLimit(himeNode.Children[0], himeNode.Children[1].Value, 1),
                                     GetLimit(himeNode.Children[6], himeNode.Children[5].Value, -1), 
                                     position.Line, position.Column);
            }
            else
            {
                position = himeNode.Children[1].Position;
                ExpressionNode value = DispatchExpression(himeNode.Children[4]);
                return new BoundNode(himeNode.Children[1].Value, value, value, position.Line, position.Column);
            }

        }

        private ExpressionNode GetLimit(ASTNode himeNode, string comp, int adjustment)
        {
            ExpressionNode value = DispatchExpression(himeNode.Children[0]);
            int line = value.LineNumber;
            int letter = value.LetterNumber;
            if (comp == "<")
                value = new AdditionExpression(value, new IntegerLiteralExpression(adjustment, line, letter), line, letter);
            return value;
        }

        private ExpressionNode GetFunctionCall(ASTNode himeNode)
        {
            List<ExpressionNode> expressions = new List<ExpressionNode>();

            if (himeNode.Children.Count != CONSTANT_FUNCTION_CALL)
                VisitExpressions(himeNode.Children[EXPRESSIONS_POS], expressions);
            return new FunctionCallExpression(himeNode.Children[0].Value, expressions,
                                              himeNode.Children[0].Position.Line,
                                              himeNode.Children[0].Position.Column);
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