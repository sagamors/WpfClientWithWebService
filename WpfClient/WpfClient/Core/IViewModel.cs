namespace WpfClient.Core
{ 
    public interface IViewModel<TView>
    {
        bool IsBusy {  get; }
        bool ErrorDataLoading {  get; }
        string ErrorDataLodingMessage {  get; }
        TView View { get; }
    }
}
