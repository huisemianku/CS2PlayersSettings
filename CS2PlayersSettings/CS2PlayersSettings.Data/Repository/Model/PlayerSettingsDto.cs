using CS2PlayersSettings.Data.Repository.Entities.Players;



namespace CS2_PlayerSettings.Data.Repository.Model
{
    public class PlayerSettingsDto
    {
        public MouseSetting MouseSettings { get; set; } = null!;
        public CrosshairSetting CrosshairSettings { get; set; } = null!;
        public ViewmodelSetting ViewmodelSettings { get; set; } = null!;
        public VideoSetting VideoSettings { get; set; } = null!;
    }
}
