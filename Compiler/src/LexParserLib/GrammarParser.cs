/*
 * WARNING: this file has been generated by
 * Hime Parser Generator 3.5.1.0
 */

using System.CodeDom.Compiler;
using Hime.Redist;
using Hime.Redist.Parsers;

namespace LexParserLib
{
	/// <summary>
	/// Represents a parser
	/// </summary>
	[GeneratedCode("Hime.SDK", "3.5.1.0")]
	internal class GrammarParser : LRkParser
	{
		/// <summary>
		/// The automaton for this parser
		/// </summary>
		private static readonly LRkAutomaton commonAutomaton = LRkAutomaton.Find(typeof(GrammarParser), "GrammarParser.bin");
		/// <summary>
		/// Contains the constant IDs for the variables and virtuals in this parser
		/// </summary>
		[GeneratedCode("Hime.SDK", "3.5.1.0")]
		public class ID
		{
			/// <summary>
			/// The unique identifier for variable Prog
			/// </summary>
			public const int VariableProg = 0x0008;
			/// <summary>
			/// The unique identifier for variable Declarations
			/// </summary>
			public const int VariableDeclarations = 0x0009;
			/// <summary>
			/// The unique identifier for variable Declaration
			/// </summary>
			public const int VariableDeclaration = 0x000A;
			/// <summary>
			/// The unique identifier for variable Conditions
			/// </summary>
			public const int VariableConditions = 0x000B;
			/// <summary>
			/// The unique identifier for variable Condition
			/// </summary>
			public const int VariableCondition = 0x000C;
			/// <summary>
			/// The unique identifier for variable Elements
			/// </summary>
			public const int VariableElements = 0x000D;
			/// <summary>
			/// The unique identifier for variable Element
			/// </summary>
			public const int VariableElement = 0x000E;
			/// <summary>
			/// The unique identifier for variable Func
			/// </summary>
			public const int VariableFunc = 0x000F;
			/// <summary>
			/// The unique identifier for variable FuncTypeDecl
			/// </summary>
			public const int VariableFuncTypeDecl = 0x0010;
			/// <summary>
			/// The unique identifier for variable Types
			/// </summary>
			public const int VariableTypes = 0x0011;
			/// <summary>
			/// The unique identifier for variable Type
			/// </summary>
			public const int VariableType = 0x0012;
			/// <summary>
			/// The unique identifier for variable Ids
			/// </summary>
			public const int VariableIds = 0x0013;
			/// <summary>
			/// The unique identifier for variable Bounds
			/// </summary>
			public const int VariableBounds = 0x0014;
			/// <summary>
			/// The unique identifier for variable Bound
			/// </summary>
			public const int VariableBound = 0x0015;
			/// <summary>
			/// The unique identifier for variable Expression
			/// </summary>
			public const int VariableExpression = 0x0016;
			/// <summary>
			/// The unique identifier for variable MaxTerm
			/// </summary>
			public const int VariableMaxTerm = 0x0017;
			/// <summary>
			/// The unique identifier for variable MinTerm
			/// </summary>
			public const int VariableMinTerm = 0x0018;
			/// <summary>
			/// The unique identifier for variable LogicTerm
			/// </summary>
			public const int VariableLogicTerm = 0x0019;
			/// <summary>
			/// The unique identifier for variable Compare
			/// </summary>
			public const int VariableCompare = 0x001A;
			/// <summary>
			/// The unique identifier for variable BoundComp
			/// </summary>
			public const int VariableBoundComp = 0x001B;
			/// <summary>
			/// The unique identifier for variable NumExpression
			/// </summary>
			public const int VariableNumExpression = 0x001C;
			/// <summary>
			/// The unique identifier for variable Term
			/// </summary>
			public const int VariableTerm = 0x001D;
			/// <summary>
			/// The unique identifier for variable Factor
			/// </summary>
			public const int VariableFactor = 0x001E;
			/// <summary>
			/// The unique identifier for variable SetExpression
			/// </summary>
			public const int VariableSetExpression = 0x001F;
			/// <summary>
			/// The unique identifier for variable SetTerm
			/// </summary>
			public const int VariableSetTerm = 0x0020;
			/// <summary>
			/// The unique identifier for variable Exponent
			/// </summary>
			public const int VariableExponent = 0x0021;
			/// <summary>
			/// The unique identifier for variable Expressions
			/// </summary>
			public const int VariableExpressions = 0x0022;
			/// <summary>
			/// The unique identifier for variable Literal
			/// </summary>
			public const int VariableLiteral = 0x0023;
		}
		/// <summary>
		/// The collection of variables matched by this parser
		/// </summary>
		/// <remarks>
		/// The variables are in an order consistent with the automaton,
		/// so that variable indices in the automaton can be used to retrieve the variables in this table
		/// </remarks>
		private static readonly Symbol[] variables = {
			new Symbol(0x0008, "Prog"), 
			new Symbol(0x0009, "Declarations"), 
			new Symbol(0x000A, "Declaration"), 
			new Symbol(0x000B, "Conditions"), 
			new Symbol(0x000C, "Condition"), 
			new Symbol(0x000D, "Elements"), 
			new Symbol(0x000E, "Element"), 
			new Symbol(0x000F, "Func"), 
			new Symbol(0x0010, "FuncTypeDecl"), 
			new Symbol(0x0011, "Types"), 
			new Symbol(0x0012, "Type"), 
			new Symbol(0x0013, "Ids"), 
			new Symbol(0x0014, "Bounds"), 
			new Symbol(0x0015, "Bound"), 
			new Symbol(0x0016, "Expression"), 
			new Symbol(0x0017, "MaxTerm"), 
			new Symbol(0x0018, "MinTerm"), 
			new Symbol(0x0019, "LogicTerm"), 
			new Symbol(0x001A, "Compare"), 
			new Symbol(0x001B, "BoundComp"), 
			new Symbol(0x001C, "NumExpression"), 
			new Symbol(0x001D, "Term"), 
			new Symbol(0x001E, "Factor"), 
			new Symbol(0x001F, "SetExpression"), 
			new Symbol(0x0020, "SetTerm"), 
			new Symbol(0x0021, "Exponent"), 
			new Symbol(0x0022, "Expressions"), 
			new Symbol(0x0023, "Literal"), 
			new Symbol(0x004B, "__VAxiom") };
		/// <summary>
		/// The collection of virtuals matched by this parser
		/// </summary>
		/// <remarks>
		/// The virtuals are in an order consistent with the automaton,
		/// so that virtual indices in the automaton can be used to retrieve the virtuals in this table
		/// </remarks>
		private static readonly Symbol[] virtuals = {
 };
		/// <summary>
		/// Initializes a new instance of the parser
		/// </summary>
		/// <param name="lexer">The input lexer</param>
		public GrammarParser(GrammarLexer lexer) : base (commonAutomaton, variables, virtuals, null, lexer) { }

