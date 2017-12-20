using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace Archiver.Utils
{
  internal static class BinarySerializer
  {
    internal static void Serialize<T>(T obj, string destinationPath)
    {
      var serializer = new DataContractSerializer(typeof(T));
      using (var fileStream = File.Create(destinationPath))
      using (var writer = XmlDictionaryWriter.CreateBinaryWriter(fileStream))
      {
        serializer.WriteObject(writer, obj);
      }
    }

    internal static T Deserialize<T>(string destinationPath, List<Type> posibileTypes)
    {
      var serializer = new DataContractSerializer(typeof(T), posibileTypes);
      using (var fileStream = File.Open(destinationPath, FileMode.Open))
      using (var reader = XmlDictionaryReader.CreateBinaryReader(fileStream, XmlDictionaryReaderQuotas.Max))
      {
        return (T)serializer.ReadObject(reader);
      }
    }
  }
}
