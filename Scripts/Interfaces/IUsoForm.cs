using System;
namespace GWG.UsoUIElements
{
    public interface IUsoForm
    {

        public event Action OnClearForm;
        public event Action<object> OnFormDataDisconnection;
        public event Action<object> OnFormDataConnection;
        public void ClearForm();

        public void UpdateDatasource(Object fieldDatasource = null);
    }
}