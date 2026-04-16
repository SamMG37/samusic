namespace samusic.Models
{
    // Stores error information for display on error page
    public class ErrorViewModel
    {
        // Holds unique request ID for current error
        public string? RequestId { get; set; }

        // Check whether request ID is available to display
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}