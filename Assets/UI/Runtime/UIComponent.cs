using Extension.UI;
using System;
using UnityEngine;

namespace UI
{
    public class Props<T>
    {
        public int index;
        public T data;
    }

    public class UIComponent : MonoBehaviour
    {
        public Action<string> OnEvent;

        public virtual void Empty()
        {
            gameObject.Set(false);
        }

        public virtual void Upsert()
        {

        }

        public virtual void Event(string param)
        {
            OnEvent?.Invoke(param);
        }
    }

    public class UIComponent<T> : UIComponent
    {
        public Action<bool, Props<T>> OnEventProps;

        [HideInInspector]
        public Props<T> props;

        public virtual void Upsert(T data)
        {
            Upsert(0, data);
        }

        public virtual void Upsert(int index, T data)
        {
            props = new Props<T>()
            {
                index = index,
                data = data,
            };

            if (data == null)
            {
                Empty();
                return;
            }

            gameObject.Set(true);
        }

        public virtual void Event()
        {
            var isSelected = true;

            OnEventProps?.Invoke(isSelected, props);
        }
    }

    //public class UIComponet<T> : MonoBehaviour
    //{
    //    public Action<bool, Props<T>> OnEvent;

    //    [HideInInspector]
    //    public Props<T> props;

    //    protected virtual void Create()
    //    {

    //    }

    //    public virtual void Empty()
    //    {
    //        gameObject.Set(false);
    //    }

    //    public virtual void Upsert(int index, T data)
    //    {
    //        props = new Props<T>()
    //        {
    //            index = index,
    //            data = data,
    //        };

    //        if (data == null)
    //        {
    //            Empty();
    //            return;
    //        }

    //        gameObject.Set(true);
    //    }

    //    protected virtual void Event()
    //    {
    //        var isSelected = true;

    //        OnEvent?.Invoke(isSelected, props);
    //    }
    //}

    //public class UIComponent : UIComponet<string>
    //{
    //    public virtual void Upsert()
    //    {
    //        Upsert(string.Empty);
    //    }

    //    public virtual void Upsert(string data)
    //    {
    //        var index = 0;

    //        base.Upsert(index, data);
    //    }
    //}
}

//using System;
//using UnityEngine;

//namespace UI
//{
//    public class UIComponent : MonoBehaviour
//    {
//        public Action<string> OnEvent;

//        protected virtual void Event(string param)
//        {
//            OnEvent?.Invoke(param);
//        }

//        public virtual void Draw() { }
//    }
//}