using Microsoft.JSInterop;
using System.Threading.Tasks;


namespace eAdmin.JSHelpers
{
    public static class JSHelpers
    {
        public static ValueTask Toast(this IJSRuntime Js, string message, AlertType alertType = AlertType.success)
        {
            return Js.InvokeVoidAsync("Toast", message, alertType.ToString(), "top center");
        }
        public static ValueTask DisplayMessage(this IJSRuntime js, string message)
        {
            return js.InvokeVoidAsync("Swal.fire", message);
        }
        public static ValueTask<bool> Confirm(this IJSRuntime js, string title = "Delete a record", string message = "Are you sure to delete?", SweetAlertMessageType sweetAlertMessageType = SweetAlertMessageType.question, bool show_cancel = true)
        {
            return js.InvokeAsync<bool>("CustomConfirm", title, message, sweetAlertMessageType.ToString(), show_cancel);
        }
        public static ValueTask<bool> Restore(this IJSRuntime js, string title = "Restore a record", string message = "Are you sure to restore?", SweetAlertMessageType sweetAlertMessageType = SweetAlertMessageType.question, bool show_cancel = true)
        {
            return js.InvokeAsync<bool>("CustomConfirm", title, message, sweetAlertMessageType.ToString(), show_cancel);
        }


        public static ValueTask<bool> HistoryBack(this IJSRuntime js)
        {
            return js.InvokeAsync<bool>("HistoryBack");
        }

        public static ValueTask<bool> Print(this IJSRuntime js)
        {
            return js.InvokeAsync<bool>("PrintPage");
        }

        public static ValueTask<bool> Alert(this IJSRuntime js, string message)
        {
            return js.InvokeAsync<bool>("Alert", message);
        }

        public static ValueTask<bool> Fullscreen(this IJSRuntime js, string id)
        {
            return js.InvokeAsync<bool>("openFullscreen", id);
        }
        public static ValueTask<bool> RefreshPage(this IJSRuntime js)
        {
            return js.InvokeAsync<bool>("RefreshPage");
        }
        public static ValueTask ToogleMenu(this IJSRuntime js)
        {
            return js.InvokeVoidAsync("ToogleMenu");
        }


    }
    public enum AlertType
    {
        warn, error, success, info
    }
    public enum SweetAlertMessageType
    {
        question, warning, error, success, info
    }
}
