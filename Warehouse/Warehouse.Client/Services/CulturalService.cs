using Microsoft.JSInterop;
using System.Globalization;

namespace Warehouse.Client.Services
{
    public class CultureService(IJSRuntime js)
    {
        private readonly IJSRuntime _js = js;

        ///  on app startup restores saved language
        public async Task LoadCultureAsync()
        {
            var saved = await _js.InvokeAsync<string>("localStorage.getItem", "culture");
            var culture = new CultureInfo(saved ?? "en");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }

        /// language switcher
        public async Task SetCultureAsync(string cultureName)
        {
            await _js.InvokeVoidAsync("localStorage.setItem", "culture", cultureName);
            var culture = new CultureInfo(cultureName);
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            /// reload Blazor picks up the new culture
            await _js.InvokeVoidAsync("location.reload");
        }

        public static string CurrentCulture => CultureInfo.CurrentUICulture.Name;
    }
}
