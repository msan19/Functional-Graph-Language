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
                          RETURNTYPE_POS = 4, CONSTANT_FUNCTION_DECLARATION = 6,
                          CONDITION_BOTH_ELEMENTS_AND_PREDICATE = 6;
        private readonly IExpressionHelper _expressionHelper;
        
        public ASTBuilder(IExpressionHelper expressionHelper)
        {
            _expressionHelper = expressionHelper;
        }

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
            ExpressionNode expressionNode = _expressionHelper.DispatchExpression(himeNode.Children[1]);
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
            return new ConditionNode(_expressionHelper.DispatchExpression(expr),
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
            ExpressionNode returnExpression = _expressionHelper.DispatchExpression(expr);

            TextPosition position = himeNode.Children[0].Position;
            if (himeNode.Children[1].Value == "_")
            {
                return new ConditionNode(returnExpression,
                                         position.Line, position.Column);
            } else
            {
                bool both = himeNode.Children.Count == CONDITION_BOTH_ELEMENTS_AND_PREDICATE;
                string symbol = himeNode.Children[1].Symbol.Name;

                ExpressionNode conditionExpr = both ? 
                    _expressionHelper.DispatchExpression(himeNode.Children[3]) : 
                    null;
                List<ElementNode> elements = null;
                if (symbol == "Elements")
                    elements = VisitElements(himeNode.Children[1]);
                else
                    conditionExpr = _expressionHelper.DispatchExpression(himeNode.Children[1]);
                return new ConditionNode(elements, conditionExpr, returnExpression,
                                         position.Line, position.Column);
            }            
        }

        public List<ElementNode> VisitElements(ASTNode himeNode)
        {
            if (himeNode.Children.Count == 1)
            {
                return new List<ElementNode> { _expressionHelper.GetElementNode(himeNode.Children[0]) };
            }
            else
            {
                List<ElementNode> elements = VisitElements(himeNode.Children[0]);
                elements.Add(_expressionHelper.GetElementNode(himeNode.Children[2]));
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
                return _expressionHelper.VisitIdentifiers(himeFuncNode.Children[PARAMETER_IDs_POS]);
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
    
    }
    
}