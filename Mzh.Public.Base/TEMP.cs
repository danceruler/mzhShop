using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mzh.Public.Base
{
    class TEMP
    {
        /// <summary>
        /// winform获取控件某个事件委托
        /// </summary>
        /// <param name="component"></param>
        /// <param name="EventName"></param>
        /// <param name="EventHandlerTypeName"></param>
        /// <returns></returns>
        public Delegate[] GetComponentEventDelegate(Component component, string EventName, string EventHandlerTypeName)
        {
            Type componentType = component.GetType();
            PropertyInfo eventsPropertyInfo = componentType.GetProperty("Events", BindingFlags.Instance | BindingFlags.NonPublic);
            EventHandlerList eventHanlderList = eventsPropertyInfo.GetValue(component, null) as EventHandlerList;
            FieldInfo HeadFieldInfo = eventHanlderList.GetType().GetField("head", BindingFlags.Instance | BindingFlags.NonPublic);
            object HeadObject = HeadFieldInfo.GetValue(eventHanlderList);

            do
            {
                FieldInfo[] fieldInfoList = componentType.GetFields(BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic);
                foreach (FieldInfo fieldInfo in fieldInfoList)
                {
                    object fieldValue = fieldInfo.GetValue(component);
                    if (fieldValue != null)
                    {
                        Type fieldType = fieldValue.GetType();
                        if (fieldType.Name == EventHandlerTypeName && (fieldValue as Delegate) != null)
                        {
                            return (fieldValue as Delegate).GetInvocationList();
                        }
                        else if (fieldType.Name == typeof(Object).Name)
                        {
                            if (fieldInfo.Name.IndexOf(EventName, StringComparison.OrdinalIgnoreCase) > -1)
                            {
                                if (HeadObject != null)
                                {
                                    Delegate delegateObject = eventHanlderList[fieldValue];
                                    if (delegateObject != null)
                                        return delegateObject.GetInvocationList();
                                }
                            }
                        }
                    }
                }
                componentType = componentType.BaseType;
            } while (componentType != null);

            if (HeadObject != null)
            {
                object ListEntry = HeadObject;
                Type ListEntryType = ListEntry.GetType();
                FieldInfo handlerFieldInfo = ListEntryType.GetField("handler", BindingFlags.Instance | BindingFlags.NonPublic);
                FieldInfo keyFieldInfo = ListEntryType.GetField("key", BindingFlags.Instance | BindingFlags.NonPublic);
                FieldInfo nextFieldInfo = ListEntryType.GetField("next", BindingFlags.Instance | BindingFlags.NonPublic);

                while (ListEntry != null)
                {
                    Delegate handler = handlerFieldInfo.GetValue(ListEntry) as Delegate;
                    object key = keyFieldInfo.GetValue(ListEntry);
                    ListEntry = nextFieldInfo.GetValue(ListEntry);

                    if (handler != null && handler.GetType().Name == EventHandlerTypeName)
                        return handler.GetInvocationList();
                }
            }
            return null;
        }
    }
}
