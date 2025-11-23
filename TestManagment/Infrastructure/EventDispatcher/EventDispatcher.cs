using System.Collections.Concurrent;
using TestManagment.ApplicationLayer.Interfaces.EventMediator;
using TestManagment.Domain.Events;

namespace TestManagment.Infrastructure.EventDispatcher
{
    public class EventDispatcher : IDomainEventDispatcher
    {
        private static ConcurrentDictionary<Type, Type> HandlerDictionary { get; set; } = new ConcurrentDictionary<Type, Type>();
        private static ConcurrentDictionary<Type ,Type> HandlerWrapperDictionary { get; set; } = new ConcurrentDictionary<Type ,Type>();
        
        private readonly IServiceProvider serviceProvider;
        public EventDispatcher(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task DispatchAsync(IDomainEvent e)
        {
            Type eventType = e.GetType();
            Type handlerType = GetHandlerType(eventType);

            IEnumerable<object?> handlers = serviceProvider.GetServices(handlerType);
            foreach (object? handler in handlers)
            {
                if(handler == null)
                {
                    continue;
                }
                HandlerWrapperBase handlerWrapper = CreateWrapper(handler, eventType);
                await handlerWrapper.Handle(e);
            }
        }

        //Get the wrapper type from the dictionary, then create an instance of the wrapper
        //if not exist, add it to the dictionary
        private static HandlerWrapperBase CreateWrapper(object handler, Type eventType)
        {
            Type handlerWrapperType = HandlerWrapperDictionary.GetOrAdd(eventType,
                    et => typeof(HandlerWrapper<>).MakeGenericType(et));

            return (HandlerWrapperBase)Activator.CreateInstance(handlerWrapperType, handler);
        }

        //Get the rqtHandler type from the dictionary. 
        //if not exist, add it in the dictionary
        private static Type GetHandlerType(Type eventType)
        {
            return HandlerDictionary.GetOrAdd(eventType,
                et => typeof(IDomainEventHandler<>).MakeGenericType(et));
        }
    }

    public abstract class HandlerWrapperBase
    {
        public abstract Task Handle(IDomainEvent e);
    }

    public class HandlerWrapper<T> : HandlerWrapperBase
        where T: IDomainEvent 
    {
        private readonly IDomainEventHandler<T> handler;
        public HandlerWrapper(object handler)
        {
            this.handler = (IDomainEventHandler<T>)handler;
        }
        public override async Task Handle(IDomainEvent e)
        {
            await handler.Publish((T)e);
        }
    }
}
