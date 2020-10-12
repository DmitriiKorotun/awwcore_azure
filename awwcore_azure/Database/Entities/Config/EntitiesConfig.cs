using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace awwcore_azure.Database.Entities.Config
{
    public struct DeveloperConfig
    {
        private const int nameMaxLength = 75,
            websiteMaxLength = 100;

        public int NameMaxLength { get => nameMaxLength; }
        public int WebsiteMaxLength { get => websiteMaxLength; }
    }

    public struct GameConfig
    {
        private const int nameMaxLength = 50,
            descriptionMaxLength = 200;

        public int NameMaxLength { get => nameMaxLength; }
        public int DescriptionMaxLength { get => descriptionMaxLength; }
    }

    public struct GenreConfig
    {
        private const int nameMaxLength = 25;

        public int NameMaxLength { get => nameMaxLength; }
    }

    public struct LanguageConfig
    {
        private const int nameMaxLength = 25;

        public int NameMaxLength { get => nameMaxLength; }
    }

    public struct PlatformConfig
    {
        private const int nameMaxLength = 25;

        public int NameMaxLength { get => nameMaxLength; }
    }

    public struct PublisherConfig
    {
        private const int nameMaxLength = 75,
            websiteMaxLength = 100;

        public int NameMaxLength { get => nameMaxLength; }
        public int WebsiteMaxLength { get => websiteMaxLength; }
    }

    public struct ReviewConfig
    {
        private const int nameMaxLength = 50,
            textMaxLength = 1500;

        public int NameMaxLength { get => nameMaxLength; }
        public int TextMaxLength { get => textMaxLength; }
    }

    public struct UserConfig
    {
        private const int nameMaxLength = 50;

        public int NameMaxLength { get => nameMaxLength; }
    }
}
