namespace Order.Application.Features.Queries.ViewModels
{
    public class CheckPaymentViewModel
    {
        public string Message { get; private set; }
        public bool IsSuccess { get; private set; }

        public CheckPaymentViewModel(string message, bool ısSuccess)
        {
            Message = message;
            IsSuccess = ısSuccess;
        }
    }
}