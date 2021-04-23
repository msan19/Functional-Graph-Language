using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Exceptions;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.ExpressionNodes.BooleanOperationNodes;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes.ElementAndSetOperations;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes.GraphFields;
using ASTLib.Nodes.ExpressionNodes.CommonOperationNodes.RelationalOperationNodes;
using ASTLib.Nodes.ExpressionNodes.NumberOperationNodes;
using ASTLib.Nodes.ExpressionNodes.OperationNodes;
using ASTLib.Nodes.ExpressionNodes.SetOperationNodes;
using ASTLib.Nodes.TypeNodes;
using Hime.Redist;

namespace LexParserLib
{
    public class ExpressionHelper : IExpressionHelper
    {
        private const int RETURNTYPE_POS = 4, 
                          CONSTANT_FUNCTION_CALL = 3, EXPRESSIONS_POS = 2,
                          SET_WITH_PREDICATE = 7, DOUBLE_BOUNDS = 7,
                          PARENTHESES = 3, GRAPH = 9, CONSTANT_ANONYMOUS_FUNCTION = 4;

        public ExpressionNode DispatchExpression(ASTNode himeNode)
        {
            switch (himeNode.Symbol.Name)
            {
                case "id":
                    return new IdentifierExpression(himeNode.Value,
                                             himeNode.Position.Line,
                                             himeNode.Position.Column);
                case "Literal": return VisitLiteral(himeNode.Children[0]);
                default: break;
            }

            if (himeNode.Children.Count == 1) return DispatchExpression(himeNode.Children[0]);

            return himeNode.Symbol.Name switch
            {
                "Expression" => GetAnonymousFunction(himeNode),
                "LogicExpression" => VisitExpression(himeNode),
                "MaxTerm" => VisitExpression(himeNode),
                "MinTerm" => VisitExpression(himeNode),
                "LogicTerm" => VisitLogicTerm(himeNode),
                "NumExpression" => VisitExpression(himeNode),
                "Term" => VisitExpression(himeNode),
                "Factor" => VisitExpression(himeNode),
                "SetExpression" => VisitExpression(himeNode),
                "SetTerm" => VisitExpression(himeNode),
                "Exponent" => VisitExponent(himeNode),
                _ => throw new UnimplementedASTException(himeNode.Symbol.Name, "symbol"),
            };
        }

        public ExpressionNode GetAnonymousFunction(ASTNode himeNode)
        {

            List<string> identifiers = new List<string>();
            List<TypeNode> types = new List<TypeNode>();
            ExpressionNode returnValue;

            if (himeNode.Children.Count == CONSTANT_ANONYMOUS_FUNCTION)
                returnValue = DispatchExpression(himeNode.Children[3]);
            else
            {
                VisitPairs(himeNode.Children[1], identifiers, types);
                returnValue = DispatchExpression(himeNode.Children[4]);
            }

            TextPosition position = himeNode.Children[0].Position;
            return new AnonymousFunctionExpression(identifiers, types, returnValue, 
                                                   position.Line, position.Column);
        }

        public void VisitPairs(ASTNode himeNode, List<string> identifiers, List<TypeNode> types)
        {
            if (himeNode.Children.Count == 1)
                VisitPair(himeNode, identifiers, types);
            else
            {
                VisitPairs(himeNode.Children[0], identifiers, types);
                VisitPair(himeNode.Children[2], identifiers, types);
            }
        }

        public void VisitPair(ASTNode himeNode, List<string> identifiers, List<TypeNode> types)
        {
            types.Add(CreateFunctionTypeNode(himeNode.Children[0]));
            identifiers.Add(himeNode.Children[0].Value);
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
                "integer" => new TypeNode(TypeEnum.Integer, position.Line, position.Column),
                "real" => new TypeNode(TypeEnum.Real, position.Line, position.Column),
                "boolean" => new TypeNode(TypeEnum.Boolean, position.Line, position.Column),
                "string" => new TypeNode(TypeEnum.String, position.Line, position.Column),
                "set" => new TypeNode(TypeEnum.Set, position.Line, position.Column),
                "element" => new TypeNode(TypeEnum.Element, position.Line, position.Column),
                "graph" => new TypeNode(TypeEnum.Graph, position.Line, position.Column),
                "FuncTypeDecl" => CreateFunctionTypeNode(himeNode.Children[0]),
                _ => throw new UnimplementedASTException(himeNode.Children[0].Symbol.Name, "type"),
            };
        }

