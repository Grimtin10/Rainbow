using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.Exceptions {
    public class UnhandledArgumentException : Exception {
        public UnhandledArgumentException(string? message) : base(message) {
        }
    }
}
