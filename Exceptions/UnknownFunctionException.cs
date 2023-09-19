namespace Rainbow.Exceptions {
    public class UnknownFunctionException : Exception {
        public UnknownFunctionException() : base() {
        }

        public UnknownFunctionException(string message) : base(message) { }
    }
}
