using System;
using System.Collections.Generic;
using System.Text;
namespace PetSerAl.Build {
    internal class TreeEntry : Entry {
        private Dictionary<string, Entry> entries;
        private string hash;
        public TreeEntry() {
            entries=new Dictionary<string, Entry>();
        }
        public override string Type => "tree";
        public override string Hash => hash??(hash=Exec.Mktree(GetMktreeInput()));
        public void AddEntry(string treePath, Entry entry) {
            if(string.IsNullOrEmpty(treePath)) {
                throw new AddEntryException(this, treePath, entry);
            }
            int index = treePath.IndexOf('\\');
            if(index==-1) {
                if(entries.ContainsKey(treePath)) {
                    throw new AddEntryException(this, treePath, entry);
                }
                entries.Add(treePath, entry);
            } else {
                GetOrCreateTreeEntry(treePath.Substring(0, index)).AddEntry(treePath.Substring(index+1), entry);
            }
            hash=null;
        }
        private TreeEntry GetOrCreateTreeEntry(string name) {
            TreeEntry treeEntry;
            Entry entry;
            if(entries.TryGetValue(name, out entry)) {
                if(!(entry is TreeEntry)) {
                    throw new AddEntryException(this, name, entry);
                }
                treeEntry=(TreeEntry)entry;
            } else {
                treeEntry=new TreeEntry();
                entries.Add(name, treeEntry);
            }
            return treeEntry;
        }
        private string GetMktreeInput() {
            StringBuilder sb = new StringBuilder();
            foreach(KeyValuePair<string, Entry> kvp in entries) {
                sb.AppendFormat("{0} {1} {2}\t{3}\n", kvp.Value.Mode, kvp.Value.Type, kvp.Value.Hash, EscapeName(kvp.Key));
            }
            return sb.ToString();
        }
        private string EscapeName(string name) => name.Replace("\\", "\\\\").Replace("\t", "\\t").Replace("\n", "\\n");
    }
}
