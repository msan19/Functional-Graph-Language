namespace FileGeneratorLib.Tests
{
    public class ExpectedGmlStrings
    {
        // Generate single GML String
        public readonly string Str1 =
            "graph [ " +
                "\n\tdirected 1\n" +
                "\tnode [ " +
                    "\n\t\tid 0\n" +
                "\t]\n" +
                "\tnode [ " +
                    "\n\t\tid 1\n" +
                "\t]\n" +
                "\tnode [ " +
                    "\n\t\tid 2\n" +
                "\t]\n" +
                "\tnode [ " +
                    "\n\t\tid 3\n" +
                "\t]\n" +
                "\tedge [ \n" +
                    "\t\tsource 1\n" +
                    "\t\ttarget 3\n" +
                "\t]\n" +
                "\tedge [ \n" +
                    "\t\tsource 2\n" +
                    "\t\ttarget 4\n" +
                "\t]\n" +
            "]\n";

        public readonly string Str2 = 
            "graph [ " +
                "\n\tdirected 1\n" +
                "\tnode [ " +
                    "\n\t\tid 0\n" +
                    "\t\tnodeValue: 1\n" +
                    "\t\tsomeLabelV: 1\n" +
                "\t]\n" +
                "\tnode [ " +
                    "\n\t\tid 1\n" +
                    "\t\tnodeValue: 2\n" +
                    "\t\tsomeLabelV: 2\n" +
                "\t]\n" +
                "\tnode [ " +
                    "\n\t\tid 2\n" +
                    "\t\tnodeValue: 3\n" +
            "\t]\n" +
                "\tnode [ " +
                    "\n\t\tid 3\n" +
                    "\t\tnodeValue: 4\n" +
            "\t]\n" +
                "\tedge [ \n" +
                    "\t\tsource 1\n" +
                    "\t\ttarget 3\n" +
                    "\t\tsomeLabelE: 1\n" +
                "\t]\n" +
                "\tedge [ \n" +
                    "\t\tsource 2\n" +
                    "\t\ttarget 4\n" +
                    "\t\tsomeLabelE: 2\n" +
                "\t]\n" +
            "]\n";

        
        // Generate list of ExtensionalGraphs
        public readonly string ExpectedGml3 = "";

    }
}