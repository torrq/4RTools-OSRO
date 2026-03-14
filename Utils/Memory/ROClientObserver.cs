using System.Collections.Generic;

namespace _ORTools.Utils
{
    public interface IObserver
    {
        void Update(ISubject subject);
    }

    public interface ISubject
    {
        Message Message { get; }

        void Attach(IObserver observer);

        void Detach(IObserver observer);

        void Notify(Message message);
    }

    public enum MessageCode
    {
        PROCESS_CHANGED,
        PROFILE_CHANGED,
        PROFILE_INPUT_CHANGE,
        TURN_ON,
        TURN_OFF,
        SHUTDOWN_APPLICATION,
        CLICK_ICON_TRAY,
        SERVER_LIST_CHANGED,
        ADDED_NEW_AUTOBUFF_SKILL,
        CHANGED_AUTOSWITCH_SKILL,
        ADDED_NEW_AUTOSWITCH_PETS,
        DEBUG_MODE_CHANGED
    }

    public class Message
    {
        public MessageCode Code { get; }
        public object Data { get; set; }

        public Message()
        { }

        public Message(MessageCode code, object data)
        {
            this.Code = code;
            this.Data = data;
        }
    }

    public class Subject : ISubject
    {
        public Message Message { get; set; } = new Message();
        private readonly List<IObserver> _observers = new List<IObserver>();
        private readonly object _observersLock = new object();

        public void Attach(IObserver observer)
        {
            if (observer == null)
            {
                DebugLogger.Warning("Subject: Attempted to attach a null observer.");
                return;
            }

            lock (_observersLock)
            {
                if (!_observers.Contains(observer))
                    _observers.Add(observer);
            }
        }

        public void Detach(IObserver observer)
        {
            lock (_observersLock)
            {
                _observers.Remove(observer);
            }
            DebugLogger.Debug("Subject: Detached an observer.");
        }

        public void Notify(Message message)
        {
            this.Message = message;
            IObserver[] snapshot;
            lock (_observersLock)
            {
                snapshot = _observers.ToArray();
            }
            foreach (var observer in snapshot)
            {
                observer.Update(this);
            }
        }
    }
}