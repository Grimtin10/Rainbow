using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.Exceptions {
    public class NullRefException : Exception {
        public NullRefException() : base() {
        }

        public NullRefException(string message) : base(message) { }
    }
}
