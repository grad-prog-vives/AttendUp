namespace AttendUp.Mvc.ViewModels
{
    public class LocationQrCodeViewModel
    {
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public string Url { get; set; }
        public string QrCodeBase64 { get; set; }
    }
}
