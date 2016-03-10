using System;
namespace PetSerAl.Build {
    internal abstract class Entry {
        protected Entry() { }
        public string Mode {
            get {
                switch(Type) {
                    case "blob":
                        return "100644";
                    case "commit":
                        return "160000";
                    case "tree":
                        return "040000";
                    default:
                        throw new ArgumentOutOfRangeException(nameof(Type));
                }
            }
        }
        public abstract string Type { get; }
        public abstract string Hash { get; }
    }
}
