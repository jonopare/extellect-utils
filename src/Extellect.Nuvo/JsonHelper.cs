using Newtonsoft.Json;
using System.Collections;
using System.IO.Compression;

namespace Extellect.Nuvo
{
    public static class JsonHelper
    {
        private static JsonSerializer PrettySerializer => new JsonSerializer
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore,
        };

        public static string SerializeJson(object item)
        {
            using var text = new StringWriter();
            using var writer = new JsonTextWriter(text);
            PrettySerializer.Serialize(writer, PreSerialize(item));
            return text.ToString();
        }

        public static T DeserializeJson<T>(string json)
        {
            using var text = new StringReader(json);
            using var reader = new JsonTextReader(text);
            var serializer = new JsonSerializer();
            var obj = serializer.Deserialize<T>(reader);
            if (obj == null)
                throw new ApplicationException("JSON deserialization returned null");
            return PostDeserialize(obj);
        }

        public static T Deserialize<T>(Stream stream)
        {
            using var text = new StreamReader(stream);
            using var reader = new JsonTextReader(text);
            var serializer = new JsonSerializer();
            var obj = serializer.Deserialize<T>(reader);
            if (obj == null)
                throw new ApplicationException("JSON deserialization returned null");
            return PostDeserialize(obj);
        }

        public static T Deserialize<T>(TextReader textReader)
        {
            using var reader = new JsonTextReader(textReader);
            var serializer = new JsonSerializer();
            var obj = serializer.Deserialize<T>(reader);
            if (obj == null)
                throw new ApplicationException("JSON deserialization returned null");
            return PostDeserialize(obj);
        }

        public static void Serialize(Stream stream, object item)
        {
            using var text = new StreamWriter(stream);
            using var writer = new JsonTextWriter(text);
            PrettySerializer.Serialize(writer, PreSerialize(item));
        }

        public static T Deserialize<T>(string path)
        {
            using var text = File.OpenText(path);
            using var reader = new JsonTextReader(text);
            var serializer = new JsonSerializer();
            var obj = serializer.Deserialize<T>(reader);
            if (obj == null)
                throw new ApplicationException("JSON deserialization returned null");
            return PostDeserialize(obj);
        }

        public static byte[] CompressJson(object item)
        {
            using var stream = new MemoryStream();
            using (var compressionStream = new GZipStream(stream, CompressionMode.Compress, true))
            using (var streamWriter = new StreamWriter(compressionStream))
            using (var writer = new JsonTextWriter(streamWriter))
            {
                PrettySerializer.Serialize(writer, PreSerialize(item));
            }
            return stream.ToArray();
        }

        public static T DecompressJson<T>(byte[] serialized)
        {
            using var stream = new MemoryStream(serialized, false);
            using var compressionStream = new GZipStream(stream, CompressionMode.Decompress);
            using var streamReader = new StreamReader(compressionStream);
            using var reader = new JsonTextReader(streamReader);
            var serializer = new JsonSerializer();
            var obj = serializer.Deserialize<T>(reader);
            if (obj == null)
                throw new ApplicationException("JSON deserialization returned null");
            return PostDeserialize(obj);
        }

        public static T Copy<T>(T item) where T : class
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            byte[] data;
            using (var memoryStream = new MemoryStream())
            {
                Serialize(memoryStream, item);
                data = memoryStream.ToArray();
            }
            using (var memoryStream = new MemoryStream(data, false))
            {
                return PostDeserialize(Deserialize<T>(memoryStream));
            }
        }

        internal static T PreSerialize<T>(T item)
        {
            if (item is IPreSerializable direct)
                direct.PreSerialize();
            else if (item is IEnumerable<IPreSerializable> genericIEnumerable)
                foreach (var preSerializable in genericIEnumerable)
                    preSerializable.PreSerialize();
            else if (item is IEnumerable nonGenericIEnumerable)
                foreach (var nonGenericItem in nonGenericIEnumerable)
                    if (nonGenericItem is IPreSerializable preSerializable)
                        preSerializable.PreSerialize();
            return item;
        }

        internal static T PostDeserialize<T>(T item)
        {
            if (item is IPostDeserializable direct)
                direct.PostDeserialize();
            else if (item is IEnumerable<IPostDeserializable> genericIEnumerable)
                foreach (var postDeserializable in genericIEnumerable)
                    postDeserializable.PostDeserialize();
            else if (item is IEnumerable nonGenericIEnumerable)
                foreach (var nonGenericItem in nonGenericIEnumerable)
                    if (nonGenericItem is IPostDeserializable postDeserializable)
                        postDeserializable.PostDeserialize();
            return item;
        }
    }
}
