using System;
using System.IO;
using AssistenteIA.Persistence;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace AssistenteIA.Tests
{
    public sealed class CharacterConfigStoreTests
    {
        private string directory;
        private CharacterConfigStore store;

        [SetUp]
        public void SetUp()
        {
            directory = Path.Combine(Path.GetTempPath(), "AssistenteIA.Tests", Guid.NewGuid().ToString("N"));
            store = new CharacterConfigStore(directory);
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(directory))
                Directory.Delete(directory, true);
        }

        [Test]
        public void SaveAndLoad_PreservesConfigurationAndUnknownFields()
        {
            var config = CreateValidConfig();
            config["futureField"] = new JObject { ["enabled"] = true };

            store.Save("character.json", config);
            config["displayName"] = "Updated";
            store.Save("character.json", config);
            var loaded = store.Load("character.json");

            Assert.AreEqual(config.ToString(), loaded.ToString());
        }

        [Test]
        public void Save_RejectsUnsupportedSchemaVersion()
        {
            var config = CreateValidConfig();
            config["schemaVersion"] = 3;

            Assert.Throws<InvalidDataException>(() => store.Save("character.json", config));
            Assert.IsFalse(Directory.Exists(directory));
        }

        [Test]
        public void Load_RejectsMalformedJson()
        {
            Directory.CreateDirectory(directory);
            File.WriteAllText(Path.Combine(directory, "character.json"), "{");

            Assert.Throws<JsonReaderException>(() => store.Load("character.json"));
        }

        [Test]
        public void Save_RejectsPathOutsideConfigurationDirectory()
        {
            Assert.Throws<ArgumentException>(() => store.Save("..\\outside.json", CreateValidConfig()));
        }

        private static JObject CreateValidConfig()
        {
            var config = new JObject
            {
                ["schemaVersion"] = 2,
                ["id"] = "char_test",
                ["displayName"] = "Test",
                ["baseModel"] = "characters/test.vrm",
                ["clothes"] = new JArray(),
                ["accessories"] = new JArray()
            };

            var objectFields = new[]
            {
                "body", "face", "eyes", "hair", "expressions", "animations",
                "scene", "lighting", "camera", "voice", "persona", "memory"
            };

            foreach (var field in objectFields)
                config[field] = new JObject();

            return config;
        }
    }
}
