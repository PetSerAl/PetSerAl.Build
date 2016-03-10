using System;
namespace PetSerAl.Build {
    internal class BlobEntry : Entry {
        private string treePath;
        private string diskPath;
        private string hash;
        public BlobEntry(string treePath, string diskPath) {
            this.treePath=treePath;
            this.diskPath=diskPath;
        }
        public override string Type => "blob";
        public override string Hash => hash??(hash=Exec.HashObject(treePath, diskPath));
    }
}
