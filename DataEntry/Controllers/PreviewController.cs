using dataentry.Utility;
using dataentry.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace dataentry.Controllers
{
    [AllowAnonymous]
    public class PreviewController : Controller
    {
        private readonly IOptions<Configs> _configs;

        public PreviewController(IOptions<Configs> configs)
        {
            _configs = configs ?? throw new ArgumentNullException(nameof(configs));
        }

        // The spa is particular with urls
        // Ex: https://localhost:5001/sg-comm/properties2/office/details/SG-SMPL-301?view=isLetting
        public IActionResult Index(string homeSiteId, string usageType)
        {
            if (string.IsNullOrWhiteSpace(homeSiteId) || string.IsNullOrWhiteSpace(usageType))
                return NotFound("homesiteId or usageType not found in url");

            var previewSettings = _configs.Value.PreviewSettings;

            // Get country specific site
            var site = previewSettings.Sites
                .Where(x => string.Equals(x.HomeSiteId, homeSiteId, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();

            if (site == null)
                return NotFound($"Preview site {homeSiteId} is not configured");

            // Get config
            var usageTypeConfig = site.SPAConfigPaths
                .Where(x => string.Equals(x.UsageType, usageType, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();

            // Use default config if usage type not found
            if (usageTypeConfig == null)
            {
                var defaultConfig = site.SPAConfigPaths
                .Where(x => string.Equals(x.UsageType, "default", StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();

                if (defaultConfig == null)
                    return NotFound($"Preview usageType {usageType} and default is not configured for site {homeSiteId}");

                usageTypeConfig = defaultConfig;
            }

            // Get Path
            var path = $"{homeSiteId}{site.ControllerPath}{usageType}";

            var model = new PreviewViewModel()
            {
                Component = "spa",
                Config = usageTypeConfig.Path,
                Path = path
            };

            return View(model);
        }
    }
}