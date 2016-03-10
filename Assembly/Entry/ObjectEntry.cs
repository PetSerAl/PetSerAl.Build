using System;
namespace PetSerAl.Build {
    internal class ObjectEntry : Entry {
        private string @object;
        private string type;
        private string hash;
        public ObjectEntry(string @object) {
            this.@object=@object;
        }
        public override string Type => type??(type=Exec.CatFile(@object));
        public override string Hash => hash??(hash=Exec.RevParse(@object));
    }
}
