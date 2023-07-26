using UniRx;

namespace Base.Classes
{
    public abstract class BaseGenericPresenter<TModel, TView> : BasePresenter
        where TModel : BaseModel
        where TView : BaseView
    {
        protected readonly TModel Model;
        protected readonly TView View;
        
        protected readonly CompositeDisposable Disposable = new CompositeDisposable();

        protected abstract void Init();

        protected BaseGenericPresenter(TModel model, TView view)
        {
            View = view;
            Model = model;
        }

        public void Dispose()
        {
            Disposable.Dispose();
        }
    }
}