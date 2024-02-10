namespace Cr.UI.Data.StateManagement
{
    public class BlazorDisplaySpinnerAutomaticallyHttpMessageHandler : DelegatingHandler
    {
        private readonly SpinnerState _spinnerState;
        public BlazorDisplaySpinnerAutomaticallyHttpMessageHandler(SpinnerState spinnerState)
        {
            _spinnerState = spinnerState;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            _spinnerState.Show();
            //  await Task.Delay(1000);
            var response = await base.SendAsync(request, cancellationToken);
            _spinnerState.Hide();
            return response;
        }
    }
}
