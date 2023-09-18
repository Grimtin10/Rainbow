namespace Rainbow.Compilation.Rain.Lexing;

public class Lexer
{
    public string document { get; set; }
    public List<Token> tokens { get; set; } = new();

    public Lexer(string docpath)
    {
        document = File.ReadAllText(docpath);
    }

    public void Parse()
    {
        //delims '";.,()<>{}[]

        string current = "";
        for(int i = 0; i < document.Length; i++)
        {
            switch(document[i])
            {
                case ' ': case '\n': case '\t': ParseCurrent(ref current); break;

                case '\'':
                {
                    ParseCurrent(ref current);

                    //add character reading logic

                    break;
                }

                case '\"':
                {
                    ParseCurrent(ref current);

                    //add character reading logic

                    break;
                }

                default:
                {
                    current = current + document[i];
                    break;
                }
            }
        }
    }

    private void ParseCurrent(ref string current)
    {
        if(current.Length > 0)
        {
            decimal dnumeric;
            long numeric;
            if(long.TryParse(current, out numeric))
            {
                tokens.Add(new Token(current, TokenType.NUMERIC, new string[0]));

                current = "";

                return;
            }

            if(decimal.TryParse(current, out dnumeric))
            {
                tokens.Add(new Token(current, TokenType.DNUMERIC, new string[0]));

                current = "";

                return;
            }

            switch(current) { default: break; }

            tokens.Add(new Token(current, TokenType.WORD, new string[0]));
            current = "";
        }
    }
}