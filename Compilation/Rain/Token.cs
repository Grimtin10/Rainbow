namespace Rainbow.Compilation.Rain;

public record Token(string content, TokenType type, string[] metadata);

public enum TokenType 
{ 
    WORD,
    LCBRACKET,
    RCBRACKET,
    LSBRACKET,
    RSBRACKET,
    LVBRACKET,
    RVBRAKET,
    LPAREN,
    RPAREN,
    SEMICOLON,
    COMMA,
    DOT,
    STRING,
    CHAR,
    NUMERIC,
    OPSTAR,
    OPPLUS,
    OPMINUS,
    OPEQUALS,
    COLON,
    OPDIVIDE,
    REF,
    IF,
    WHILE,
    STATIC,
    FIXED,
    SWITCH,
    CASE,
    FALLBACK,
    DEFAULT,
    OPAND,
    OPNULL,
    OPNOT
}