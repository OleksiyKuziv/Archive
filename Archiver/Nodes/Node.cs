using System.Runtime.Serialization;

namespace Archiver.Nodes
{
  [DataContract]
  internal abstract class Node
  {
    public abstract void Load(string path);
    public abstract void Serialize(string path);
    public abstract void Save(string path);
  }
}
