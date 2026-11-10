using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AssistenteIA.Persistence
{
    public sealed class CharacterConfigStore
    {
        private static readonly string[] RequiredObjects =
        {
            "body", "face", "eyes", "hair", "expressions", "animations",
            "scene", "lighting", "camera", "voice", "persona", "memory"
        };

        private readonly string directory;

        public CharacterConfigStore(string directory)
        {
            if (string.IsNullOrWhiteSpace(directory))
                throw new ArgumentException("A pasta de configuracoes e obrigatoria.", nameof(directory));

            this.directory = Path.GetFullPath(directory);
        }

        public JObject Load(string fileName)
        {
            var path = ResolvePath(fileName);
            var config = JObject.Parse(File.ReadAllText(path, Encoding.UTF8));
            ValidateCoreV2(config);
            return config;
        }

        public void Save(string fileName, JObject config)
        {
            ValidateCoreV2(config);

            var path = ResolvePath(fileName);
            Directory.CreateDirectory(directory);
            var temporaryPath = path + "." + Guid.NewGuid().ToString("N") + ".tmp";

            try
            {
                using (var stream = new FileStream(temporaryPath, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                using (var writer = new StreamWriter(stream, new UTF8Encoding(false)))
                {
                    writer.Write(config.ToString(Formatting.Indented));
                    writer.Flush();
                    stream.Flush();
                }

                if (File.Exists(path))
                    File.Replace(temporaryPath, path, null);
                else
                    File.Move(temporaryPath, path);
            }
            finally
            {
                if (File.Exists(temporaryPath))
                    File.Delete(temporaryPath);
            }
        }

        private string ResolvePath(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName) ||
                !string.Equals(Path.GetFileName(fileName), fileName, StringComparison.Ordinal) ||
                !string.Equals(Path.GetExtension(fileName), ".json", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Informe apenas um nome de arquivo .json.", nameof(fileName));
            }

            return Path.Combine(directory, fileName);
        }

        private static void ValidateCoreV2(JObject config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            var version = config["schemaVersion"];
            if (version == null || version.Type != JTokenType.Integer || (int)version != 2)
                throw new InvalidDataException("Configuracao ausente ou com schemaVersion nao suportada; esperado v2.");

            RequireString(config, "id", 1, 128);
            RequireString(config, "displayName", 1, 120);
            RequireString(config, "baseModel", 1, 512);

            foreach (var name in RequiredObjects)
                RequireType(config, name, JTokenType.Object);

            RequireType(config, "clothes", JTokenType.Array);
            RequireType(config, "accessories", JTokenType.Array);
        }

        private static void RequireString(JObject config, string name, int minLength, int maxLength)
        {
            var value = config[name];
            if (value == null || value.Type != JTokenType.String)
                throw new InvalidDataException("Campo obrigatorio ausente ou invalido: " + name + ".");

            var text = (string)value;
            if (text.Length < minLength || text.Length > maxLength)
                throw new InvalidDataException("Campo fora do limite permitido: " + name + ".");
        }

        private static void RequireType(JObject config, string name, JTokenType expectedType)
        {
            var value = config[name];
            if (value == null || value.Type != expectedType)
                throw new InvalidDataException("Campo obrigatorio ausente ou invalido: " + name + ".");
        }
    }
}
