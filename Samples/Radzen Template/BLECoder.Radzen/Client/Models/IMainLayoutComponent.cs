using System.Threading.Tasks;

namespace RadzenTemplate.Client.Models
{
    public interface IMainLayoutComponent
    {
        Task ShowBusyDialog();

        Task CloseDialog();
    }
}
