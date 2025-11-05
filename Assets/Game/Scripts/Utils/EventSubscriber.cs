#region _____________________________/ INFOS
//  AUTHOR : Nathan THEOPHILE (2025)
//  Engine : Unity
//  Modular Script
//  Note : MY_CONST, myPublic, m_MyProtected, _MyPrivate, lMyLocal, MyFunc(), pMyParam, onMyEvent, OnMyCallback, MyStruct
#endregion

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Rush.Utils
{
    /// <summary>
    /// SignalSubscriber est une classe moduelaire généreique concue pour renforcer l'indépendance des objets.
    /// Je l'attache à n'importe quel objet à son instanciation en lui passant les paramètres nécessaires et ça l'abonne à un signal donné.
    /// Une fois attachée à un objet elle permet de le mettre en listener de n'importe quel signal en la rappelant.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class EventSubscriber<T> : MonoBehaviour
    {
        #region _________________________/ VALUES
        // I setup a list vazy flm je fais en français pour pouvoir désabo tous les events au cas où y en a plusieurs.
        private readonly List<AttachedEvent> _SubscribedEvents = new List<AttachedEvent>();

        private struct AttachedEvent
        {
            public Component sendingSubject;
            public EventInfo eventName;
            public Delegate listener;
        }

        #endregion

        #region _________________________/ METHODS
        /// <summary>
        /// Methode appelée qund on veut attacher un signal à un objet, en gros on lui passe le 
        /// </summary>
        /// <param name="pSendingSubject">on lui passe le sujet qui contient l'event</param>
        /// <param name="pEventName">le nom de l'event</param>
        /// <param name="pListenerMethod">et la fonction à abonner de cet objet auquel l'event subscriber est attaché</param>
        public void SubscribeToEvent(Component pSendingSubject, string pEventName, Action<T> pListenerMethod)
        {
#if UNITY_EDITOR
            //  _________________________/ DEBUG
            if (pSendingSubject == null || string.IsNullOrEmpty(pEventName) || pListenerMethod == null)
            {
                Debug.LogWarning($"Mauvais abonnement de signal.", this);
                return;
            }
#endif

            Component lSendingSubject = pSendingSubject;
            string lEventName = pEventName;
            Delegate lListenerMethod = pListenerMethod;

            /// on essaie de recup le signal a l'emplacement donné puis on y abonne la methode donnée.
            EventInfo lMatchingEvent = lSendingSubject.GetType().GetEvent(lEventName, BindingFlags.Instance | BindingFlags.Public);

#if UNITY_EDITOR
            //  _________________________/ DEBUG
            if (lMatchingEvent == null)
            {
                Debug.LogWarning($"Event {lEventName} introuvable.");
                return;
            }
#endif

            lMatchingEvent.AddEventHandler(lSendingSubject, lListenerMethod);

            // On oublie pas d'ajouter l'event à la liste à désabo au destroy
            AttachedEvent lEvent = new AttachedEvent
            { sendingSubject = lSendingSubject, eventName = lMatchingEvent, listener = lListenerMethod };
            _SubscribedEvents.Add(lEvent);
        }

        private void OnDisable() => UnsubscrieToAllEvent();
        private void OnDestroy() => UnsubscrieToAllEvent();

        /// <summary>
        /// ON OUBLIE PAS DE SE DESEABONNER SINON BONJOUR LES PROBLEMES C COMME LA CHAINE DE NORMAN
        /// </summary>
        public void UnsubscrieToAllEvent()
        {
            for (int i = _SubscribedEvents.Count - 1; i >= 0; i--)
            {
                AttachedEvent lEvent = _SubscribedEvents[i];
                if (lEvent.sendingSubject != null && lEvent.eventName != null && lEvent.listener != null)
                    lEvent.eventName.RemoveEventHandler(lEvent.sendingSubject, lEvent.listener);
            }
            _SubscribedEvents.Clear();
        }

        #endregion
    }
}