		/// <summary>
		/// Visitor interface
		/// </summary>
		[GeneratedCode("Hime.SDK", "3.5.1.0")]
		public class Visitor
		{
			public virtual void OnTerminalId(ASTNode node) {}
			public virtual void OnTerminalRealNumber(ASTNode node) {}
			public virtual void OnTerminalIntegerNumber(ASTNode node) {}
			public virtual void OnTerminalWhiteSpace(ASTNode node) {}
			public virtual void OnTerminalSeparator(ASTNode node) {}
			public virtual void OnVariableProg(ASTNode node) {}
			public virtual void OnVariableDeclarations(ASTNode node) {}
			public virtual void OnVariableDeclaration(ASTNode node) {}
			public virtual void OnVariableConditions(ASTNode node) {}
			public virtual void OnVariableCondition(ASTNode node) {}
			public virtual void OnVariableElements(ASTNode node) {}
			public virtual void OnVariableElement(ASTNode node) {}
			public virtual void OnVariableFunc(ASTNode node) {}
			public virtual void OnVariableFuncTypeDecl(ASTNode node) {}
			public virtual void OnVariableTypes(ASTNode node) {}
			public virtual void OnVariableType(ASTNode node) {}
			public virtual void OnVariableIds(ASTNode node) {}
			public virtual void OnVariableBounds(ASTNode node) {}
			public virtual void OnVariableBound(ASTNode node) {}
			public virtual void OnVariableExpression(ASTNode node) {}
			public virtual void OnVariableMaxTerm(ASTNode node) {}
			public virtual void OnVariableMinTerm(ASTNode node) {}
			public virtual void OnVariableLogicTerm(ASTNode node) {}
			public virtual void OnVariableCompare(ASTNode node) {}
			public virtual void OnVariableBoundComp(ASTNode node) {}
			public virtual void OnVariableNumExpression(ASTNode node) {}
			public virtual void OnVariableTerm(ASTNode node) {}
			public virtual void OnVariableFactor(ASTNode node) {}
			public virtual void OnVariableSetExpression(ASTNode node) {}
			public virtual void OnVariableSetTerm(ASTNode node) {}
			public virtual void OnVariableExponent(ASTNode node) {}
			public virtual void OnVariableExpressions(ASTNode node) {}
			public virtual void OnVariableLiteral(ASTNode node) {}
		}

