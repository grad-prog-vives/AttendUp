namespace AttendUp.Mvc.Models
{
    public class Setting
    {
        public int ID { get; set; }

        // Huidige kleuren
        public string ButtonColor { get; set; } = "#ff6600";
        public string BackgroundColor { get; set; } = "#f0f4f8";
        public string? LogoPath { get; set; }

        // Standaardkleuren
        public string DefaultButtonColor { get; set; } = "#ff6600";
        public string DefaultBackgroundColor { get; set; } = "#f0f4f8";

        // Eventbanner
        public string? EventBannerPath { get; set; }

        // Tijdsvenster registratie (minuten)
        public int RegistrationWindowMinutes { get; set; } = 30;
        // Sessie-timeout (min)
        public int BoardSessionTimeoutMinutes { get; set; } = 60;

        public bool StoreIpAddresses { get; set; }
    }
}
