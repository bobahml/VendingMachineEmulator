using System.Threading.Tasks;


namespace VendingMachine.Services.Interfaces
{
    public interface ILocalizedMessageService
    {
        Task ShowAsync(string localizationKey);
        Task ShowWarningAsync(string localizationKey);
    }
}
