namespace EHTool.UIKit {
    public interface IGUI {
        public void SetOn();
        public void SetOff();
        public void Open();
        public void Open(CallbackMethod callback);
        public void Close();

    }
}