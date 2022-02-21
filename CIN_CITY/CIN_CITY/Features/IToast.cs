namespace CIN_CITY.Features
{
    // Interface to allow Toasts to be implemented in native code 
    public interface IToast
    {
        void ToastLong(string message);
        void ToastShort(string message);
    }
}
