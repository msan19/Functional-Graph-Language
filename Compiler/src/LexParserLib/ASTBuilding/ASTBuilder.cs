using System;
using System.Collections.Generic;
using ASTLib;
using ASTLib.Exceptions;
using ASTLib.Nodes;
using ASTLib.Nodes.ExpressionNodes;
using ASTLib.Nodes.TypeNodes;
using Hime.Redist;

namespace LexParserLib.ASTBuilding
{
    public class ASTBuilder
    {
        private readonly AstBuilderSettings _conf = new AstBuilderSettings();
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
                ast = GetNewAst();
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

        private AST GetNewAst()
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
            ExpressionNode expressionNode   = _expressionHelper.DispatchExpression(himeNode.Children[1]);
            ExpressionNode fileName = _expressionHelper.DispatchExpression(himeNode.Children[3]);
            List<ExpressionNode> vertexLabels   = new List<ExpressionNode>();
            List<ExpressionNode> edgeLabels     = new List<ExpressionNode>();
            
            if(himeNode.Children.Count == _conf.EXPORT_VERTEX_LABELS ||
               himeNode.Children.Count == _conf.EXPORT_BOTH_LABELS)
                _expressionHelper.VisitExpressions(himeNode.Children[6], vertexLabels);
            if(himeNode.Children.Count == _conf.EXPORT_EDGE_LABELS)
                _expressionHelper.VisitExpressions(himeNode.Children[8], edgeLabels);
            if(himeNode.Children.Count == _conf.EXPORT_BOTH_LABELS)
                _expressionHelper.VisitExpressions(himeNode.Children[9], edgeLabels);

            TextPosition position = himeNode.Children[0].Position;
            return new ExportNode(expressionNode, fileName, vertexLabels, edgeLabels, 
                                  position.Line, position.Column);
        }
        
        private FunctionNode CreateFunctionNode(ASTNode himeDeclNode)
        {
            ASTNode himeFuncNode = GetHimeFuncNode(himeDeclNode);
            FunctionTypeNode type = _expressionHelper.CreateFunctionTypeNode(himeFuncNode.Children[_conf.FUNCTIONTYPE_POS]);

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
            ExpressionNode returnExpression = null;
            TextPosition position = himeNode.Children[0].Position;
            if (himeNode.Children[1].Value == "_")
            {

                returnExpression = _expressionHelper.DispatchExpression(himeNode.Children[3]);
                return new ConditionNode(returnExpression,
                                         position.Line, position.Column);
            } 
            else
            {
                bool both = himeNode.Children.Count == _conf.CONDITION_BOTH_ELEMENTS_AND_PREDICATE;
                string symbol = himeNode.Children[1].Symbol.Name;

                ExpressionNode conditionExpr = null;
                if(both)
                {
                    conditionExpr = _expressionHelper.DispatchExpression(himeNode.Children[3]);
                    returnExpression = _expressionHelper.DispatchExpression(himeNode.Children[5]);
                } 
                else
                    returnExpression = _expressionHelper.DispatchExpression(himeNode.Children[3]);
                
                List<ElementNode> elements = new List<ElementNode>();
                if (symbol == "Elements")
                    elements = VisitElements(himeNode.Children[1]);
                else
                    conditionExpr = _expressionHelper.DispatchExpression(himeNode.Children[1]);
                return new ConditionNode(elements, conditionExpr, returnExpression,
                                         position.Line, position.Column);
            }            
        }

        private List<ElementNode> VisitElements(ASTNode himeNode)
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
                return _expressionHelper.VisitIdentifiers(himeFuncNode.Children[_conf.PARAMETER_IDs_POS]);
        }

        private bool IsParameterLessFunctionDeclaration(ASTNode himeFuncNode)
        {
            return himeFuncNode.Children.Count == _conf.CONSTANT_FUNCTION_DECLARATION;
        }
    
    }
    
}