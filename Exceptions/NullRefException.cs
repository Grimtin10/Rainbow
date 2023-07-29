namespace Rainbow.Exceptions {
    public class NullRefException : Exception {
        public NullRefException() : base() {
        }

        public NullRefException(string message) : base(message) { }
    }
}
