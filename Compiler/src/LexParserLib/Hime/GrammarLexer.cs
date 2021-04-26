/*
 * WARNING: this file has been generated by
 * Hime Parser Generator 3.5.1.0
 */
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using Hime.Redist;
using Hime.Redist.Lexer;

namespace LexParserLib
{
	/// <summary>
	/// Represents a lexer
	/// </summary>
	[GeneratedCodeAttribute("Hime.SDK", "3.5.1.0")]
	internal class GrammarLexer : ContextFreeLexer
	{
		/// <summary>
		/// The automaton for this lexer
		/// </summary>
		private static readonly Automaton commonAutomaton = Automaton.Find(typeof(GrammarLexer), "GrammarLexer.bin");
		/// <summary>
		/// Contains the constant IDs for the terminals for this lexer
		/// </summary>
		[GeneratedCodeAttribute("Hime.SDK", "3.5.1.0")]
		public class ID
		{
			/// <summary>
			/// The unique identifier for terminal id
			/// </summary>
			public const int TerminalId = 0x0003;
			/// <summary>
			/// The unique identifier for terminal realNumber
			/// </summary>
			public const int TerminalRealNumber = 0x0004;
			/// <summary>
			/// The unique identifier for terminal integerNumber
			/// </summary>
			public const int TerminalIntegerNumber = 0x0005;
			/// <summary>
			/// The unique identifier for terminal emptySet
			/// </summary>
			public const int TerminalEmptySet = 0x0006;
			/// <summary>
			/// The unique identifier for terminal stringLiteral
			/// </summary>
			public const int TerminalStringLiteral = 0x0007;
			/// <summary>
			/// The unique identifier for terminal NEW_LINE
			/// </summary>
			public const int TerminalNewLine = 0x0008;
			/// <summary>
			/// The unique identifier for terminal WHITE_SPACE
			/// </summary>
			public const int TerminalWhiteSpace = 0x0009;
			/// <summary>
			/// The unique identifier for terminal COMMENT_LINE
			/// </summary>
			public const int TerminalCommentLine = 0x000A;
			/// <summary>
			/// The unique identifier for terminal SEPARATOR
			/// </summary>
			public const int TerminalSeparator = 0x000B;
		}
		/// <summary>
		/// Contains the constant IDs for the contexts for this lexer
		/// </summary>
		[GeneratedCodeAttribute("Hime.SDK", "3.5.1.0")]
		public class Context
		{
			/// <summary>
			/// The unique identifier for the default context
			/// </summary>
			public const int Default = 0;
		}
		/// <summary>
		/// The collection of terminals matched by this lexer
		/// </summary>
		/// <remarks>
		/// The terminals are in an order consistent with the automaton,
		/// so that terminal indices in the automaton can be used to retrieve the terminals in this table
		/// </remarks>
		private static readonly Symbol[] terminals = {
			new Symbol(0x0001, "ε"),
			new Symbol(0x0002, "$"),
			new Symbol(0x0003, "id"),
			new Symbol(0x0004, "realNumber"),
			new Symbol(0x0005, "integerNumber"),
			new Symbol(0x0006, "emptySet"),
			new Symbol(0x0007, "stringLiteral"),
			new Symbol(0x0008, "NEW_LINE"),
			new Symbol(0x0009, "WHITE_SPACE"),
			new Symbol(0x000A, "COMMENT_LINE"),
			new Symbol(0x000B, "SEPARATOR"),
			new Symbol(0x002A, "export"),
			new Symbol(0x002B, "{"),
			new Symbol(0x002C, "}"),
			new Symbol(0x002D, "="),
			new Symbol(0x002E, "|"),
			new Symbol(0x002F, ","),
			new Symbol(0x0030, "_"),
			new Symbol(0x0031, "["),
			new Symbol(0x0032, "]"),
			new Symbol(0x0033, ":"),
			new Symbol(0x0034, "("),
			new Symbol(0x0035, ")"),
			new Symbol(0x0036, "->"),
			new Symbol(0x0037, "integer"),
			new Symbol(0x0038, "real"),
			new Symbol(0x0039, "boolean"),
			new Symbol(0x003A, "set"),
			new Symbol(0x003B, "element"),
			new Symbol(0x003C, "graph"),
			new Symbol(0x003D, "string"),
			new Symbol(0x003E, "=="),
			new Symbol(0x003F, "=>"),
			new Symbol(0x0040, "or"),
			new Symbol(0x0041, "and"),
			new Symbol(0x0042, "not"),
			new Symbol(0x0043, "in"),
			new Symbol(0x0044, "subset"),
			new Symbol(0x0045, ">="),
			new Symbol(0x0046, ">"),
			new Symbol(0x0047, "!="),
			new Symbol(0x0048, "<"),
			new Symbol(0x0049, "<="),
			new Symbol(0x004A, "+"),
			new Symbol(0x004B, "-"),
			new Symbol(0x004C, "*"),
			new Symbol(0x004D, "/"),
			new Symbol(0x004E, "mod"),
			new Symbol(0x004F, "^"),
			new Symbol(0x0050, "union"),
			new Symbol(0x0051, "intersection"),
			new Symbol(0x0052, ".V"),
			new Symbol(0x0053, ".E"),
			new Symbol(0x0054, ".src"),
			new Symbol(0x0055, ".dst"),
			new Symbol(0x0056, "true"),
			new Symbol(0x0057, "false") };
		/// <summary>
		/// Initializes a new instance of the lexer
		/// </summary>
		/// <param name="input">The lexer's input</param>
		public GrammarLexer(string input) : base(commonAutomaton, terminals, 0x000B, input) {}
		/// <summary>
		/// Initializes a new instance of the lexer
		/// </summary>
		/// <param name="input">The lexer's input</param>
		public GrammarLexer(TextReader input) : base(commonAutomaton, terminals, 0x000B, input) {}
	}
}
