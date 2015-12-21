using System.Threading.Tasks;
using Catel.Services;
using VendingMachine.Services.Interfaces;

namespace VendingMachine.Services
{
    class LocalizedMessageService: ILocalizedMessageService
    {
        private readonly IMessageService _messageService;
        private readonly ILanguageService _languageService;

        public LocalizedMessageService(IMessageService messageService, ILanguageService languageService)
        {
            _messageService = messageService;
            _languageService = languageService;
        }



        public Task ShowAsync(string localizationKey)
        {
            return  _messageService.ShowAsync(_languageService.GetString(localizationKey));
        }

        public Task ShowWarningAsync(string localizationKey)
        {

            return _messageService.ShowWarningAsync(_languageService.GetString(localizationKey));
        }
    }
}
