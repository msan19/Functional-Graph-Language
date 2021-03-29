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
			/// The unique identifier for variable Func
			/// </summary>
			public const int VariableFunc = 0x000D;
			/// <summary>
			/// The unique identifier for variable FuncTypeDecl
			/// </summary>
			public const int VariableFuncTypeDecl = 0x000E;
			/// <summary>
			/// The unique identifier for variable Types
			/// </summary>
			public const int VariableTypes = 0x000F;
			/// <summary>
			/// The unique identifier for variable Type
			/// </summary>
			public const int VariableType = 0x0010;
			/// <summary>
			/// The unique identifier for variable Ids
			/// </summary>
			public const int VariableIds = 0x0011;
			/// <summary>
			/// The unique identifier for variable Expression
			/// </summary>
			public const int VariableExpression = 0x0012;
			/// <summary>
			/// The unique identifier for variable MaxTerm
			/// </summary>
			public const int VariableMaxTerm = 0x0013;
			/// <summary>
			/// The unique identifier for variable MinTerm
			/// </summary>
			public const int VariableMinTerm = 0x0014;
			/// <summary>
			/// The unique identifier for variable LogicTerm
			/// </summary>
			public const int VariableLogicTerm = 0x0015;
			/// <summary>
			/// The unique identifier for variable Compare
			/// </summary>
			public const int VariableCompare = 0x0016;
			/// <summary>
			/// The unique identifier for variable BoundComp
			/// </summary>
			public const int VariableBoundComp = 0x0017;
			/// <summary>
			/// The unique identifier for variable NumExpression
			/// </summary>
			public const int VariableNumExpression = 0x0018;
			/// <summary>
			/// The unique identifier for variable Term
			/// </summary>
			public const int VariableTerm = 0x0019;
			/// <summary>
			/// The unique identifier for variable Factor
			/// </summary>
			public const int VariableFactor = 0x001A;
			/// <summary>
			/// The unique identifier for variable Exponent
			/// </summary>
			public const int VariableExponent = 0x001B;
			/// <summary>
			/// The unique identifier for variable Expressions
			/// </summary>
			public const int VariableExpressions = 0x001C;
			/// <summary>
			/// The unique identifier for variable Literal
			/// </summary>
			public const int VariableLiteral = 0x001D;
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
			new Symbol(0x000D, "Func"), 
			new Symbol(0x000E, "FuncTypeDecl"), 
			new Symbol(0x000F, "Types"), 
			new Symbol(0x0010, "Type"), 
			new Symbol(0x0011, "Ids"), 
			new Symbol(0x0012, "Expression"), 
			new Symbol(0x0013, "MaxTerm"), 
			new Symbol(0x0014, "MinTerm"), 
			new Symbol(0x0015, "LogicTerm"), 
			new Symbol(0x0016, "Compare"), 
			new Symbol(0x0017, "BoundComp"), 
			new Symbol(0x0018, "NumExpression"), 
			new Symbol(0x0019, "Term"), 
			new Symbol(0x001A, "Factor"), 
			new Symbol(0x001B, "Exponent"), 
			new Symbol(0x001C, "Expressions"), 
			new Symbol(0x001D, "Literal"), 
			new Symbol(0x003B, "__VAxiom") };
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
			public virtual void OnVariableFunc(ASTNode node) {}
			public virtual void OnVariableFuncTypeDecl(ASTNode node) {}
			public virtual void OnVariableTypes(ASTNode node) {}
			public virtual void OnVariableType(ASTNode node) {}
			public virtual void OnVariableIds(ASTNode node) {}
			public virtual void OnVariableExpression(ASTNode node) {}
			public virtual void OnVariableMaxTerm(ASTNode node) {}
			public virtual void OnVariableMinTerm(ASTNode node) {}
			public virtual void OnVariableLogicTerm(ASTNode node) {}
			public virtual void OnVariableCompare(ASTNode node) {}
			public virtual void OnVariableBoundComp(ASTNode node) {}
			public virtual void OnVariableNumExpression(ASTNode node) {}
			public virtual void OnVariableTerm(ASTNode node) {}
			public virtual void OnVariableFactor(ASTNode node) {}
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
				case 0x000D: visitor.OnVariableFunc(node); break;
				case 0x000E: visitor.OnVariableFuncTypeDecl(node); break;
				case 0x000F: visitor.OnVariableTypes(node); break;
				case 0x0010: visitor.OnVariableType(node); break;
				case 0x0011: visitor.OnVariableIds(node); break;
				case 0x0012: visitor.OnVariableExpression(node); break;
				case 0x0013: visitor.OnVariableMaxTerm(node); break;
				case 0x0014: visitor.OnVariableMinTerm(node); break;
				case 0x0015: visitor.OnVariableLogicTerm(node); break;
				case 0x0016: visitor.OnVariableCompare(node); break;
				case 0x0017: visitor.OnVariableBoundComp(node); break;
				case 0x0018: visitor.OnVariableNumExpression(node); break;
				case 0x0019: visitor.OnVariableTerm(node); break;
				case 0x001A: visitor.OnVariableFactor(node); break;
				case 0x001B: visitor.OnVariableExponent(node); break;
				case 0x001C: visitor.OnVariableExpressions(node); break;
				case 0x001D: visitor.OnVariableLiteral(node); break;
			}
		}
	}
}