		/// <summary>
		/// Walk the AST of a result using a visitor
		/// <param name="result">The parse result</param>
		/// <param name="visitor">The visitor to use</param>
		/// </summary>
		public static void Visit(ParseResult result, Visitor visitor)
		{
			VisitASTNode(result.Root, visitor);
		}

		/// <summary>
		/// Walk the sub-AST from the specified node using a visitor
		/// </summary>
		/// <param name="node">The AST node to start from</param>
		/// <param name="visitor">The visitor to use</param>
		public static void VisitASTNode(ASTNode node, Visitor visitor)
		{
			for (int i = 0; i < node.Children.Count; i++)
				VisitASTNode(node.Children[i], visitor);
			switch(node.Symbol.ID)
			{
				case 0x0003: visitor.OnTerminalId(node); break;
				case 0x0004: visitor.OnTerminalRealNumber(node); break;
				case 0x0005: visitor.OnTerminalIntegerNumber(node); break;
				case 0x0006: visitor.OnTerminalWhiteSpace(node); break;
				case 0x0007: visitor.OnTerminalSeparator(node); break;
				case 0x0008: visitor.OnVariableProg(node); break;
				case 0x0009: visitor.OnVariableDeclarations(node); break;
				case 0x000A: visitor.OnVariableDeclaration(node); break;
				case 0x000B: visitor.OnVariableConditions(node); break;
				case 0x000C: visitor.OnVariableCondition(node); break;
				case 0x000D: visitor.OnVariableElements(node); break;
				case 0x000E: visitor.OnVariableElement(node); break;
				case 0x000F: visitor.OnVariableFunc(node); break;
				case 0x0010: visitor.OnVariableFuncTypeDecl(node); break;
				case 0x0011: visitor.OnVariableTypes(node); break;
				case 0x0012: visitor.OnVariableType(node); break;
				case 0x0013: visitor.OnVariableIds(node); break;
				case 0x0014: visitor.OnVariableBounds(node); break;
				case 0x0015: visitor.OnVariableBound(node); break;
				case 0x0016: visitor.OnVariableExpression(node); break;
				case 0x0017: visitor.OnVariableMaxTerm(node); break;
				case 0x0018: visitor.OnVariableMinTerm(node); break;
				case 0x0019: visitor.OnVariableLogicTerm(node); break;
				case 0x001A: visitor.OnVariableCompare(node); break;
				case 0x001B: visitor.OnVariableBoundComp(node); break;
				case 0x001C: visitor.OnVariableNumExpression(node); break;
				case 0x001D: visitor.OnVariableTerm(node); break;
				case 0x001E: visitor.OnVariableFactor(node); break;
				case 0x001F: visitor.OnVariableSetExpression(node); break;
				case 0x0020: visitor.OnVariableSetTerm(node); break;
				case 0x0021: visitor.OnVariableExponent(node); break;
				case 0x0022: visitor.OnVariableExpressions(node); break;
				case 0x0023: visitor.OnVariableLiteral(node); break;
			}
		}
	}
}
