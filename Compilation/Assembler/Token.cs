namespace Rainbow.Compilation.Assembler {
    internal class Token {
        public TokenType type;
        public string value;

        public Token(TokenType type, string value) {
            this.type = type;
            this.value = value;
        }

        public override string ToString() {
            return type + ": " + value;
        }
    }
}
