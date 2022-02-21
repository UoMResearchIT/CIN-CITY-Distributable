namespace CIN_CITY.Features
{
    // Interface to set 'busy' text information on a page
    public interface IBusyIndicator
    {
        // Disable activity indicator
        void SetBusy();

        // Start activity indcator with message
        void SetBusy(string message);
    }
}
