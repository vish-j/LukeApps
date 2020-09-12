using System.ComponentModel.DataAnnotations;

namespace LukeApps.Common.Enum
{
    public enum CategoryIT
    {
        [Display(Name = "Anti-Virus")]
        AntiVirus,

        Hardware,
        Internet,

        [Display(Name = "General IT Infrastructure")]
        ITInfrastructure,

        [Display(Name = "Latest Technology")]
        LatestTechnology,

        Network,

        [Display(Name = "Operating System")]
        OperatingSystem,

        Printers,
        Routers,
        Services,
        Software,
        Switches,
        Telephone,
        Testing,

        [Display(Name = "User Support")]
        UserSupport,

        [Display(Name = "User Administration")]
        UserAdministration,

        Computers,
        Others,
    }
}