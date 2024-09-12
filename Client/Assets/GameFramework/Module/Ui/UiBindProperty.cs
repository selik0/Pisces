/****************
 *@class name:		UiBindProperty
 *@description:		数据绑定的类型（类似于Object）
 *@author:			selik0
*************************************************************************/
namespace PiscesGame
{
    public class UiBindProperty<T>
    {
        //保存真正的值
        private T _value;
        //get时返回真正的值，set时顺便调用值改变事件
        protected T Value
        {
            get => _value;
            set
            {
                _value = value;
                OnValueChanged?.Invoke(value);
            }
        }
        //用event存储值改变的事件
        public event System.Action<T> OnValueChanged;
        //初始化
        public UiBindProperty(T value, System.Action<T> action)
        {
            _value = value;
            OnValueChanged += action;
        }
        public UiBindProperty(T value)
        {
            _value = value;
        }
        public UiBindProperty()
        {
            _value = default;
        }
        public void Update()
        {
            OnValueChanged?.Invoke(_value);
        }
    }
}