using System;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace TennisBall.Infrastructure
{
    public static class ObjectCloner
    {
        public static T Clone<T>(T source)
        {
            if (!typeof(T).IsSerializable || !typeof(T).BaseType.IsSerializable)
            {
                throw new ArgumentException("The type must be serializable", "source");
            }

            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
    }
}