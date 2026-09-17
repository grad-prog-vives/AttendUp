using AttendUp.Mvc.Models;

namespace AttendUp.Mvc.Helpers
{
    public static class BrandingHelper
    {
        private static async Task UploadFile(IFormFile file, string fileName)
        {
            var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);
            using var stream = new FileStream(savePath, FileMode.Create);
            await file.CopyToAsync(stream);

        }
        public static void SetNewBrandingColor(Setting existing, Setting model)
        {
            existing.ButtonColor = model.ButtonColor;
            existing.BackgroundColor = model.BackgroundColor;
            existing.DefaultButtonColor = model.DefaultButtonColor;
            existing.DefaultBackgroundColor = model.DefaultBackgroundColor;
            existing.RegistrationWindowMinutes = model.RegistrationWindowMinutes;
        }

        private static string GetFileName(IFormFile file, string assetName) => assetName + Path.GetExtension(file.FileName);

        public static async Task SetLogoPath(IFormFile logoFile, Setting model)
        {
            var logoAssetName = "current-logo";
            await UploadFile(logoFile, GetFileName(logoFile, logoAssetName));
            model.LogoPath = "/images/" + logoAssetName;
        }

        public static async Task SetEventBannerPath(IFormFile eventBannerFile, Setting model)
        {
            var eventBannerAssetName = "event-banner";
            await UploadFile(eventBannerFile, GetFileName(eventBannerFile, eventBannerAssetName));
            model.EventBannerPath = "/images/" + eventBannerAssetName;
        }
    }
}