        public List<TypeNode> VisitTypes(ASTNode himeNode)
        {
            if (himeNode.Children.Count == 1)
            {
                return new List<TypeNode> { CreateTypeNode(himeNode.Children[0]) };
            }
            else
            {
                List<TypeNode> types = VisitTypes(himeNode.Children[0]);
                types.Add(CreateTypeNode(himeNode.Children[2]));
                return types;
            }
        }

        public ExpressionNode VisitExpression(ASTNode himeNode)
        {
            return himeNode.Children.Count switch
            {
                2 => VisitExpressionWithTwoChildren(himeNode), 
                3 => VisitExpressionWithThreeChildren(himeNode),
                _ => throw new UnimplementedASTException(himeNode.Symbol.Name, "symbol"),
            };
        }

        public ExpressionNode VisitExpressionWithTwoChildren(ASTNode himeNode)
        {
            NegativeExpression negativeExpression = new NegativeExpression(new List<ExpressionNode>(),
                                                                    himeNode.Children[0].Position.Line,
                                                                    himeNode.Children[0].Position.Column);

            ExpressionNode rightOperand = DispatchExpression(himeNode.Children[1]);
            negativeExpression.Children.Add(rightOperand);
            return negativeExpression;
        }

        public ExpressionNode VisitExpressionWithThreeChildren(ASTNode himeNode)
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
                "intersection" => new IntersectionExpression(leftOperant, rightOperant,
                                                             position.Line,
                                                             position.Column),
                "union" => new UnionExpression(leftOperant, rightOperant,
                                               position.Line,
                                               position.Column),
                "subset" => new SubsetExpression(leftOperant, rightOperant,
                                                 position.Line,
                                                 position.Column),
                "in" => new InExpression(leftOperant, rightOperant,
                                         position.Line,
                                         position.Column),
                _ => throw new UnimplementedASTException(himeNode.Children[1].Value, "operator")
            };
        }

        public ExpressionNode VisitLogicTerm(ASTNode himeNode)
        {
            if (himeNode.Children.Count == 2)
            {
                ExpressionNode expr = DispatchExpression(himeNode.Children[1]);
                return new NotExpression(expr, himeNode.Position.Line,
                                         himeNode.Position.Column);
            }
            else
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

        public ExpressionNode VisitExponent(ASTNode himeNode)
        {
            return himeNode.Children[0].Symbol.Name switch
            {
                "(" when himeNode.Children.Count == PARENTHESES 
                    => DispatchExpression(himeNode.Children[1]),
                "(" when himeNode.Children.Count == GRAPH
                    => GetGraph(himeNode),
                "|" => new AbsoluteValueExpression(DispatchExpression(himeNode.Children[1]),
                                                  himeNode.Children[0].Position.Line,
                                                  himeNode.Children[0].Position.Column),
                "{" when himeNode.Children.Count == 3
                    => GetSet(himeNode),
                "{" => GetSetBuilder(himeNode),
                "element" => GetElementExpression(himeNode),
                "exponent" => GetField(himeNode),
                "id" => GetFunctionCall(himeNode),
                _ => throw new UnimplementedASTException(himeNode.Children[0].Value, "exponent")
            };
        }

        private ExpressionNode GetSet(ASTNode himeNode)
        {
            TextPosition position = himeNode.Children[0].Position;
            List<ExpressionNode> expressions = new List<ExpressionNode>();
            VisitExpressions(himeNode.Children[1], expressions);
            return new SetExpression(expressions, position.Line, position.Column);
        }

        public ExpressionNode GetField(ASTNode himeNode)
        {
            ExpressionNode graph = DispatchExpression(himeNode.Children[0]);
            TextPosition position = himeNode.Children[1].Position;
            return himeNode.Children[1].Value switch
            {
                ".V" => new VerticesGraphField(graph, position.Line, position.Column),
                ".E" => new EdgesGraphField(graph, position.Line, position.Column),
                ".src" => new SrcGraphField(graph, position.Line, position.Column),
                ".dst" => new DstGraphField(graph, position.Line, position.Column),
                _ => throw new UnimplementedASTException(himeNode.Children[1].Value, "Graph Field")
            };
        }

        public ExpressionNode GetGraph(ASTNode himeNode)
        {
            ExpressionNode vertices = DispatchExpression(himeNode.Children[1]);
            ExpressionNode edges = DispatchExpression(himeNode.Children[3]);
            ExpressionNode src = DispatchExpression(himeNode.Children[5]);
            ExpressionNode dst = DispatchExpression(himeNode.Children[7]);

            TextPosition position = himeNode.Children[0].Position;
            return new GraphExpression(vertices, edges, src, dst, position.Line, position.Column);
        }

        public ExpressionNode GetElementExpression(ASTNode himeNode)
        {
            List<ExpressionNode> children = new List<ExpressionNode>();
            VisitExpressions(himeNode.Children[2], children);
            return new ElementExpression(children,
                                         himeNode.Children[0].Position.Line,
                                         himeNode.Children[0].Position.Column);
        }

        public SetExpression GetSetBuilder(ASTNode himeNode)
        {
            ExpressionNode predicate = (himeNode.Children.Count == SET_WITH_PREDICATE) ?
                                        DispatchExpression(himeNode.Children[5]) : new BooleanLiteralExpression(true, 0, 0);
            ElementNode element = GetElementNode(himeNode.Children[1]);
            List<BoundNode> bounds = VisitBounds(himeNode.Children[3]);
            TextPosition position = himeNode.Children[0].Position;
            return new SetExpression(element, bounds, predicate, position.Line, position.Column);
        }

        public ElementNode GetElementNode(ASTNode himeNode)
        {
            ASTNode himeElement = himeNode.Children[0];
            TextPosition position = himeElement.Position;
            return new ElementNode(himeElement.Value,
                                                  VisitIdentifiers(himeNode.Children[2]),
                                                  position.Line, position.Column);
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

        public List<BoundNode> VisitBounds(ASTNode himeNode)
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

        public BoundNode CreateBoundNode(ASTNode himeNode)
        {
            TextPosition position;
            if (himeNode.Children.Count == DOUBLE_BOUNDS)
            {
                position = himeNode.Children[3].Position;
                return new BoundNode(himeNode.Children[3].Value,
                                     GetLimit(himeNode.Children[0], himeNode.Children[1].Children[0].Value, 1),
                                     GetLimit(himeNode.Children[6], himeNode.Children[5].Children[0].Value, -1),
                                     position.Line, position.Column);
            }
            else
            {
                position = himeNode.Children[1].Position;
                ExpressionNode value = DispatchExpression(himeNode.Children[4]);
                return new BoundNode(himeNode.Children[1].Value, value, value, position.Line, position.Column);
            }

        }

        public ExpressionNode GetLimit(ASTNode himeNode, string comp, int adjustment)
        {
            ExpressionNode value = DispatchExpression(himeNode);
            int line = value.LineNumber;
            int letter = value.LetterNumber;
            if (comp == "<")
                value = new AdditionExpression(value, new IntegerLiteralExpression(adjustment, line, letter), line, letter);
            return value;
        }

        public ExpressionNode GetFunctionCall(ASTNode himeNode)
        {
            List<ExpressionNode> expressions = new List<ExpressionNode>();

            if (himeNode.Children.Count != CONSTANT_FUNCTION_CALL)
                VisitExpressions(himeNode.Children[EXPRESSIONS_POS], expressions);
            return new FunctionCallExpression(himeNode.Children[0].Value, expressions,
                                              himeNode.Children[0].Position.Line,
                                              himeNode.Children[0].Position.Column);
        }

        public void VisitExpressions(ASTNode himeNode, List<ExpressionNode> expressions)
        {
            if (himeNode.Children.Count == 1)
                expressions.Add(DispatchExpression(himeNode.Children[0]));
            else
            {
                expressions.Add(DispatchExpression(himeNode.Children[0]));
                VisitExpressions(himeNode.Children[2], expressions);
            }
        }

        public ExpressionNode VisitLiteral(ASTNode himeNode)
        {
            return himeNode.Symbol.Name switch
            {
                "realNumber" => new RealLiteralExpression(himeNode.Value,
                                                          himeNode.Position.Line,
                                                          himeNode.Position.Column),
                "integerNumber" => new IntegerLiteralExpression(himeNode.Value,
                                                                himeNode.Position.Line,
                                                                himeNode.Position.Column),
                "stringLiteral" => new StringLiteralExpression(himeNode.Value[1..^1],
                                                               himeNode.Position.Line,
                                                               himeNode.Position.Column),
                "true" => new BooleanLiteralExpression(true,
                                                       himeNode.Position.Line,
                                                       himeNode.Position.Column),
                "false" => new BooleanLiteralExpression(false,
                                                        himeNode.Position.Line,
                                                        himeNode.Position.Column),
                "Ø" => new EmptySetLiteralExpression(himeNode.Position.Line,
                                                     himeNode.Position.Column),
                _ => throw new UnimplementedASTException(himeNode.Symbol.Name, "literal"),
            };
        }

    }

}