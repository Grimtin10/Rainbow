namespace Rainbow.Exceptions {
    public class UnsupportedException : Exception {
        public UnsupportedException() : base() {
        }

        public UnsupportedException(string message) : base(message) { }
    }
}
